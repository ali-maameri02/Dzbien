using Microsoft.AspNetCore.Http;

namespace Propelo.Models
{
    public class ApartmentDocument
    {
        public int Id { get; set; }
        public string DocumentName { get; set; }  
        public string DocumentPath { get; set; }
        public long DocumentSize { get; set; }

        //many to one
        public int? ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }
    }
}
