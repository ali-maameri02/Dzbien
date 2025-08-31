using Propelo.DTO;
using Propelo.Models;
using System.Reflection.Metadata;

namespace Propelo.Interfaces
{
    //Mabey
    public interface IApartmentDocumentRepository
    {
        Task<List<ApartmentDocument>>CreateDocumentAsync(ApartmentDocumentDTO apartmentDocumentDTO);
        Task<IEnumerable<ApartmentDocument>> GetDocumentsAsync();
        Task<ApartmentDocument> GetDocumentByIdAsync(int id);
        Task<IEnumerable<ApartmentDocument>> GetApartmentDocumentsByApartmentIdAsync(int apartmentId);
        Task<ApartmentDocument> UpdateDocumentAsync(ApartmentDocumentDTO apartmentDocumentDTO);
        Task<ApartmentDocument> DeleteDocumentAsync(int id);
        Task<bool> SaveAllAsync();
    }
}
