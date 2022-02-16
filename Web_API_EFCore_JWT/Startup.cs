
using System.Text;
using APICatalogo.Context;
using APICatalogo_Fundamentos.Extensions;
using APICatalogo_Fundamentos.Filters;

using APICatalogo_Fundamentos.Services;
using APICatalogo_Repositorio.DTOs.Mappings;
using APICatalogo_Repositorio.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LanchesMac;
public class Startup {

    public Startup(IConfiguration configuration) {

        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }


    // Este método é chamado pelo tempo de execução. Use este método para adicionar serviços ao contêiner.
    public void ConfigureServices(IServiceCollection services) {

        

        
        services.AddTransient<IMeuServico, MeuServico>();


        // Registrando serviço da pasta Filters
        services.AddScoped<ApiLogginFilter>();


        services.AddControllersWithViews();

        // Implementado padrão Unit Of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();



        // Registrando o serviço de mapeamento das DTOs
        var mappingConfig = new MapperConfiguration(mc => {
            mc.AddProfile(new MappingProfile());
        });
        IMapper mapper = mappingConfig.CreateMapper();
        services.AddSingleton(mapper);// AddSingleton é registrado uma instancia só



        // Comando criado a partir da classe context(CURSO)
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


        // Registrar serviço do Identtity para Segurança
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();





        //JWT
        //adiciona o manipulador de autenticação e define o 
        //esquema de autenticação usado : Bearer
        //valida o emissor, a audiência e a chave
        //usando a chave secreta valida a assinatura
        services.AddAuthentication(
            JwtBearerDefaults.AuthenticationScheme).
            AddJwtBearer(options =>
             options.TokenValidationParameters = new TokenValidationParameters {
                 ValidateIssuer = true,
                 ValidateAudience = true,
                 ValidateLifetime = true,
                 ValidAudience = Configuration["TokenConfiguration:Audience"],
                 ValidIssuer = Configuration["TokenConfiguration:Issuer"],
                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = new SymmetricSecurityKey(
                     Encoding.UTF8.GetBytes(Configuration["Jwt:key"]))
             });

    }



    // Este método é chamado pelo tempo de execução. Use este método para configurar o pipeline de solicitação HTTP(.
    // Aqui é onde se configura os Middleware
    // O middleware é um software montado em um pipeline de aplicativo para manipular solicitações e respostas.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory) {

        if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        } else {
            
            app.UseExceptionHandler("/Home/Error");
            
            app.UseHsts();
        }


        // loggerFactory.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration {

        //     LogLevel = LogLevel.Information
        // }));

        // Adiciona o middleware de tratamento de erros criado na pasta Extensions
        app.ConfigureExceptionHandler();




        //adiciona o middleware para redirecionar para https
        app.UseHttpsRedirection();

        app.UseStaticFiles();

        // Adiciona o middleware de roteamento
        app.UseRouting();



        // Adiciona o middleware de autenticação (identity)
        app.UseAuthentication();
        // Adiciona o middleware que habilita a autorização (identity)
        app.UseAuthorization();



        //Adiciona o middleware que executa o endpoint 
        //do request atual
        app.UseEndpoints(endpoints => {

            // adiciona os endpoints para as Actions
            // dos controladores
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}