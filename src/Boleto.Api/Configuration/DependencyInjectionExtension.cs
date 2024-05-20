using Boleto.Domain.Intefaces.Services;
using Boleto.Service.Services;

namespace Boleto.Api.Configuration
{
    public static class DependencyInjectionExtension
     {
    //      public static void AddDependencyInjection(this IServiceCollection services, IConfiguration config)
    // {
    //     services.AddControllers();
    //     services.AddSwagger();
    //     services.AddFluent();
    //     services.AddOptions(config);

    //     services.AddServices();
    //     services.AddExternalServices(config);
    //     services.AddRepositories();
    //     services.AddOptions(config);
    // }
    //     public static void AddServices(this IServiceCollection services)
    // {
    //     services.AddScoped<IBoletoService, BoletoService>();
    // }

    // public static void AddExternalServices(this IServiceCollection services, IConfiguration config)
    // {
    //     var apis = config.GetSection("Apis");

    //     services.AddRefitClient<IUsuarioRepository>()
    //       .ConfigureHttpClient(c => c.BaseAddress = new Uri(apis.GetValue<string>("Plataforma")));

    // }

    // public static void AddRepositories(this IServiceCollection services)
    // {
    //     //services.AddScoped<IInvestimentosRepository, InvestimentosRepository>();
    //     //services.AddScoped<ICoreBankRepository, CoreBankRepository>();
    // }

    // public static void AddOptions(this IServiceCollection services, IConfiguration config)
    // {
    //     //services.Configure<CoreBankOptions>(config.GetSection("CoreBank"));
    //     //services.Configure<GenialAuthOptions>(config.GetSection("GenialAuth"));
    // }
    }
}