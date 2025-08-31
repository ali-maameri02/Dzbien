using AutoMapper;
using Propelo.DTO;
using Propelo.Models;

namespace Propelo.extensions
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Promoter,PromoterDTO>().ReverseMap();
            CreateMap<Promoter,PromoterDTO>().ReverseMap();
            CreateMap<Property,PropertyDTO>().ReverseMap();
            CreateMap<Apartment, ApartmentDTO>().ReverseMap();
            CreateMap<Area, AreaDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Setting, SettingDTO>().ReverseMap();
                

            CreateMap<PropertyPictureDTO,PropertyPicture>().ReverseMap();
                //.ForMember(dest => dest.PictureName, opt=>opt.MapFrom(src=>src.Picture.FileName))
                //.ForMember(dest => dest.PicturePath, opt=>opt.MapFrom(src=>src.Picture.Length))
                //.ForMember(dest => dest.PictureSize, opt=>opt.MapFrom(src=>src.Picture.Length));
       
            CreateMap<ApartmentPictureDTO, ApartmentPicture>().ReverseMap();
                //.ForMember(dest => dest.PictureName, opt => opt.MapFrom(src => src.Picture.FileName))
                //.ForMember(dest => dest.PicturePath, opt => opt.MapFrom(src => src.Picture.Length))
                //.ForMember(dest => dest.PictureSize, opt => opt.MapFrom(src => src.Picture.Length));

            CreateMap<ApartmentDocumentDTO, ApartmentDocument>().ReverseMap();
                //.ForMember(dest => dest.DocumentName, opt => opt.MapFrom(src => src.Document.FileName))
                //.ForMember(dest => dest.DocumentPath, opt => opt.MapFrom(src => src.Document.Length))
                //.ForMember(dest => dest.DocumentSize, opt => opt.MapFrom(src => src.Document.Length));

            CreateMap<LogoDTO, Logo>()
                .ForMember(dest => dest.LogoName, opt => opt.MapFrom(src => src.Logo.FileName))
                .ForMember(dest => dest.LogoPath, opt => opt.MapFrom(src => src.Logo.Length))
                .ForMember(dest => dest.LogoSize, opt => opt.MapFrom(src => src.Logo.Length));

            CreateMap<PromoterPictureDTO, PromoterPicture>()
                .ForMember(dest => dest.PictureName, opt => opt.MapFrom(src => src.Picture.FileName))
                .ForMember(dest => dest.PicturePath, opt => opt.MapFrom(src => src.Picture.Length))
                .ForMember(dest => dest.PictureSize, opt => opt.MapFrom(src => src.Picture.Length));
        }
    }
}
