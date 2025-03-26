using AutoMapper;

namespace FinancialControl.Api.Mappings
{
    public static class AutoMapperConfiguration
    {
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddScoped(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new FinancialControlProfile());
            }).CreateMapper());
        }
    }
}