using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Propelo.Data;
using Propelo.DTO;
using Propelo.Interfaces;
using Propelo.Models;

namespace Propelo.Repository
{
    public class ApartmentPictureRepository : IApartmentPictureRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApartmentPictureRepository(ApplicationDBContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<ApartmentPicture>> CreateApartmentPictureAsync(ApartmentPictureDTO apartmentPictureDTO)
        {
            var pictures = new List<ApartmentPicture>();
            string path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "apartment-pictures");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (var file in apartmentPictureDTO.Pictures)
            {
                if (file == null || file.Length == 0)
                {
                    throw new InvalidOperationException("One or more files are empty.");
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(path, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var request = _httpContextAccessor.HttpContext.Request;
                var fileUrl = $"{request.Scheme}://{request.Host}/apartment-pictures/{fileName}";

                var picture = new ApartmentPicture
                {
                    ApartmentId = apartmentPictureDTO.ApartmentId,
                    PictureName = fileName,
                    PicturePath = fileUrl,
                    PictureSize = file.Length
                };

                pictures.Add(picture);
                _context.ApartmentPictures.Add(picture);
            }

            await _context.SaveChangesAsync();

            return pictures;
        }

        public async Task<ApartmentPicture> DeletePictureAsync(int id)
        {
            var picture = await _context.ApartmentPictures.FindAsync(id);
            if (picture == null)
            {
                throw new KeyNotFoundException("Picture not found.");
            }

            // Remove the file from the server
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "apartment-pictures", picture.PictureName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Remove the picture entity from the database
            _context.ApartmentPictures.Remove(picture);
            await _context.SaveChangesAsync();

            return picture;
        }

        public async Task<IEnumerable<ApartmentPicture>> GetApartmentPicturesByApartmentIdAsync(int apartmentId)
        {
            return await _context.ApartmentPictures.Where(a => a.ApartmentId == apartmentId).ToListAsync();
        }

        public async Task<ApartmentPicture> GetPictureByIdAsync(int id)
        {
            return await _context.ApartmentPictures.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ApartmentPicture>> GetPicturesAsync()
        {
            return await _context.ApartmentPictures.OrderBy(a => a.Id).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            var save = await _context.SaveChangesAsync();
            return save > 0 ? true : false;
        }

        public async Task<ApartmentPicture> UpdatePictureAsync(ApartmentPictureDTO apartmentPictureDTO)
        {
            var picture = await _context.ApartmentPictures.FindAsync(apartmentPictureDTO.Id);
            if (picture == null)
            {
                throw new KeyNotFoundException("Picture not found.");
            }

            // If a new file is uploaded, delete the old file and upload the new one
            if (apartmentPictureDTO.Pictures != null && apartmentPictureDTO.Pictures.Count > 0)
            {
                var file = apartmentPictureDTO.Pictures.First(); // Assuming only one file for simplicity
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "apartment-pictures");

                // Delete the old picture file
                var oldFilePath = Path.Combine(path, picture.PictureName);
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }

                // Save the new file
                var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var newFilePath = Path.Combine(path, newFileName);

                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Generate the public URL for the new file
                var request = _httpContextAccessor.HttpContext.Request;
                var fileUrl = $"{request.Scheme}://{request.Host}/apartment-pictures/{newFileName}";

                // Update the picture entity with new data
                picture.PictureName = newFileName;
                picture.PicturePath = fileUrl;
                picture.PictureSize = file.Length;
            }

            // Update other fields if necessary (e.g., PropertyId)
            picture.ApartmentId = apartmentPictureDTO.ApartmentId;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return picture;
        }
    }
}
