using System.Text.Json;
using TestProject.Controllers;
using static TestProject.Models.FileInfoDTOs;

namespace TestProject.Actions
{
    public class FileUtils
    {
        public static DirectoryInfoResponseDto? GetDirectoryInfo(
            string inputPath)
        //            string directoryPath,
        //            string searchPattern = "*",
        //            bool recursive = false)
        {

            string directoryPath = "";
            if (inputPath.Equals(""))
            {
                inputPath = "\\TestFileSystem";
                directoryPath = Directory.GetCurrentDirectory() + inputPath;
            }
            else
            {
                directoryPath = inputPath;
            }

            var di = new DirectoryInfo(directoryPath);

            if (!di.Exists) return null;

            var files = di.GetFiles().Select(f => new FileInfoResponseDto
                {
                    FullPath = f.FullName,
                    Name = f.Name,
                    SizeBytes = f.Length,
                    CreatedUtc = f.CreationTimeUtc,
                    ModifiedUtc = f.LastWriteTimeUtc,
                    IsReadOnly = f.IsReadOnly,
                    Extension = f.Extension
                })
                .ToList();

            var subDirs = di.GetDirectories().Select(d => new SubDirectoryResponseDto
            {
                FullPath = d.FullName,
                Name = d.Name,
            }).ToList();

            return new DirectoryInfoResponseDto
            {
                FullPath = di.FullName,
                Name = di.Name,
                CreatedUtc = di.CreationTimeUtc,
                ModifiedUtc = di.LastWriteTimeUtc,
                Files = files,
                SubDirectories = subDirs
            };



            //List<string> files = new List<string>();
            //try
            //{
            //    files = Directory.GetFileSystemEntries(directoryPath).ToList();

            //}
            //catch (Exception e)
            //{
            //    return NotFound(new ApiResponseDto
            //    {
            //        Success = false,
            //        Data = JsonSerializer.Serialize("Bad Path"),
            //        Message = "Bad Path"
            //    });
            //}

            //int fileCount = 0;
            //int folderCount = 0;

            //foreach (string file in files)
            //{
            //    TestData td = new();
            //    td.FileName = Path.GetFileName(file);

            //    if (!Directory.Exists(file))
            //    {
            //        FileInfo fileInfo = new FileInfo(file);
            //        if (fileInfo.Exists)
            //        {
            //            td.FileSize = fileInfo.Length + "";
            //            fileCount++;
            //        }
            //    }
            //    else
            //    {
            //        folderCount++;
            //    }


            //    response.DataList.Add(td);
            //}

            //response.FileCount = fileCount + "";
            //response.FolderCount = folderCount + "";


            ////ActionFileInfo file = new ActionFileInfo();
            ////file.create();
            ////file.delete();
            ////file.view();



            //return Ok(new ApiResponseDto
            //{
            //    Success = true,
            //    Data = JsonSerializer.Serialize(response),
            //    Message = directoryPath
            //});
        }


    }

    //public class TestResponseDto
    //{
    //    public string FileCount { get; set; } = string.Empty;
    //    public string FolderCount { get; set; } = string.Empty;
    //    public List<TestData> DataList { get; set; } = new();
    //}



    public class ApiResponseDto
    {
        public bool Success { get; set; }
        public string? Data { get; set; }
        public string? FullPath { get; set; }
        public string Message { get; set; } = string.Empty;
        public string[]? Errors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class ActionFileInfo
    {

        public static class FileSystemUtils
        {


            //    /// <summary>
            //    /// Gets info for a single file.
            //    /// </summary>
            //    public static FileInfoResponseDto? GetFileInfo(string filePath)
            //    {
            //        var fi = new FileInfo(filePath);
            //        if (!fi.Exists) return null;

            //        return new FileInfoResponseDto
            //        {
            //            FullPath = fi.FullName,
            //            Name = fi.Name,
            //            SizeBytes = fi.Length,
            //            CreatedUtc = fi.CreationTimeUtc,
            //            ModifiedUtc = fi.LastWriteTimeUtc,
            //            IsReadOnly = fi.IsReadOnly,
            //            Extension = fi.Extension
            //        };

            //    }



            //    /// <summary>

            //    /// Gets info for a directory and optionally its files.

            //    /// searchPattern e.g. "*.json", "*.csv" - defaults to all files.

            //    /// </summary>

            //    public static DirectoryInfoResponseDto? GetDirectoryInfo(

            //        string directoryPath,

            //        string searchPattern = "*",

            //        bool recursive = false)

            //    {

            //        var di = new DirectoryInfo(directoryPath);

            //        if (!di.Exists) return null;



            //        var searchOption = recursive

            //            ? SearchOption.AllDirectories

            //            : SearchOption.TopDirectoryOnly;



            //        var files = di.GetFiles(searchPattern, searchOption)

            //            .Select(f => new FileInfoResponseDto

            //            {

            //                FullPath = f.FullName,

            //                Name = f.Name,

            //                SizeBytes = f.Length,

            //                CreatedUtc = f.CreationTimeUtc,

            //                ModifiedUtc = f.LastWriteTimeUtc,

            //                IsReadOnly = f.IsReadOnly,

            //                Extension = f.Extension

            //            })

            //            .ToList();



            //        var subDirs = di.GetDirectories()

            //            .Select(d => d.FullName)

            //            .ToList();



            //        return new DirectoryInfoResponseDto

            //        {

            //            FullPath = di.FullName,

            //            Name = di.Name,

            //            CreatedUtc = di.CreationTimeUtc,

            //            ModifiedUtc = di.LastWriteTimeUtc,

            //            Files = files,

            //            SubDirectories = subDirs

            //        };

            //    }



            //    /// <summary>

            //    /// Gets all files in a directory matching a pattern, sorted by modified date descending.

            //    /// </summary>

            //    public static List<FileInfoResponseDto> GetFilesSorted(

            //        string directoryPath,

            //        string searchPattern = "*",

            //        bool recursive = false)

            //    {

            //        var di = new DirectoryInfo(directoryPath);

            //        if (!di.Exists) return new List<FileInfoResponseDto>();



            //        var searchOption = recursive

            //            ? SearchOption.AllDirectories

            //            : SearchOption.TopDirectoryOnly;



            //        return di.GetFiles(searchPattern, searchOption)

            //            .OrderByDescending(f => f.LastWriteTimeUtc)

            //            .Select(f => new FileInfoResponseDto

            //            {

            //                FullPath = f.FullName,

            //                Name = f.Name,

            //                SizeBytes = f.Length,

            //                CreatedUtc = f.CreationTimeUtc,

            //                ModifiedUtc = f.LastWriteTimeUtc,

            //                IsReadOnly = f.IsReadOnly,

            //                Extension = f.Extension

            //            })

            //            .ToList();

            //    }



            //    /// <summary>

            //    /// Safely combines path segments using the OS path separator.

            //    /// Prefer this over string concatenation.

            //    /// </summary>

            //    public static string CombinePath(params string[] segments)

            //        => Path.Combine(segments);



            //    /// <summary>

            //    /// Returns the app's root content directory from IWebHostEnvironment,

            //    /// useful for locating wwwroot or other app-relative paths in ASP.NET Core.

            //    /// </summary>

            //    public static string GetContentRoot(IWebHostEnvironment env)

            //        => env.ContentRootPath;



            //    public static string GetWebRoot(IWebHostEnvironment env)

            //        => env.WebRootPath;

        }
    }
}
