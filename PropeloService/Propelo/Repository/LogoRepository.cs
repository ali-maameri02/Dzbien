using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Propelo.Data;
using Propelo.DTO;
using Propelo.Interfaces;
using Propelo.Models;

namespace Propelo.Repository
{
    public class LogoRepository : ILogoRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogoRepository(ApplicationDBContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Logo> CreateLogoAsync(LogoDTO logoDTO)
        {
            var logo = _mapper.Map<Logo>(logoDTO);

            if (logoDTO.Logo == null || logoDTO.Logo.Length == 0)
            {
                throw new InvalidOperationException("No file uploaded.");
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(logoDTO.Logo.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Logo");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = Path.Combine(path, fileName);

            // Save the file to the path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await logoDTO.Logo.CopyToAsync(stream);
            }

            var request = _httpContextAccessor.HttpContext.Request;
            var fileUrl = $"{request.Scheme}://{request.Host}/logo/{fileName}";

            logo.LogoPath = fileUrl;

            _context.Logos.Add(logo);

            await _context.SaveChangesAsync();

            return logo;
        }

        public Task<bool> DeleteLogoAsync(int id)
        {
            _context.Logos.Remove(_context.Logos.Find(id));
            return SaveAllAsync();
        }

        public async Task<Logo> GetLogoByIdAsync(int id)
        {
            return await _context.Logos.Where(l => l.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Logo>> GetLogosAsync()
        {
            return await _context.Logos.OrderBy(l => l.Id).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            var save= await _context.SaveChangesAsync();
            return save > 0? true:false;
        }

        public async Task<Logo> UpdateLogoAsync(LogoDTO logoDTO, int logoId)
        {
            // Retrieve the existing picture from the database
            var logo = await _context.Logos.Where(p => p.Id == logoId).FirstOrDefaultAsync();

            if (logo == null)
            {
                throw new Exception("Picture not found");
            }

            // Update properties except the picture file path
            _mapper.Map(logoDTO, logo);

            if (logoDTO.Logo != null && logoDTO.Logo.Length > 0)
            {
                // Generate a unique file name for the new picture
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(logoDTO.Logo.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "logo");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var filePath = Path.Combine(path, fileName);

                // Save the new file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await logoDTO.Logo.CopyToAsync(stream);
                }

                // Generate the new public URL
                var request = _httpContextAccessor.HttpContext.Request;
                var fileUrl = $"{request.Scheme}://{request.Host}/promoter-pictures/{fileName}";

                // Delete the old file if it exists
                if (!string.IsNullOrEmpty(logo.LogoPath) && File.Exists(logo.LogoPath))
                {
                    File.Delete(logo.LogoPath);
                }

                // Update the picture entity with the new URL
                logo.LogoPath = fileUrl;
            }

            // Update the picture entity in the context
            _context.Logos.Update(logo);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return logo;
        }
    }
}
