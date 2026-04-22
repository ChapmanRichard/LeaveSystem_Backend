using Api.Common.Helper;
using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Api.Common
{
    public static class FileHelper
    {
        /// <summary>
        /// 新旧文件对比，文件是否修改
        /// </summary>
        /// <param name="oldFilePath"></param>
        /// <param name="newFilePath"></param>
        /// <returns></returns>
        public static bool FileIsModify(string oldFilePath, string newFilePath)
        {

            bool isModify = false;
            FileInfo oldFileInfo = new FileInfo(oldFilePath);
            FileInfo newFileInfo = new FileInfo(newFilePath);
            if (oldFileInfo.CreationTime != newFileInfo.CreationTime)
            {
                isModify = true;
            }
            else if (oldFileInfo.LastWriteTime != newFileInfo.LastWriteTime)
            {
                if (GetMd5ByFile(oldFilePath) != GetMd5ByFile(newFilePath))
                {
                    isModify = true;
                }
            }
            return isModify;
        }

        /// <summary>
        /// 将文件转换为Md5字符串（可作为文件对比修改用）
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetMd5ByFile(string filePath)
        {
            using (FileStream fs = File.OpenRead(filePath))
            {
                using (var crypto = MD5.Create())
                {
                    var md5Hash = crypto.ComputeHash(fs);
                    return md5Hash.Aggregate(string.Empty, (res, b) => res = res + b.ToString("X2"));
                }
            }
        }
        /// <summary>
        /// 读取根目录下面的文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static byte[] ReadFile(string fileName, string rootPath)
        {
            string path = GetFilePath(fileName, rootPath);
            return Read(path);
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <param name="buffer">文件数据</param>
        /// <returns></returns>
        public static void WriteFile(string fileName, string rootPath, byte[] buffer)
        {
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
            string path = GetFilePath(fileName, rootPath);
            //using (FileStream fs = new FileStream(path, FileMode.CreateNew))
            //{
            //    fs.Write(buffer, 0, buffer.Length);
            //    fs.Close();
            //};

            FileInfo file = new FileInfo(path);

            FileStream fs = file.Create();
            fs.Write(buffer, 0, buffer.Length);
            fs.Close();
        }
        public static void WriteFile(string fileName, string rootPath, string result)
        {
            string path = GetFilePath(fileName, rootPath);
#if NET6_0_OR_GREATER || NET5_0_OR_GREATER
            // FileStream.Lock is not supported on all platforms (e.g., macOS/OSX)
            using (FileStream fs = new FileStream(path, FileMode.CreateNew))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(result);
                sw.Flush();
            }
#else
    using (FileStream fs = new FileStream(path, FileMode.CreateNew))
    {
        fs.Lock(0, fs.Length);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(result);
        fs.Unlock(0, fs.Length);
        sw.Flush();
    }
#endif
        }

        /// <summary>
        /// 删文件
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns></returns>
        public static void RemoveFile(string fileName, string rootPath)
        {
            string path = GetFilePath(fileName, rootPath);
            File.Delete(path);
        }
        /// <summary>
        /// 删文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static void RemoveFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// 获取文件绝对路径
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns></returns>
        public static string GetFilePath(string fileName, string rootPath)
        {
            return rootPath + fileName;
        }

        public static string GetDocPath(string FileSavePath, string DocDesc, string DocGuid)
        {
            return FileSavePath + "\\" + Path.GetFileNameWithoutExtension(DocDesc) + "_" + DocGuid + Path.GetExtension(DocDesc);
        }
        public static string GetPath(string DocPath)
        {
            return Path.GetDirectoryName(DocPath) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(DocPath);
        }
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filePath">文件绝对路径</param>
        /// <returns></returns>
        public static byte[] Read(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }

        /// <summary>
        /// 获取文件的传输类型
        /// </summary>
        /// <param name="fileExt">文件扩展名</param>
        /// <returns></returns>
        public static string getContentType(string fileExt)
        {
            string contentType = "";
            switch (fileExt?.ToLower())
            {
                case "jpg":
                case "jpeg":
                case "gif":
                case "png":
                case "webp":
                    contentType = "image/" + fileExt.ToLower();
                    break;
                case "bmp":
                    contentType = "application/x-bmp";
                    break;
                case "pdf":
                case "csv":
                    contentType = "application/" + fileExt.ToLower();
                    break;
                case "txt":
                    contentType = "text/plain";
                    break;
                case "html":
                    contentType = "text/html";
                    break;
                default:
                    contentType = "application/octet-stream";
                    break;
            }

            return contentType;
        }

        public static string getFileExtension(string fileName)
        {
            if (fileName == null) return null;

            var tmpStr = fileName.Split('.');
            if (tmpStr.Length > 1)
            {
                var ext = tmpStr[tmpStr.Length - 1];
                return ext.ToLower();
            }
            return null;
        }

        public static string ComputeSize(decimal fileLength)
        {
            if (fileLength < 1024)
            {
                return fileLength.ToString() + " bytes";
            }
            else if (fileLength >= 1024 && fileLength < 1024 * 1024)
            {
                return Math.Round(Convert.ToDecimal(fileLength) / 1024, 1).ToString() + " KB";
            }
            else if (fileLength >= 1024 * 1024 && fileLength < 1024 * 1024 * 1024)
            {
                return Math.Round(Convert.ToDecimal(fileLength) / 1024 / 1024, 1).ToString() + " MB";
            }
            else if (fileLength >= 1024 * 1024 * 1024)
            {
                return Math.Round(Convert.ToDecimal(fileLength) / 1024 / 1024 / 1024, 3).ToString() + " GB";
            }
            return "unknown size";
        }
        public static decimal ComputeSizeToG(decimal fileLength)
        {
            return Math.Round(Convert.ToDecimal(fileLength) / 1024 / 1024 / 1024, 3);
        }
        public static void CreateDirectory(string rootPath)
        {
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
        }
        public static async Task<byte[]> FilesZipToByteArray(List<string> fileNames, List<string> guids, AttachmentOwnerType attachmentOwnerType)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    string category = string.Empty;
                    for (int i = 0; i < fileNames.Count; i++)
                    {
                        var file = fileNames[i];
                        if (System.IO.File.Exists(file))
                        {
                            FileInfo item = new FileInfo(file);
                            if (attachmentOwnerType == AttachmentOwnerType.SubmissionDrawingList)
                            {
                                var pathlst = Path.GetDirectoryName(item.FullName).Split("\\");
                                category = pathlst[pathlst.Length - 1] + "/";
                            }
                            ZipArchiveEntry zipEntry = zipArchive.CreateEntry(category + item.Name.Replace("_" + guids[i], ""));
                            await using var entryStream = zipEntry.Open();
                            Stream ss = new MemoryStream(System.IO.File.ReadAllBytes(item.FullName));
                            await ss.CopyToAsync(entryStream);
                            await ss.DisposeAsync();
                        }
                    }
                    zipArchive.Dispose();
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    await memoryStream.FlushAsync();
                    return memoryStream.ToArray();
                }
            }
        }

        public static string GetBase64StringByPath(string path)
        {
            string image = null;
            var bytes = System.IO.File.ReadAllBytes(path);
            image = Convert.ToBase64String(bytes);

            return image;

        }

        public static string GetPicWidthAndHeight(string path)
        {
            var suffix = path.Substring(path.LastIndexOf("."));
#if WINDOWS
    if ((suffix == ".png" || suffix == ".jpg" || suffix == ".gif" || suffix == ".bmp"))
    {
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            Image img = Image.FromStream(fs);
            return img.Width + "x" + img.Height;
        }
    }
    else
    {
        return "";
    }
#else
            // 非 Windows 平台不支持 System.Drawing.Image
            return "";
#endif
        }

        public static System.Data.DataTable ReadFile_CSV(string url)
        {
            TxtLoadOptions lo = new TxtLoadOptions();
            lo.Encoding = Encoding.Default;//设置编码方式

            Workbook workbook = new Workbook(url, lo);

            //配置读取文件的类型（CSV）
            workbook.FileFormat = FileFormatType.Csv;//可在此配置Excel文件类型

            Worksheet worksheet = workbook.Worksheets[0];//默认第一个Sheet页

            Cells cells = worksheet.Cells;

            //读取到DataTable中            

            System.Data.DataTable dt = cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1, true);

            //释放资源         

            workbook = null;
            worksheet = null;
            return dt;
        }
        public static string ReplaceWrongCharactorOfFileName(string str)
        {
            if (str.IsEmpty())
                return str;
            str = str.Replace("\\", string.Empty);
            str = str.Replace("/", string.Empty);
            str = str.Replace(":", string.Empty);
            str = str.Replace("*", string.Empty);
            str = str.Replace("?", string.Empty);
            str = str.Replace("\"", string.Empty);
            str = str.Replace("<", string.Empty);
            str = str.Replace(">", string.Empty);
            str = str.Replace("|", string.Empty);
            //str = str.Replace(" ", string.Empty);
            return str;
        }
        public static string SanitizeFileNameForHeader(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return "file";

            // 移除常见非法字符
            var invalidChars = Path.GetInvalidFileNameChars().Concat(new[] { '\r', '\n', '\t' });
            foreach (var c in invalidChars)
            {
                fileName = fileName.Replace(c.ToString(), string.Empty);
            }

            // 进行URL编码
            return HttpUtility.UrlEncode(fileName, Encoding.UTF8);
        }
    }
}
