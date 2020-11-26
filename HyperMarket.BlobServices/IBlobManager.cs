using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HyperMarket.BlobServices
{
    public interface IBlobManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        Task UploadAsync(string key, byte[] content);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        Task UploadAsync(string key, Stream content);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="localFilePath"></param>
        /// <returns></returns>
        Task UploadAsync(string key, string localFilePath);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task DeleteAsync(string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Prefix path to query from. To get root folder query ""</param>
        /// <param name="includeFiles"></param>
        /// <returns></returns>
        Task<IReadOnlyCollection<EntryInfo>> GetContentsAsync(string path, bool foldersOnly);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<Stream> GetFileAsync(string path);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<string> GetDownloadUrl(string path);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="expiresIn"></param>
        /// <returns></returns>
        Task<string> GetDownloadUrl(string path, TimeSpan expiresIn);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        Task MoveAsync(string source, string target);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        Task CopyAsync(string source, string target);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task CreateFolder(string path);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<int> DeleteFolderAsync(string path);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<Stream> ContinuousUpload(string path);
    }
}
