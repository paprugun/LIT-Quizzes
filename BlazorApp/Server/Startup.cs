using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using BlazorApp.Services.Interfaces;
using BlazorApp.Services.Services;
using System.Net.Http;
using BlazorApp.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BlazorApp.Domain.Entities.Identity;
using System;
using BlazorApp.DAL.Abstract;
using BlazorApp.Common.Utilities.Interfaces;
using Microsoft.AspNetCore.Http;
using BlazorApp.Common.Utilities;
using BlazorApp.DAL.Repository;
using BlazorApp.DAL.UnitOfWork;
using ApplicationAuth.Services.StartApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BlazorApp.Common.Constants;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.CookiePolicy;
using BlazorApp.Services.Interfaces.Utilities;
using BlazorApp.Shared.Models.ResponseModel.Session;
using BlazorApp.Services.Services.Utilities;
using Microsoft.JSInterop;
using Blazored.LocalStorage;
using Blazored.Toast;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using BlazorApp.Services.Services.PageServices;
using BlazorApp.Services.Interfaces.PageServices;

namespace BlazorApp.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("Connection"));
                options.EnableSensitiveDataLogging(false);
            });

            services.AddCors();

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+#=";
            }).AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(o =>
            {
                o.Name = "Default";
                o.TokenLifespan = TimeSpan.FromHours(12);
            });

            services.AddScoped<IDataContext>(provider => provider.GetService<DataContext>());
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //utilities
            services.AddSingleton<IHashUtility, HashUtility>();
            services.AddSingleton(typeof(IStateContainer<>), typeof(StateContainer<>));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<ILocalStorageService<UserRoleResponse>, UserLocalStorageService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IQuizAdminService, QuizAdminService>();
            services.AddScoped<IQuizUserService, QuizUserService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<ITopicsService, TopicsService>();
            services.AddScoped<IResultsService, ResultService>();

            //pages services
            services.AddScoped<IHomePageService, HomePageService>();
            services.AddScoped<ICatalogPageService, CatalogPageService>();
            services.AddScoped<IAdminPageService, AdminPageService>();

            services.AddBlazoredLocalStorage();
            services.AddBlazoredToast();

            services.AddControllersWithViews();
            services.AddRazorPages();

            //AddSingleton - означает что класс сервиса будет общим для всего приложения и не будет выгружаться из памяти

            //AddScoped - означает что класс сервиса не будет общим для всего приложения, каждая ссылка является новой, очищается после использования

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfileConfiguration());
            });

            services.AddSingleton(config.CreateMapper());


            services
                .AddDetection()
                .AddCoreServices()
                .AddRequiredPlatformServices();

            


            services.AddMiniProfiler(opt =>
            {
                opt.RouteBasePath = "/profiler";
            })
            .AddEntityFramework();

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddVersionedApiExplorer(
                 options =>
                 {
                     options.GroupNameFormat = "'v'VVV";

                     // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                     // can also be used to control the format of the API version in route templates
                     options.SubstituteApiVersionInUrl = true;
                 });

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddMvc(options =>
            {
                // Allow use optional parameters in actions
                options.AllowEmptyInputInBodyModelBinding = true;
                options.EnableEndpointRouting = false;
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            })
            .ConfigureApiBehaviorOptions(o => o.SuppressModelStateInvalidFilter = true)
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            
            var sp = services.BuildServiceProvider();
            var serviceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(/*JwtBearerDefaults.AuthenticationScheme, */options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthOptions.ISSUER,
                    ValidateAudience = true,
                    ValidateActor = false,
                    ValidAudience = AuthOptions.AUDIENCE,
                    ValidateLifetime = true,
                    //SignatureValidator = (string token, TokenValidationParameters validationParameters) => {

                    //    var jwt = new JwtSecurityToken(token);

                    //    var signKey = AuthOptions.GetSigningCredentials().Key as SymmetricSecurityKey;

                    //    var encodedData = jwt.EncodedHeader + "." + jwt.EncodedPayload;
                    //    var compiledSignature = Encode(encodedData, signKey.Key);

                    //    //Validate the incoming jwt signature against the header and payload of the token
                    //    if (compiledSignature != jwt.RawSignature)
                    //    {
                    //        throw new Exception("Token signature validation failed.");
                    //    }

                    //    /// TO DO: initialize user claims

                    //    return jwt;
                    //},
                    LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters) =>
                    {
                        var jwt = securityToken as JwtSecurityToken;

                        if (!notBefore.HasValue || !expires.HasValue || DateTime.Compare(expires.Value, DateTime.UtcNow) <= 0)
                        {
                            return false;
                        }

                        if (jwt == null)
                            return false;

                        var isRefresStr = jwt.Claims.FirstOrDefault(t => t.Type == "isRefresh")?.Value;

                        if (isRefresStr == null)
                            return false;

                        var isRefresh = Convert.ToBoolean(isRefresStr);

                        if (!isRefresh)
                        {
                            try
                            {
                                using (var scope = serviceScopeFactory.CreateScope())
                                {
                                    var hash = scope.ServiceProvider.GetService<IHashUtility>().GetHash(jwt.RawData);
                                    return scope.ServiceProvider.GetService<IRepository<UserToken>>().Find(t => t.AccessTokenHash == hash && t.IsActive) != null;
                                }
                            }
                            catch (Exception ex)
                            {
                                var logger = sp.GetService<ILogger<Startup>>();
                                logger.LogError(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") + ": Exception occured in token validator. Exception message: " + ex.Message + ". InnerException: " + ex.InnerException?.Message);
                                return false;
                            }
                        }

                        return false;
                    },
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true
                };
            });

            services.AddRouting();
            services.AddMemoryCache();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            #region Cookie auth

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always
            });

            app.Use(async (context, next) =>
            {
                var token = context.Request.Cookies[".AspNetCore.Application.Id"];
                if (!string.IsNullOrEmpty(token))
                    context.Request.Headers.Add("Authorization", "Bearer " + token);

                await next();
            });

            #endregion

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
