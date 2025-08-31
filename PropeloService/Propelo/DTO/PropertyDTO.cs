using System.ComponentModel.DataAnnotations;

namespace Propelo.DTO
{
    public class PropertyDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateOnly? ConstractionDate { get; set; }
        public DateOnly? EndConstractionDate { get; set; }
        public int? ApartmentsNumber { get; set; }
        public bool? Terrain { get; set; }
        public string? Description { get; set; }
    }
}
