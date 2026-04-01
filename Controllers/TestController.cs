using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Text.Json;
using TestProject.Actions;
using static System.Net.Mime.MediaTypeNames;
using static TestProject.Models.FileInfoDTOs;


namespace TestProject.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase {

        
        private readonly ILogger<TestController> _logger;


        public TestController(ILogger<TestController> logger) {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<ApiResponseDto> Get(
            [FromQuery] string? requestType = null,
            [FromQuery] string? location = null)
        {
            if (requestType != null)
            {
                if (requestType.Equals("updateLocation"))
                {
                    if(location == null) { location = ""; }
                    DirectoryInfoResponseDto requestDto = FileUtils.GetDirectoryInfo(location);

                    if (requestDto == null)
                    {
                        return NotFound(new ApiResponseDto
                        {
                            Success = false,
                            Data = JsonSerializer.Serialize("Bad Path"),
                            Message = "Bad Path"
                        });
                    }

                    DataResponseDto response = new DataResponseDto();
                    response.FileCount = requestDto.Files.Count + "";
                    response.FolderCount = requestDto.SubDirectories.Count + "";

                    for(int i = 0; i < requestDto.Files.Count; i++)
                    {
                        TestData td = new();
                        td.FileName = requestDto.Files[i].Name;
                        td.FileSize = requestDto.Files[i].SizeBytes + "";

                        response.DataList.Add(td);
                    }

                    for(int i =0; i<requestDto.SubDirectories.Count; i++)
                    {
                        TestData td = new();
                        td.FileName = requestDto.SubDirectories[i].Name;
                        response.DataList.Add(td);
                    }

                    response.FullPath = requestDto.FullPath;

                    return Ok(new ApiResponseDto
                    {
                        Success = true,
                        Data = JsonSerializer.Serialize(response),
                        Message = location
                    });


                }
            }

            return null;
            

        }

    }


}