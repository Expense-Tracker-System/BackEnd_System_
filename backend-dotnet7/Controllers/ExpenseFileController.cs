using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace backend_dotnet7.Controllers
{
    public class ExpenseFileController : ControllerBase
    {
        private readonly IFileService _fileService;
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        //container name
        private const string AzureContainerName = "sajithsfiles";



        // Upload method
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload([FromForm] FileModel fileModel)
        {
            try
            {
                await _fileService.Upload(fileModel, AzureContainerName);
                return Ok(fileModel);
            }
            catch (Exception ex)
            {
                // Log the exception (use your preferred logging framework)
                Console.WriteLine($"Error uploading file: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }



        // downlord photo methord

        [HttpGet]
        [Route("download")]
        public async Task<IActionResult> Download(String name)
        {
            var imageFileStream = await _fileService.Get(name, AzureContainerName);
            string fileType = "jpeg";
            if (name.Contains("png"))
            {
                fileType = "png";
            }
            return File(imageFileStream, $"image/{fileType}", $"blobfile.{fileType}");
        }

        //list of files

        /* [HttpGet]
       [Route("list-files")]
     public async Task<IActionResult> ListFiles()
       {
           var fileNames = await _fileService.ListFiles(AzureContainerName);
           return Ok(fileNames);
       }
      */
        //delete photo
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteFile(String fileName)
        {
            await _fileService.Delete(fileName, AzureContainerName);
            return Ok("Successfully deleted");

        }
    }
}
