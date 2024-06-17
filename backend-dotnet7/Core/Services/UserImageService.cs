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

        public void deleteUserImage(string userName, string imageNameWithExtension)
        {
            if (string.IsNullOrEmpty(imageNameWithExtension))
            {
                throw new ArgumentNullException(nameof(imageNameWithExtension));
            }

            var contentPath = _environment.ContentRootPath;
            var path = Path.Combine(contentPath, "Uploads",userName, imageNameWithExtension);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Invalid file {path}");
            }

            File.Delete(path);
        }

        public async Task<string> SaveUserImageAsync(string userName, IFormFile imageFile, string[] allowedFileExtensions)
        {
            // -----
            var contentPath = _environment.ContentRootPath;
            var path = Path.Combine(contentPath, "Uploads", userName);
            // ------

            // ------
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // -----
            var filesInDirectory = Directory.GetFiles(path);
            foreach ( var file in filesInDirectory)
            {
                var fileExtension = Path.GetExtension(file).ToLowerInvariant();
                if (allowedFileExtensions.Contains(fileExtension))
                {
                    throw new ArgumentException("already have");
                }
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
