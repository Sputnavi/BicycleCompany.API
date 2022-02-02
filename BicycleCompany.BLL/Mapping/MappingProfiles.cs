using AutoMapper;
using BicycleCompany.DAL.Models;
using BicycleCompany.Models.Request;
using BicycleCompany.Models.Response;

namespace BicycleCompany.BLL.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Client, ClientForReadModel>();
            CreateMap<ClientForCreateOrUpdateModel, Client>().ReverseMap();
            CreateMap<ClientForReadModel, ClientForCreateOrUpdateModel>();

            CreateMap<Bicycle, BicycleForReadModel>();
            CreateMap<BicycleForCreateOrUpdateModel, Bicycle>().ReverseMap();
            CreateMap<BicycleForReadModel, BicycleForCreateOrUpdateModel>();

            CreateMap<Part, PartForReadModel>();
            CreateMap<PartForCreateOrUpdateModel, Part>().ReverseMap();
            CreateMap<PartForReadModel, PartForCreateOrUpdateModel>();

            CreateMap<Problem, ProblemForReadModel>()
                .ForMember(dst => dst.Parts,
                    opts => opts.MapFrom(x => x.PartDetails));

            CreateMap<ProblemForCreateModel, Problem>()
                .ForMember(dst => dst.PartDetails,
                    opts => opts.MapFrom(x => x.Parts))
                .ForMember(dst => dst.Parts,
                    opts => opts.Ignore());

            CreateMap<ProblemForUpdateModel, Problem>();
            CreateMap<ProblemForReadModel, ProblemForCreateModel>();
            CreateMap<ProblemForReadModel, ProblemForUpdateModel>();

            CreateMap<PartDetails, PartDetailsForCreateModel>().ReverseMap();
            CreateMap<PartDetails, PartDetailsForReadModel>();

            CreateMap<User, UserForReadModel>();
            CreateMap<UserForUpdateModel, User>();
            CreateMap<UserForReadModel, UserForUpdateModel>();
            CreateMap<UserForRegistrationModel, User>();
        }
    }
}
