using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Text.Json;
using TestProject.Actions;
using static System.Net.Mime.MediaTypeNames;
using static TestProject.Models.FileInfoDTOs;
using System.IO;


namespace TestProject.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<ApiResponseDto> Get(
            [FromQuery] string? requestType = null,
            [FromQuery] string? location = null,
            [FromQuery] string? filter = null)
        {
            if (requestType != null)
            {
                if (requestType.Equals("updateLocation"))
                {
                    if (location == null) { location = ""; }
                    DirectoryInfoResponseDto requestDto = FileUtils.GetDirectoryInfo(location, filter);

                    if (requestDto == null)
                    {
                        return BadRequest("Bad Path");
                    }

                    DataResponseDto response = new DataResponseDto();
                    response = buildOutputList(requestDto);

                    return Ok(new 
                    {
                        Success = true,
                        Data = JsonSerializer.Serialize(response),
                        Message = location
                    });


                }
            }

            return NoContent();


        }

        public DataResponseDto buildOutputList(DirectoryInfoResponseDto requestDto)
        {
            DataResponseDto response = new DataResponseDto();
            response.FileCount = requestDto.Files.Count + "";
            response.FolderCount = requestDto.SubDirectories.Count + "";
            for (int i = 0; i < requestDto.Files.Count; i++)
            {
                TestData td = new();
                td.FileName = requestDto.Files[i].Name;
                td.FileSize = requestDto.Files[i].SizeBytes + "";
                response.DataList.Add(td);
            }
            for (int i = 0; i < requestDto.SubDirectories.Count; i++)
            {
                TestData td = new();
                td.FileName = requestDto.SubDirectories[i].Name;
                response.DataList.Add(td);
            }
            response.FullPath = requestDto.FullPath;
            return response;
        }


        [HttpPost]
        public async Task<IActionResult> Post(
            [FromForm] string currentPath,
            [FromForm] IFormFile myFile)
        {
            if (myFile == null || myFile.Length == 0)
                return BadRequest("No file");

            try
            {
                string path = FileUtils.checkImportFile(currentPath, myFile);

                using (Stream filestream = new FileStream(path,FileMode.Create))
                {
                    await myFile.CopyToAsync(filestream);
                }
            }
            catch(Exception e)
            {
                return BadRequest("Error saving file: " + e.Message);
            }
 

            return Ok(new { message = "Uploaded", fileName = myFile.FileName });
        }

    }

}