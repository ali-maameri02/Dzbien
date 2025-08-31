namespace Propelo.Models
{
    public class PromoterPicture
    {
        public int Id { get; set; }
        public string PictureName { get; set; }
        public string PicturePath { get; set; }
        public long PictureSize { get; set; }
        
        //one to many
        public int? PromoterId { get; set; }
        public Promoter? Promoter { get; set; }
    }
}
