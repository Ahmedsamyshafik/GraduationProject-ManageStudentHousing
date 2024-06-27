using Microsoft.AspNetCore.Mvc;

namespace BetterCallHomeWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Create a unique file name for the image
            var fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
            var extension = Path.GetExtension(imageFile.FileName);
            var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

            // Get the path to the "uploads" folder in the wwwroot directory
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Test");

            // Ensure the "uploads" folder exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Combine the path to get the full path to the image file
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the image file to the specified path
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            // Return a success response (e.g., the URL of the uploaded image)
            var imageUrl = $"/Test/{uniqueFileName}";
            return Ok(new { ImageUrl = imageUrl });
        }

        [HttpGet("image/{fileName}")]
        public IActionResult GetImage(string fileName)
        {
            // Get the path to the "uploads" folder in the wwwroot directory
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Test");
            var filePath = Path.Combine(uploadsFolder, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            // Get the content type of the image
            var contentType = GetContentType(filePath);

            // Read the image file into a byte array
            var imageBytes = System.IO.File.ReadAllBytes(filePath);

            // Return the image as a File result
            return File(imageBytes, contentType);
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
    {
        { ".txt", "text/plain" },
        { ".pdf", "application/pdf" },
        { ".doc", "application/vnd.ms-word" },
        { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
        { ".xls", "application/vnd.ms-excel" },
        { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
        { ".png", "image/png" },
        { ".jpg", "image/jpeg" },
        { ".jpeg", "image/jpeg" },
        { ".gif", "image/gif" },
        { ".csv", "text/csv" }
    };
        }
    }
}
