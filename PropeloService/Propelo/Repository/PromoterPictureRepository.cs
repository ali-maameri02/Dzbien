using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Propelo.Data;
using Propelo.DTO;
using Propelo.Interfaces;
using Propelo.Models;

namespace Propelo.Repository
{
    public class PromoterPictureRepository : IPromoterPictureRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PromoterPictureRepository(ApplicationDBContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor= httpContextAccessor;
        }
        public async Task<PromoterPicture> CreatePromoterPictureAsync(PromoterPictureDTO promoterPictureDTO)
        {
            // Map DTO to entity
            var picture = _mapper.Map<PromoterPicture>(promoterPictureDTO);

            if (promoterPictureDTO.Picture == null || promoterPictureDTO.Picture.Length == 0)
            {
                throw new InvalidOperationException("No file uploaded.");
            }

            // Generate a unique file name
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(promoterPictureDTO.Picture.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "promoter-pictures");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filePath = Path.Combine(path, fileName);

            // Save the file to the path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await promoterPictureDTO.Picture.CopyToAsync(stream);
            }

            // Generate the public URL
            var request = _httpContextAccessor.HttpContext.Request;
            var fileUrl = $"{request.Scheme}://{request.Host}/promoter-pictures/{fileName}";

            // Update the picture entity with URL
            picture.PicturePath = fileUrl;

            // Save to database
            _context.PromoterPictures.Add(picture);

            // Ensure changes are saved
            await _context.SaveChangesAsync();

            return picture;
        }


        public Task<bool> DeletePromoterPictureAsync(int id)
        {
            _context.PromoterPictures.Remove(_context.PromoterPictures.Find(id));
            return SaveAllAsync();
        }

        public async Task<PromoterPicture> GetPromoterPictureByIdAsync(int id)
        {
            return await _context.PromoterPictures.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PromoterPicture>> GetPromoterPicturesAsync()
        {
            return await _context.PromoterPictures.OrderBy(a => a.Id).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            var save= await _context.SaveChangesAsync();
            return save >0 ? true: false;
        }

        public async Task<PromoterPicture> UpdatePromoterPicture(PromoterPictureDTO promoterPictureDTO, int promoterPictureId)
        {
            // Retrieve the existing picture from the database
            var picture = await _context.PromoterPictures
                .Where(p => p.Id == promoterPictureId)
                .FirstOrDefaultAsync();

            if (picture == null)
            {
                throw new Exception("Picture not found");
            }

            // Update properties except the picture file path
            _mapper.Map(promoterPictureDTO, picture);

            if (promoterPictureDTO.Picture != null && promoterPictureDTO.Picture.Length > 0)
            {
                // Generate a unique file name for the new picture
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(promoterPictureDTO.Picture.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "promoter-pictures");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var filePath = Path.Combine(path, fileName);

                // Save the new file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await promoterPictureDTO.Picture.CopyToAsync(stream);
                }

                // Generate the new public URL
                var request = _httpContextAccessor.HttpContext.Request;
                var fileUrl = $"{request.Scheme}://{request.Host}/promoter-pictures/{fileName}";

                // Delete the old file if it exists
                if (!string.IsNullOrEmpty(picture.PicturePath) && File.Exists(picture.PicturePath))
                {
                    File.Delete(picture.PicturePath);
                }

                // Update the picture entity with the new URL
                picture.PicturePath = fileUrl;
            }

            // Update the picture entity in the context
            _context.PromoterPictures.Update(picture);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return picture;
        }

    }
}
