using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace HyperMarket {
    public class NetworkHelper2 {
        private String UserAgent;

        public NetworkHelper2(String userAgent) {
            Guard.NotNullOrEmpty(userAgent, "userAgent");
            UserAgent = userAgent;
        }

        public HttpResponse MakeRemoteRequest(HttpRequest info) {
            Guard.NotNull(info, "info");
            info.Validate();
            var req = GetRequest(info);

            var length = SetupBody(info, req);
            if ((info.HttpVerb == HttpVerb.Put || info.HttpVerb == HttpVerb.Post) && req.ContentLength == -1)
                req.ContentLength = length ?? 0;
            try {
                return ProcessResponse((HttpWebResponse)req.GetResponse());
            } catch (WebException ex) {
                if (!info.ProceedOnError || ex.Response == null)
                    throw;
                var result = ProcessResponse((HttpWebResponse)ex.Response);
                result.ErrorOccurred = true;
                return result;
            }
        }
        public Task<HttpResponse> MakeRemoteRequestAsync(HttpRequest info) {
            return Task.Factory.StartNew(() => MakeRemoteRequest(info));
        }
        public HttpResponse MakeRemoteRequest(String url) {
            return MakeRemoteRequest(url, HttpVerb.Get);
        }
        public Task<HttpResponse> MakeRemoteRequestAsync(String url) {
            return Task.Factory.StartNew(() => MakeRemoteRequest(url));
        }
        public HttpResponse MakeRemoteRequest(String url, HttpVerb verb) {
            return MakeRemoteRequest(url, verb, null);
        }
        public Task<HttpResponse> MakeRemoteRequestAsync(String url, HttpVerb verb) {
            return Task.Factory.StartNew(() => MakeRemoteRequest(url, verb));
        }
        public HttpResponse MakeRemoteRequest(String url, HttpVerb verb, String body) {
            return MakeRemoteRequest(new HttpRequest {
                Url = url,
                HttpVerb = verb,
                PostBody = body
            });
        }
        public Task<HttpResponse> MakeRemoteRequestAsync(String url, HttpVerb verb, String body) {
            return Task.Factory.StartNew(() => MakeRemoteRequest(url, verb, body));
        }
        public void SendEmail(String to, String subject, String body, params KeyValuePair<String, Byte[]>[] attachments) {
            SendEmail(new[] { to }, subject, body, attachments);
        }
        public void SendEmail(String[] recipients, String subject, String body, params KeyValuePair<String, Byte[]>[] attachments) {
            var message = new MailMessage();
            var fsHelper = new FileSystemHelper();

            foreach (var item in recipients)
                message.To.Add(item.ToLower());
            foreach (var att in attachments)
                message.Attachments.Add(new Attachment(new MemoryStream(att.Value), att.Key, fsHelper.GetContentType(att.Key)));
            message.Body = body;
            message.IsBodyHtml = true;
            message.Subject = subject;
            using (var sender = new SmtpClient())
                try {
                    sender.Send(message);
                } catch (Exception ex) {
                    var super = ErrorHelper.Smtp("Failed to send", ex);
                    super.Data.Add("recipients", recipients);
                    super.Data.Add("Host", sender.Host);
                    super.Data.Add("Port", sender.Port);
                    super.Data.Add("UseDefaultCredentials", sender.UseDefaultCredentials);
                    throw super;
                }
        }

        private static HttpResponse ProcessResponse(HttpWebResponse response) {
            var res = (Stream)null;
            using (var rs = response.GetResponseStream())
                res = rs.CopyToMemory();
            if (res.Position != 0)
                res.Position = 0;
            return new HttpResponse(response.StatusCode, response.Headers.AllKeys.ToDictionary(x => x, x => response.Headers[x]), res);
        }
        private static Int64? SetupBody(HttpRequest info, HttpWebRequest reqObject) {
            if (info.MultipartPostParams.Any())
                return FillMultipart(info, reqObject);
            if (info.PostBinaryBody != null && info.PostBinaryBody.Any()) {
                if (reqObject.ContentType == null)
                    reqObject.ContentType = HttpRequest.ContentTypeFormUrlEncoded;
                using (var rs = reqObject.GetRequestStream()) {
                    rs.Write(info.PostBinaryBody);
                    return info.PostBinaryBody.LongLength;
                }
            }
            if (!String.IsNullOrEmpty(info.PostBody)) {
                if (reqObject.ContentType == null)
                    reqObject.ContentType = HttpRequest.ContentTypeFormUrlEncoded;
                using (var rs = reqObject.GetRequestStream()) {
                    var bytes = Encoding.UTF8.GetBytes(info.PostBody);
                    rs.Write(bytes);
                    return bytes.LongLength;
                }
            }
            if (info.PostParams.Any()) {
                if (reqObject.ContentType == null)
                    reqObject.ContentType = HttpRequest.ContentTypeFormUrlEncoded;
                using (var rs = reqObject.GetRequestStream()) {
                    var result = info.PostParams.Aggregate(new StringBuilder(), (sb, x) => sb.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(x.Key), HttpUtility.UrlEncode(x.Value)), x => x.Length > 0 ? x.RemoveLast().ToString() : x.ToString());
                    var bytes = Encoding.UTF8.GetBytes(result);
                    rs.Write(bytes);
                    return bytes.LongLength;
                }
            }
            return null;
        }
        /// <summary>
        /// Populates the body of the request and returns the actual content length
        /// </summary>
        /// <param name="info"></param>
        /// <param name="reqObject"></param>
        /// <returns></returns>
        private static Int64 FillMultipart(HttpRequest info, HttpWebRequest reqObject) {
            var ms = new MemoryStream();
            var @params = info.MultipartPostParams;
            var boundary = String.Format("----------{0:N}", Guid.NewGuid());
            reqObject.ContentType = String.Format("multipart/form-data; boundary={0}", boundary);
            var needsNewLine = false;
            foreach (var item in @params) {
                if (needsNewLine)
                    ms.WriteText("\r\n");
                needsNewLine = true;
                var header = (String)null;
                if (item.FileName != null && item.ContentType != null)
                    header = String.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: {3}\r\n\r\n", boundary, item.ParamName, item.FileName, item.ContentType);
                else
                    header = String.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n", boundary, item.ParamName);
                ms.WriteText(header);
                ms.Write(item.Value);
            }
            ms.WriteText(String.Format("\r\n--{0}--\r\n", boundary));
            ms.Position = 0;
            using (var rs = reqObject.GetRequestStream())
                ms.CopyTo(rs);
            return ms.Length;
        }
        private HttpWebRequest GetRequest(HttpRequest info) {
            var url = info.GetUrl();
            var req = (HttpWebRequest)WebRequest.Create(url);
            
            var headers = info.GetAllHeaders();
            var typedHeaders = headers.Keys.OfType<HttpRequestHeader>().ToDictionary(x => x, x => headers[x]);
            var stringHeaders = headers.Keys.OfType<String>().ToDictionary(x => x, x => headers[x]);
            req.Method = info.HttpVerb.ToString().ToUpper();
            req.UserAgent = info.UserAgent ?? UserAgent;
            info.Timeout.Try(x => {
                if (x.Value > TimeSpan.Zero)
                    req.Timeout = (Int32)x.Value.TotalMilliseconds;
            });
            if (!String.IsNullOrWhiteSpace(info.UserName))
                req.Headers["Authorization"] = String.Format("Basic {0}", String.Format("{0}:{1}", info.UserName, info.Password).ToByte().ToBase64());
            if (!String.IsNullOrWhiteSpace(info.BearerToken)) 
                req.Headers["Authorization"] = String.Format("Bearer {0}", info.BearerToken);
            
            if (info.EmulateAjax)
                req.Headers["X-Requested-With"] = "XMLHttpRequest";
            Action<HttpRequestHeader, Action<String>> setUpHeader = (x, y) => {
                if (typedHeaders.ContainsKey(x)) {
                    y(typedHeaders[x]);
                    typedHeaders.Remove(x);
                }
            };
            if (info.Cookies != null && info.Cookies.Any()) {
                req.CookieContainer = new CookieContainer();
                info.Cookies.ForEach(x => req.CookieContainer.Add(x));
            }

            setUpHeader(HttpRequestHeader.ContentType, x => req.ContentType = x);
            setUpHeader(HttpRequestHeader.Accept, x => req.Accept = x);
            setUpHeader(HttpRequestHeader.Referer, x => req.Referer = x);
            setUpHeader(HttpRequestHeader.Connection, x => req.Connection = x);

            foreach (var header in typedHeaders)
                req.Headers.Add(header.Key, header.Value);
            foreach (var header in stringHeaders)
                req.Headers.Add(header.Key, header.Value);

            return req;
        }
        
    }
}