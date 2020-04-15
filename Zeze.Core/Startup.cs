using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Zeze.Common;
using Zeze.Common.Helper;
using Zeze.Core.AOP;
using Zeze.Domin.Data;

namespace Zeze.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration
            ,IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var basePath = AppContext.BaseDirectory;
            services.AddScoped<BaseContext>();
            services.AddSingleton<IRedisCacheManager, RedisCacheManager>();
            services.AddSingleton(new Appsettings(Env.ContentRootPath));
            #region Swagger
            //var ApiName = Appsettings.app(new string[] { "Startup", "ApiName" });
            services.AddSwaggerGen(c =>
            {
                // swagger�ĵ�����
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = $"Zeze �ӿ��ĵ�",
                    Description = $"Zeze HTTP API " + "v1",
                    Contact = new OpenApiContact { Name = "Zeze", Email = "305102247@qq.com", Url = new Uri("https://www.zeeeeee.com") },
                    License = new OpenApiLicense { Name = "Zeze", Url = new Uri("https://www.zeeeeee.com") }
                });
                // �ӿ�����
                c.OrderActionsBy(o => o.RelativePath);

                // ���� xml �ĵ�
                var xmlPath = Path.Combine(basePath, "Zeze.Core.xml");
                c.IncludeXmlComments(xmlPath, true);
                var xmlModelPath = Path.Combine(basePath, "Zeze.Core.Model.xml");
                c.IncludeXmlComments(xmlModelPath);

                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                // ����Ҫ���������ð�ȫУ�飬��֮ǰ�İ汾��һ��
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                // ���� oauth2 ��ȫ������������ oauth2 �������
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ֱ�����¿�������Bearer {token}��ע������֮����һ���ո�\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });


            #endregion

            #region Jwt
            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })// Ҳ����ֱ��д�ַ�����AddAuthentication("Bearer")
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,//�����������±�
                    ValidateIssuer = true,
                    ValidIssuer = audienceConfig["Issuer"],//������
                    ValidateAudience = true,
                    ValidAudience = audienceConfig["Audience"],//������
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,//����ǻ������ʱ�䣬Ҳ����˵����ʹ���������˹���ʱ�䣬����ҲҪ���ǽ�ȥ������ʱ��+���壬Ĭ�Ϻ�����7���ӣ������ֱ������Ϊ0
                    RequireExpirationTime = true,
                };

            });
            #endregion

            services.AddAutoMapper(typeof(Startup));//����AutoMapper��2.0������
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            var basePath = AppContext.BaseDirectory;

            var servicesDllFile = Path.Combine(basePath, "Zeze.Services.dll");
            var repositoryDllFile = Path.Combine(basePath, "Zeze.Repository.dll");


            // AOP ���أ������Ҫ��ָ���Ĺ��ܣ�ֻ��Ҫ�� appsettigns.json ��Ӧ��Ӧ true ���С�
            var cacheType = new List<Type>();
            if (Appsettings.app(new string[] { /*"AppSettings", */"RedisCachingAOP", "Enabled" }).ObjToBool())
            {
                builder.RegisterType<ZezeRedisCacheAOP>();
                cacheType.Add(typeof(ZezeRedisCacheAOP));
            }
            //if (Appsettings.app(new string[] { "AppSettings", "MemoryCachingAOP", "Enabled" }).ObjToBool())
            //{
            //    builder.RegisterType<BlogCacheAOP>();
            //    cacheType.Add(typeof(BlogCacheAOP));
            //}
            //if (Appsettings.app(new string[] { "AppSettings", "TranAOP", "Enabled" }).ObjToBool())
            //{
            //    builder.RegisterType<BlogTranAOP>();
            //    cacheType.Add(typeof(BlogTranAOP));
            //}
            if (Appsettings.app(new string[] { /*"AppSettings", */"LogAOP", "Enabled" }).ObjToBool())
            {
                builder.RegisterType<ZezeLogAOP>();
                cacheType.Add(typeof(ZezeLogAOP));
            }

            // ��ȡ Service.dll ���򼯷��񣬲�ע��
            var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            builder.RegisterAssemblyTypes(assemblysServices)
                      .AsImplementedInterfaces()
                      .InstancePerDependency()
                      .EnableInterfaceInterceptors()//����Autofac.Extras.DynamicProxy;
                      //.InterceptedBy(typeof(ZezeLogAOP))
                      .InterceptedBy(cacheType.ToArray());//.InterceptedBy(cacheType.ToArray());//����������������б�����ע�ᡣ

            // ��ȡ Repository.dll ���򼯷��񣬲�ע��
            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblysRepository)
                   .AsImplementedInterfaces()
                   .InstancePerDependency();

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
            });
            #endregion

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
