using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NZWalksAPI.Data;
using NZWalksAPI.Model.Domain;
using System.IO;
using System.Threading.Tasks;

namespace NZWalksAPI.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NzWalksDbContext dbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, NzWalksDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}");
            // Upload Image to Local Path
            // คำสั่ง using ถูกใช้เพื่อให้แน่ใจว่า object ที่ถูกสร้างขึ้นจะถูกปิดหรือ dispose ทันทีเมื่อการทำงานของมันเสร็จสิ้น
            // FileStream จะใช้ในการเปิดไฟล์, เขียน, อ่าน หรือทำการสร้างไฟล์ใหม่
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);
            // https://localhost:1234/Images/image.jpg
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;

            // Add Image to the Images table
            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();

            return image;
        }
    }
}
