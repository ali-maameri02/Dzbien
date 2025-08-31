namespace Propelo.DTO
{
    public class ApartmentDocumentDTO
    {
        public int? Id { get; set; }
        public List<IFormFile>? Documents { get; set; }
        public int? ApartmentId { get; set; }
    }
}
