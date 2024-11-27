
using OnlineBookShop.Service;
namespace ProductMiniApi.Repository.Implementation
{
    public class FileService : IFileService
    {
        private readonly string _absolutePath;

        public FileService(IConfiguration configuration)
        {
            _absolutePath = configuration["FileUpload:ImagePath"];
            if (!Directory.Exists(_absolutePath))
            {
                Directory.CreateDirectory(_absolutePath);
            }

        }

        public Tuple<int, string> SaveImage(IFormFile imageFile)
        {
            try
            {
                // Check the allowed extensions
                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
                if (!allowedExtensions.Contains(ext.ToLower()))
                {
                    string msg = $"Only {string.Join(", ", allowedExtensions)} extensions are allowed.";
                    return new Tuple<int, string>(0, msg);
                }

                // Generate a unique file name
                var uniqueString = Guid.NewGuid().ToString();
                var newFileName = uniqueString + ext;
                var fileWithPath = Path.Combine(_absolutePath, newFileName);

                // Save the file
                using (var stream = new FileStream(fileWithPath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                // Return the relative path for the Angular app
                var relativePath = $"/assets/img/book/{newFileName}";
                return new Tuple<int, string>(1, relativePath);
            }
            catch (Exception)
            {
                return new Tuple<int, string>(0, "An error occurred while saving the image.");
            }
        }

        public async Task DeleteImage(string imageFileName)
        {
            var fileWithPath = Path.Combine(_absolutePath, imageFileName);
            if (File.Exists(fileWithPath))
            {
                File.Delete(fileWithPath);
            }
        }

    }
}
