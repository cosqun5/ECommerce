namespace Furn.Areas.Admin.Utilites.Extensions
{
    public static class FileExtensions
    {
        public static bool CheckContentType(this IFormFile file, string contentType)
        {
            return file.ContentType.ToLower().Trim().Contains(contentType.ToLower().Trim());
        }
        public static bool CheckSize(this IFormFile file, double size)
        {
            return file.Length / 1024 < size;
        }
        public static async Task<string> SaveAsync(this IFormFile file, string rootpath)
        {
            string filename = Guid.NewGuid().ToString() + file.FileName;
            string root = Path.Combine(rootpath, filename);
            using (FileStream fileStream = new FileStream(root, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return filename;
        }
    }
}
