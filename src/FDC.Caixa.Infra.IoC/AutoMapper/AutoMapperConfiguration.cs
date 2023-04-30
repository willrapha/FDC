using AutoMapper;

namespace FDC.Caixa.Infra.IoC.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        public static MapperConfiguration Initialize()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.AddProfile(new MappingProfile());
            });

            return mapperConfig;
        }
    }
}
