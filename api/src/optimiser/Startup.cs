using Optimiser.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Optimiser.Models;
using Amazon.DynamoDBv2;
using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using Microsoft.Net.Http.Headers;

namespace Optimiser
{
    public class Startup
    {
        readonly string CorsPolicyName = "_corsPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AWSXRayRecorder.InitializeInstance(configuration);
        }

        public static IConfiguration Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: CorsPolicyName,
                                  builder => { builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();});
            });
            services.AddOptions();
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.Configure<AwsSettingsModel>(Configuration.GetSection("AWS"));
            AWSSDKHandler.RegisterXRayForAllServices();
            services.AddTransient<IProcessingService, ProcessingService>();
            services.AddTransient<IDBDataService<IData>, DBDataService<IData>>();
            services.AddAWSService<IAmazonDynamoDB>();
            services.AddControllers();
    }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseCors(CorsPolicyName);
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
