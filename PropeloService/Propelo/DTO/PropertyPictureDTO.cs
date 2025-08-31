namespace Propelo.DTO
{
    public class PropertyPictureDTO
    {
        public int? Id { get; set; }
        public List<IFormFile>? Pictures { get; set; }
        public int? PropertyId { get; set; }
    }
}
