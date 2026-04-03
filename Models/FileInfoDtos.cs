using TestProject.Actions;

namespace TestProject.Models
{
    public class FileInfoDTOs
    {
        public class DirectoryInfoResponseDto
        {
            public string FullPath { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public DateTime CreatedUtc { get; set; }
            public DateTime ModifiedUtc { get; set; }
            public List<FileInfoResponseDto> Files { get; set; } = new();
            public List<SubDirectoryResponseDto> SubDirectories { get; set; } = new();
        }

        public class FileInfoResponseDto
        {
            public string FullPath { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public long SizeBytes { get; set; }
            public DateTime CreatedUtc { get; set; }
            public DateTime ModifiedUtc { get; set; }
            public bool IsReadOnly { get; set; }
            public string Extension { get; set; } = string.Empty;
        }

        public class SubDirectoryResponseDto
        {
            public string FullPath { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
        }



        public class DataResponseDto
        {
            public string FullPath { get; set; } = string.Empty;
            public string FileCount { get; set; } = string.Empty;
            public string FolderCount { get; set; } = string.Empty;
            public List<TestData> DataList { get; set; } = new();
        }

        public class TestData
        {
            public string FileName { get; set; } = string.Empty;
            public string? FileSize { get; set; } = string.Empty;
        }

        public class ApiResponseDto
        {
            public bool Success { get; set; }
            public string? Data { get; set; }
            public string? FullPath { get; set; }
            public string Message { get; set; } = string.Empty;
            public string[]? Errors { get; set; }
            public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        }
    }
}
