namespace Propelo.DTO
{
    public class ApartmentPictureDTO
    {
        public int? Id { get; set; }
        public List<IFormFile>? Pictures { get; set; }
        public int? ApartmentId { get; set; }
    }
}
