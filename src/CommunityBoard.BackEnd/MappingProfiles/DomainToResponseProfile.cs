using AutoMapper;
using CommunityBoard.Core.DTOs.Responses;
using CommunityBoard.Core.Models.CoreModels;

namespace CommunityBoard.BackEnd.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Announcement, AnnouncementResponse>()
                .ForMember(dest => dest.Image,
                    opt =>
                        opt.Condition(a => a.Image != null));
            CreateMap<Report, ReportResponse>();
        }
    }
}