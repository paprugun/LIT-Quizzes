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
using BlazorApp.Shared.Models.ResponseModel.Session;
using Microsoft.JSInterop;
using Blazored.LocalStorage;
using Blazored.Toast;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using BlazorApp.Services.Services.PageServices;
using BlazorApp.Services.Interfaces.PageServices;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;
using BlazorApp.Server.Helpers.SwaggerFilters;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Stripe;
using BlazorApp.Services.Interfaces.CourseServices;
using BlazorApp.Services.Services.CourseServices;
using BlazorApp.Common.Exceptions;
using BlazorApp.Models.ResponseModels;
using BlazorApp.ResourceLibrary;
using BlazorApp.Server.Helpers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Logging;

namespace BlazorApp.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;

        }

        private readonly IWebHostEnvironment _env;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("Connection"));
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
                options.SignIn.RequireConfirmedEmail = true;
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
            services.AddTransient<IAccountService, Services.Services.AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IQuizAdminService, QuizAdminService>();
            services.AddScoped<IQuizUserService, QuizUserService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<ITopicsService, TopicsService>();
            services.AddScoped<IResultsService, ResultService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IUserCourseService, UserCourseService>();
            services.AddScoped<IEmailService, EmailService>();

            //pages services
            services.AddScoped<IAdminPageService, AdminPageService>();

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

            if (!_env.IsProduction())
            {
                services.AddSwaggerGen(options =>
                {
                    options.EnableAnnotations();

                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                    {
                        In = ParameterLocation.Header,
                        Description = "Access token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                    });

                    options.OrderActionsBy(x => x.ActionDescriptor.DisplayName);

                    // resolve the IApiVersionDescriptionProvider service
                    // note: that we have to build a temporary service provider here because one has not been created yet
                    var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                    // add a swagger document for each discovered API version
                    // note: you might choose to skip or document deprecated API versions differently
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                    }

                    // add a custom operation filter which sets default values

                    // integrate xml comments
                    options.IncludeXmlComments(XmlCommentsFilePath);
                    options.IgnoreObsoleteActions();

                    options.OperationFilter<DefaultValues>();
                    options.OperationFilter<SecurityRequirementsOperationFilter>("Bearer");

                    // for deep linking
                    options.CustomOperationIds(e => $"{e.HttpMethod}_{e.RelativePath.Replace('/', '-').ToLower()}");
                });

                // instead of options.DescribeAllEnumsAsStrings()
                services.AddSwaggerGenNewtonsoftSupport();
            }


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

            if (!_env.IsProduction())
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger(options =>
                {
                    options.PreSerializeFilters.Add((swagger, httpReq) =>
                    {
                        //swagger.Host = httpReq.Host.Value;

                        var ampersand = "&amp;";

                        foreach (var path in swagger.Paths)
                        {
                            if (path.Value.Operations.Any(x => x.Key == OperationType.Get && x.Value.Deprecated))
                                path.Value.Operations.First(x => x.Key == OperationType.Get).Value.Description = path.Value.Operations.First(x => x.Key == OperationType.Get).Value.Description.Replace(ampersand, "&");

                            if (path.Value.Operations.Any(x => x.Key == OperationType.Delete && x.Value?.Description != null))
                                path.Value.Operations.First(x => x.Key == OperationType.Delete).Value.Description = path.Value.Operations.First(x => x.Key == OperationType.Delete).Value.Description.Replace(ampersand, "&");
                        }

                        var paths = swagger.Paths.ToDictionary(p => p.Key, p => p.Value);
                        foreach (KeyValuePair<string, OpenApiPathItem> path in paths)
                        {
                            swagger.Paths.Remove(path.Key);
                            swagger.Paths.Add(path.Key.ToLowerInvariant(), path.Value);
                        }
                    });
                });

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(options =>
                {
                    options.IndexStream = () => System.IO.File.OpenRead("Views/Swagger/swagger-ui.html");
                    options.InjectStylesheet("/Swagger/swagger-ui.style.css");

                    var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();

                    foreach (var description in provider.ApiVersionDescriptions)
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());

                    options.EnableFilter();

                    // for deep linking
                    options.EnableDeepLinking();
                    options.DisplayOperationId();
                });

                app.UseReDoc(c =>
                {
                    c.RoutePrefix = "docs";
                    c.SpecUrl("/swagger/v1/swagger.json");
                    c.ExpandResponses("200");
                    c.RequiredPropsFirst();
                });
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
			app.UseRouting();

			#region Error handler

			// Different middleware for api and ui requests  
			app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
			{
				var localizer = app.ApplicationServices.GetService<IStringLocalizer<ErrorsResource>>();
				var logger = app.ApplicationServices.GetService<ILoggerFactory>().CreateLogger("GlobalErrorHandling");

				// Exception handler - show exception data in api response
				appBuilder.UseExceptionHandler(new ExceptionHandlerOptions
				{
					ExceptionHandler = async context =>
					{
						var errorModel = new ErrorResponseModel(localizer);
						var result = new ContentResult();

						var exception = context.Features.Get<IExceptionHandlerPathFeature>();

						if (exception.Error is CustomException)
						{
							var ex = (CustomException)exception.Error;

							result = errorModel.Error(ex);
						}
						else
						{
							var message = exception.Error.InnerException?.Message ?? exception.Error.Message;
							logger.LogError($"{exception.Path} - {message}");

							errorModel.AddError("general", message);
							result = errorModel.InternalServerError(_env.IsDevelopment() ? exception.Error.StackTrace : null);
						}

						context.Response.StatusCode = result.StatusCode.Value;
						context.Response.ContentType = result.ContentType;

						await context.Response.WriteAsync(result.Content);
					}
				});

				// Handles responses with status codes (correctly executed requests, without any exceptions)
				appBuilder.UseStatusCodePages(async context =>
				{
					var errorResponse = ErrorHelper.GetError(localizer, context.HttpContext.Response.StatusCode);

					context.HttpContext.Response.ContentType = "application/json";
					await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse, new JsonSerializerSettings { Formatting = Formatting.Indented }));
				});
			});

			app.UseWhen(context => !context.Request.Path.StartsWithSegments("/api"), appBuilder =>
			{
				appBuilder.UseExceptionHandler("/Error");
				appBuilder.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}");
			});

			#endregion

			app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }

        static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = $"ApplicationAuth API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = "The ApplicationAuth application with Swagger and API versioning."
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }

        private string Encode(string input, byte[] key)
        {
            HMACSHA256 myhmacsha = new HMACSHA256(key);
            byte[] byteArray = Encoding.UTF8.GetBytes(input);
            MemoryStream stream = new MemoryStream(byteArray);
            byte[] hashValue = myhmacsha.ComputeHash(stream);
            return Base64UrlEncoder.Encode(hashValue);
        }
    }
}
