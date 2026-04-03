using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Text.Json;
using TestProject.Controllers;
using static TestProject.Models.FileInfoDTOs;

namespace TestProject.Actions
{
    public class FileUtils
    {
        public static DirectoryInfoResponseDto? GetDirectoryInfo(
            string directoryPath,
            string searchPattern)
        {
            // Check if exists
            if(directoryPath.Length == 0)
            {
                directoryPath = null;
            }
            var di = new DirectoryInfo(directoryPath);

            if (!di.Exists) return null;


            //Get files
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

            //Get folders
            var subDirs = di.GetDirectories().Select(d => new SubDirectoryResponseDto
            {
                FullPath = d.FullName,
                Name = d.Name,
            }).ToList();

            //Filter by SearchPattern
            filterBySearchPattern(ref files, searchPattern);
            filterBySearchPattern(ref subDirs, searchPattern);

            return new DirectoryInfoResponseDto
            {
                FullPath = di.FullName,
                Name = di.Name,
                CreatedUtc = di.CreationTimeUtc,
                ModifiedUtc = di.LastWriteTimeUtc,
                Files = files,
                SubDirectories = subDirs
            };
        }

        public static void filterBySearchPattern(ref List<FileInfoResponseDto> files, string filter)
        {
            if(filter == null || filter.Length == 0) { return; }

            for(int i = files.Count-1; i>=0; i--)
            {
                if(!files[i].Name.ToLower().Contains(filter.ToLower()))
                {
                    files.Remove(files[i]);
                }
            }
        }
        //Overload based off Dto Type
        public static void filterBySearchPattern(ref List<SubDirectoryResponseDto> files, string filter)
        {
            if (filter == null || filter.Length == 0) { return; }

            for (int i = files.Count - 1; i >= 0; i--)
            {
                if (!files[i].Name.ToLower().Contains(filter.ToLower()))
                {
                    files.Remove(files[i]);
                }
            }
        }

        public static string checkImportFile(string currentPath, IFormFile myFile)
        {
            try
            {
                var path = Path.Combine(currentPath, myFile.FileName);

                FileInfo fileInfo = new FileInfo(path);
                int index = 1;
                string name = myFile.FileName.Substring(0, myFile.FileName.LastIndexOf("."));
                string extension = myFile.FileName.Substring(myFile.FileName.LastIndexOf("."));
                while (fileInfo.Exists)
                {
                    string newFileName = name + " - Copy";
                    if (index > 1)
                    {
                        newFileName += " (" + index + ")";
                    }
                    index++;
                    newFileName += extension;
                    path = Path.Combine(currentPath, newFileName);
                    fileInfo = new FileInfo(path);
                }

                return path;

            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }


   
}
