using backend_dotnet7.Core.Interfaces;

namespace backend_dotnet7.Core.Services
{
    public class UserImageService : IUserImageService
    {
        private readonly IWebHostEnvironment _environment;

        public UserImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void deleteUserImage(string imageNameWithExtension)
        {
            if (string.IsNullOrEmpty(imageNameWithExtension))
            {
                throw new ArgumentNullException(nameof(imageNameWithExtension));
            }

            var contentPath = _environment.ContentRootPath;
            var path = Path.Combine(contentPath, "Uploads", imageNameWithExtension);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Invalid file {path}");
            }

            File.Delete(path);
        }

        public async Task<string> SaveUserImageAsync(IFormFile imageFile, string[] allowedFileExtensions)
        {
            // ----
            if(imageFile == null)
            {
                throw new ArgumentNullException(nameof(imageFile));
            }

            // -----
            var contentPath = _environment.ContentRootPath;
            var path = Path.Combine(contentPath, "Uploads");
            // ------

            // ------
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // -----
            var ext = Path.GetExtension(imageFile.FileName);

            // -----
            if (!allowedFileExtensions.Contains(ext))
            {
                throw new ArgumentException($"only {string.Join(",",allowedFileExtensions)} are allowed.");
            }

            // -----
            var fileName = $"{Guid.NewGuid().ToString()}{ext}";
            var fileNameWithPath = Path.Combine(path, fileName);
            using var stream = new FileStream(fileNameWithPath, FileMode.Create);
            await imageFile.CopyToAsync(stream);
            return fileName;
        }
    }
}
