using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.BlobServices
{
    public abstract class BlobManager : IBlobManager
    {
        protected readonly string emptyFileName = "directory.empty";
        protected string GlobalPrefix { get; }
        protected virtual TimeSpan DefaultUrlTimeout => TimeSpan.FromDays(1);

        public BlobManager(string globalPrefix)
        {
            if (!string.IsNullOrEmpty(globalPrefix))
                GlobalPrefix = globalPrefix;
        }

        public abstract Task<Stream> ContinuousUpload(string path);

        public abstract Task CopyAsync(string source, string target);

        public abstract Task CreateFolder(string path);

        public abstract Task DeleteAsync(string key);

        public abstract Task<int> DeleteFolderAsync(string path);

        public abstract Task<IReadOnlyCollection<EntryInfo>> GetContentsAsync(string path, bool foldersOnly);

        public abstract Task<string> GetDownloadUrl(string path);

        public abstract Task<string> GetDownloadUrl(string path, TimeSpan expiresIn);

        public abstract Task<Stream> GetFileAsync(string path);

        public abstract Task MoveAsync(string source, string target);

        public abstract Task UploadAsync(string key, byte[] content);

        public abstract Task UploadAsync(string key, Stream content);

        public abstract Task UploadAsync(string key, string localFilePath);

        protected void SplitKey(string key, out string container, out string fileName, bool throwOnNoKey)
        {
            container = string.IsNullOrWhiteSpace(key) ? "" : GetContainer(key);
            fileName = string.IsNullOrWhiteSpace(key) ? "" : GetKey(key);
            if (throwOnNoKey && fileName == null)
                throw new InvalidOperationException("key must contain at least one containing folder");
        }

        protected string GetContainer(string fullPath)
        {
            Guard.NotNullOrEmpty(fullPath, "fullPath");
            var firstIndex = 0;
            if (fullPath.StartsWith("/"))
                firstIndex = 1;
            var firstSlash = fullPath.IndexOf('/', firstIndex);
            if (firstSlash == -1)
                return fullPath;
            return fullPath.Substring(firstIndex, firstSlash);
        }

        protected string GetKey(string fullPath)
        {
            Guard.NotNullOrEmpty(fullPath, "fullPath");
            var firstIndex = 0;
            if (fullPath.StartsWith("/"))
                firstIndex = 1;
            var firstSlash = fullPath.IndexOf('/', firstIndex);
            if (firstSlash == -1)
                return null;
            return fullPath.Substring(firstSlash + 1);
        }

        protected (string, string) ProcessWithGlobalPrefix(string path, bool throwsOnAbsentKey)
        {
            path ??= "";
            if (path.StartsWith("/"))
            {
                path = path.Substring(1);
            }
            path = PrependGlobalPrefix(path);
            SplitKey(path, out var containerName, out var fileName, throwsOnAbsentKey);
            return (containerName, fileName);
        }

        protected string PrependGlobalPrefix(string path)
        {
            if (string.IsNullOrWhiteSpace(GlobalPrefix))
                return path;
            string cleanPrefix = GetCleanPrefix();
            var pathStartIndex = 0;
            if (path.StartsWith("/"))
            {
                pathStartIndex = 1;
            }
            return $"{cleanPrefix}/{path.Substring(pathStartIndex)}";
        }

        protected string GetCleanPrefix()
        {
            var startIndex = 0;
            var length = GlobalPrefix.Length;
            if (GlobalPrefix.StartsWith("/"))
            {
                startIndex = 1;
                length--;
            }
            if (GlobalPrefix.EndsWith("/"))
            {
                length--;
            }
            var cleanPrefix = GlobalPrefix.Substring(startIndex, length);
            return cleanPrefix;
        }

        protected void RemovePrefixes(List<EntryInfo> entries)
        {
            for (var i = entries.Count - 1; i >= 0; i--)
            {
                var item = entries[i];
                item.FullPath = RemoveGlobalPrefix(item.FullPath);
                if (item.IsFolder)
                {
                    RemovePrefixes(((FolderInfo)item).Children);
                }
                else if (item.Path == emptyFileName)
                {
                    entries.RemoveAt(i);
                }
            }
        }

        protected string Combine(params string[] pathComponents)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < pathComponents.Length; i++)
            {
                var item = pathComponents[i];
                if (item.StartsWith('/'))
                {
                    sb.Append(item.Substring(1));
                }
                else
                {
                    sb.Append(item);
                }
                if (i < pathComponents.Length - 1 && sb[^1] != '/')
                {
                    sb.Append('/');
                }
            }
            return sb.ToString();
        }

        protected string RemoveGlobalPrefix(string input)
        {
            if (string.IsNullOrWhiteSpace(GlobalPrefix))
            {
                return input;
            }
            var prefix = GetCleanPrefix();
            var start = prefix.Length + 1;
            if (input.StartsWith("/"))
            {
                start++;
            }
            return input.Substring(start);
        }
    }
}
