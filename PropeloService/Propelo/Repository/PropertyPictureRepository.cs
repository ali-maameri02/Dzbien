using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Propelo.Data;
using Propelo.DTO;
using Propelo.Interfaces;
using Propelo.Models;

namespace Propelo.Repository
{
    public class PropertyPictureRepository : IPropertyPictureRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PropertyPictureRepository(ApplicationDBContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<PropertyPicture>> CreatePropertyPictureAsync(PropertyPictureDTO propertyPictureDTO)
        {
            var pictures = new List<PropertyPicture>();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "property-pictures");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (var file in propertyPictureDTO.Pictures)
            {
                if (file == null || file.Length == 0)
                {
                    throw new InvalidOperationException("One or more files are empty.");
                }

                // Generate a unique file name
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(path, fileName);

                // Save the file to the path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Generate the public URL
                var request = _httpContextAccessor.HttpContext.Request;
                var fileUrl = $"{request.Scheme}://{request.Host}/property-pictures/{fileName}";

                // Create and map the picture entity
                var picture = new PropertyPicture
                {
                    PropertyId = propertyPictureDTO.PropertyId,
                    PictureName = fileName,
                    PicturePath = fileUrl,
                    PictureSize = file.Length
                };

                pictures.Add(picture);
                _context.PropertyPictures.Add(picture);
            }

            // Save all pictures to the database
            await _context.SaveChangesAsync();

            return pictures;
        }

        public async Task<PropertyPicture> DeletePropertyPictureAsync(int id)
        {
            var picture = await _context.PropertyPictures.FindAsync(id);
            if (picture == null)
            {
                throw new KeyNotFoundException("Picture not found.");
            }

            // Remove the file from the server
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "property-pictures", picture.PictureName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Remove the picture entity from the database
            _context.PropertyPictures.Remove(picture);
            await _context.SaveChangesAsync();

            return picture;
        }

        public async Task<PropertyPicture> GetPropertyPictureByIdAsync(int id)
        {
            return await _context.PropertyPictures.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PropertyPicture>> GetPropertyPicturesAsync()
        {
            return await _context.PropertyPictures.OrderBy(a => a.Id).ToListAsync();
        }

        public async Task<IEnumerable<PropertyPicture>> GetPropertyPicturesByPropertyIdAsync(int propertyId)
        {
            return await _context.PropertyPictures.Where(a => a.PropertyId == propertyId).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            var save = await _context.SaveChangesAsync();
            return save > 0 ? true : false;
        }

        public async Task<PropertyPicture> UpdatePropertyPictureAsync(PropertyPictureDTO propertyPictureDTO)
        {
            var picture = await _context.PropertyPictures.FindAsync(propertyPictureDTO.Id);
            if (picture == null)
            {
                throw new KeyNotFoundException("Picture not found.");
            }

            // If a new file is uploaded, delete the old file and upload the new one
            if (propertyPictureDTO.Pictures != null && propertyPictureDTO.Pictures.Count > 0)
            {
                var file = propertyPictureDTO.Pictures.First(); // Assuming only one file for simplicity
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "property-pictures");

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
                var fileUrl = $"{request.Scheme}://{request.Host}/property-pictures/{newFileName}";

                // Update the picture entity with new data
                picture.PictureName = newFileName;
                picture.PicturePath = fileUrl;
                picture.PictureSize = file.Length;
            }

            // Update other fields if necessary (e.g., PropertyId)
            picture.PropertyId = propertyPictureDTO.PropertyId;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return picture;
        }
    }
}
