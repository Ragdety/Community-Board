using AutoMapper;
using CommunityBoard.Core.Contracts.Requests.Queries;
using CommunityBoard.Core.Models.Filters;

namespace CommunityBoard.BackEnd.MappingProfiles
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<PaginationQuery, PaginationFilter>();
        }
    }
}