using System.IO;
using System.Threading.Tasks;

namespace GasPrice.Core.Common.Infrastructure
{
    public interface IFtpClient
    {
        void Autheticate(string host, string username, string password, string path);
        Task<FileInfo> DownloadAsync(string remoteFile, string localFile);
        Task<object> UploadAsync(string fileName, string dir = "");
        bool DeleteFile(string deleteFile);
        bool RenameFile(string currentFileNameAndPath, string newFileName);
        string GetFileCreatedDateTime(string fileName);
        string GetFileSize(string fileName);

        /// <summary>
        /// Create a New Directory on the FTP Server 
        /// </summary>
        /// <param name="localDir">The new directory.</param>
        /// <returns></returns>
        bool CreateDirectory(string localDir = "");

        /// <summary>
        /// Check whether the remote directory the exists.
        /// </summary>
        /// <param name="ftpPath">The FTP path.</param>
        /// <returns></returns>
        bool DirectoryExists(string ftpPath);
        string[] DirectoryListSimple(string ftpPath);
        string[] DirectoryListDetailed(string directory);
        
    }
}