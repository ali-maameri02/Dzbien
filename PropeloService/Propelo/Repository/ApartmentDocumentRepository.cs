using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Propelo.Data;
using Propelo.DTO;
using Propelo.Interfaces;
using Propelo.Models;
using System.IO;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace Propelo.Repository
{
    public class ApartmentDocumentRepository : IApartmentDocumentRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApartmentDocumentRepository(ApplicationDBContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<ApartmentDocument>> CreateDocumentAsync(ApartmentDocumentDTO apartmentDocumentDTO)
        {
            var documents = new List<ApartmentDocument>();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "apartment-documents");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (var file in apartmentDocumentDTO.Documents)
            {
                if (file == null || file.Length == 0)
                {
                    throw new InvalidOperationException("One or more files are empty.");
                }

                //string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string fileName = Path.GetFileName(file.FileName);
                string filePath = Path.Combine(path, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var request = _httpContextAccessor.HttpContext.Request;
                string fileUrl = $"{request.Scheme}://{request.Host}/apartment-documents/{fileName}";

                var document = new ApartmentDocument
                {
                    ApartmentId = apartmentDocumentDTO.ApartmentId,
                    DocumentName = fileName,
                    DocumentPath = fileUrl,
                    DocumentSize = file.Length
                    
                };

                documents.Add(document);
                _context.ApartmentDocuments.Add(document);
            }
            await _context.SaveChangesAsync();

            return documents;
        }

        public async Task<ApartmentDocument> DeleteDocumentAsync(int id)
        {
            var document = await _context.ApartmentDocuments.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (document == null)
            {
                throw new KeyNotFoundException("Picture not found.");
            }
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "apartment-documents", document.DocumentName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            _context.ApartmentDocuments.Remove(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task<IEnumerable<ApartmentDocument>> GetApartmentDocumentsByApartmentIdAsync(int apartmentId)
        {
            return await _context.ApartmentDocuments.Where(a => a.ApartmentId == apartmentId).ToListAsync();
        }

        public async Task<ApartmentDocument> GetDocumentByIdAsync(int id)
        {
            return await _context.ApartmentDocuments.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ApartmentDocument>> GetDocumentsAsync()
        {
            return await _context.ApartmentDocuments.OrderBy(a => a.Id).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            var save = await _context.SaveChangesAsync();
            return save > 0? true:false ;
        }

        public async Task<ApartmentDocument> UpdateDocumentAsync(ApartmentDocumentDTO apartmentDocumentDTO)
        {
            var document = _context.ApartmentDocuments.Where(a => a.Id == apartmentDocumentDTO.Id).FirstOrDefault();
            if (document == null)
            {
                throw new KeyNotFoundException("Document not found.");
            }

            if(apartmentDocumentDTO.Documents != null && apartmentDocumentDTO.Documents.Any())
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "apartment-documents", document.DocumentName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                var file = apartmentDocumentDTO.Documents.FirstOrDefault();
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "apartment-documents", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var request = _httpContextAccessor.HttpContext.Request;
                string fileUrl = $"{request.Scheme}://{request.Host}/apartment-documents/{fileName}";

                document.DocumentName = fileName;
                document.DocumentPath = fileUrl;
                document.DocumentSize = file.Length;
            }
            document.ApartmentId = apartmentDocumentDTO.ApartmentId;
            await _context.SaveChangesAsync();

            return document;
        }
    }
}
