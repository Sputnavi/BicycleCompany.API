using AutoMapper;
using BicycleCompany.BLL.Mapping;

namespace BicycleCompany.IntegrationTests.Utils
{
    public class MapperFixture
    {
        public MapperFixture()
        {
            var mappingConfiguration = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfiles());
            });

            Mapper = mappingConfiguration.CreateMapper();
        }

        public IMapper Mapper { get; set; }
    }
}
