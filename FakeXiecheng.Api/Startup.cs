using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiecheng.Api.Common;
using FakeXiecheng.Api.Common.Extensions;
using FakeXiecheng.Api.Repository;
using FakeXiecheng.Api.Repository.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

namespace FakeXiecheng.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Configuration.AddSectionValue<JwtConfigs>();

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            // services.AddTransient<ITouristRouteRepository,MockTouristRouteRepository>();
            services.AddTransient<ITouristRouteRepository, TouristRouteRepository>();
            services.AddDbContext<AppDbContext>(options =>
            {
                // options.UseSqlServer(Configuration.GetConnectionString("SqlServer"));
                options.UseMySql(Configuration.GetConnectionString("MySql"));
            });

            services.AddAutoMapper(typeof(MapperConfig));

            services.AddCors(c =>
            {
                c.AddPolicy("AllRequests", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;

                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidAudience = JwtConfigs.Issuer,

                        ValidateIssuer = true,
                        ValidIssuer = JwtConfigs.Issuer,

                        ValidateLifetime = true,
                        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => DateTime.UtcNow < expires,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfigs.Key)),
                    };
                });

            services.AddControllers(options =>
            {
                // 当无法处理请求的 media-type 时，不返回默认的 media-type
                options.ReturnHttpNotAcceptable = true;
                // options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                options.Filters.Add<ExceptionFilter>();
            })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .AddXmlDataContractSerializerFormatters()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var problemDetail = new ValidationProblemDetails(context.ModelState)
                        {
                            Type = "无所谓",
                            Title = "数据验证失败",
                            Status = StatusCodes.Status422UnprocessableEntity,
                            Detail = context.HttpContext.Request.Path
                        };
                        problemDetail.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
                        return new UnprocessableEntityObjectResult(problemDetail)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    };
                });

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fake Xiecheng API", Version = "1.0" });
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{MethodBase.GetCurrentMethod()?.DeclaringType?.Namespace}.xml"));
            });

            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // 你在哪儿？
            app.UseRouting();
            // 你是谁？
            app.UseAuthentication();
            // 你可以干什么?
            app.UseAuthorization();

            app.UseSwagger();

            app.UseCors("AllRequests");

            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapGet("/", async context =>
                // {
                //     await context.Response.WriteAsync("Hello World!");
                // });
                endpoints.MapControllers();
            });

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "";
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "v1");
            });
        }
    }
}
