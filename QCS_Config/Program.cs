using System.Diagnostics;
using System.Reflection;
using System.Text;
using QCS_Config.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseWindowsService();
// Add services to the container.
// var baseAddress = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
// SetConfig.init(baseAddress + @"\appsettings.json");

var myJObject = JObject.Parse(@"{
""Logging"": {
    ""LogLevel"": {
        ""Default"": ""Information"",
        ""Microsoft"": ""Warning"",
        ""Microsoft.Hosting.Lifetime"": ""Information""
    }
},
""Config"": {
    ""RabbitmqConfig"": {
        ""HostName"": ""192.168.130.40"",
        ""UserName"": ""guest"",
        ""Password"": ""guest""
    },
    ""TransferValidatorApiAddress"": ""http://192.168.130.92:12001/services/transfer/parse"",
    ""sqlConnectionString"": ""workstation id=DarmanYar.mssql.somee.com;packet size=4096;user id=DarmanYar_SQLLogin_1;pwd=endkcy6ilu;data source=DarmanYar.mssql.somee.com;persist security info=False;initial catalog=DarmanYar"",


    ""CheckConnection"": {
        ""GetFiles"": 10,
        ""GetFilteredFiles"": 10,
        ""GetFolders"": 100000,
        ""SortItem"": 1,
        ""SortOrder"": 1,
        ""RequestType"": 0,
        ""FileServerConfigTimeout"": 2000,
        ""PingRetries"": 1,
        ""PingTimeout"": 1000
    },
    
    
    ""Log"": {
        ""AgentName"": ""Clinic"",
        ""ElasticLog"": {
            ""Active"": false,
            ""Uri"": ""http://192.168.130.31:9200"",
            ""IndexFormat"": ""clinic"",
            ""BufferBaseFilename"": ""D:\\Logs\\HostGuestLog\\ElasticBuffers\\buffer"",
            ""BufferFileCountLimit"": 10, 
            ""BufferLogShippingInterval"": 60 
        },
        ""FileLog"": {
            ""Active"": false,
            ""LogTemplate"": ""{Timestamp:yyyy-MM-dd HH:mm:ss} {AgentName} [{Level:u4}] {Message}{NewLine}{Exception}"",
            ""Address"": ""\\ClinicLog""
        },
        ""ConsoleLog"": {
            ""Active"": true
        }
    }
},
""AllowedHosts"": ""*""
}
");
var obj = myJObject["Config"] as JObject;
Config.All = obj.ToObject<ConfigModels>();

builder.Services.Configure<IISServerOptions>(options =>
{
    options.AutomaticAuthentication = false;
});
builder.Services.AddControllersWithViews();
builder.Services.AddDirectoryBrowser();
builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(x =>
              {
                  x.RequireHttpsMetadata = false; 
                  x.SaveToken = true;
                  x.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = false,
                      ValidateAudience = false,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      //ValidIssuer = "Kavosh 3 HostGuest Issuer",
                      //ValidAudience = "Kavosh 3 HostGuest Audience",
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("Clinic auth111111111111111111111")),
                      ClockSkew = new TimeSpan(00, 01, 00),
                  };    
              });

            builder.Services.AddDbContext<QCS_Config.Models.DBContext>(options => options.UseSqlServer(Config.All.sqlConnectionString));
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                              Type = ReferenceType.SecurityScheme,
                              Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

            });

builder.Services.AddSwaggerGen();
            // In production, the Angular files will be served from this directory
builder.Services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            //Log initial
            Log.Initialize(Config.All.Log.AgentName, Config.All.Log.FileLog.LogTemplate, Config.All.Log.ElasticLog.Active, Config.All.Log.ElasticLog.Uri, Config.All.Log.ElasticLog.IndexFormat,
                            Config.All.Log.FileLog.Active, Config.All.Log.FileLog.Address, Config.All.Log.ConsoleLog.Active);
            

            Log.Logger.Information("Clinic Running Version:{Version}", FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
            
var app = builder.Build();


    
            app.UseSpaStaticFiles();
            app.Use(async (context, next) =>
            {
                try
                {
                    // if (context.Request.Path == "/" && Config.All.UI.Active == false && Config.All.UI.ApiAddress == ".") context.Response.Redirect("/swagger/index.html");

                    await next().ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    Log.Logger.Error("{ActionSourceError}{Message}{Exception}", context.Request.Path, e.Message, e);
                    throw;
                }
            });
            
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
          
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", ""));
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default"
                    ,
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                    );
            });

            app.UseSpa(spa =>
            {
            // To learn more about options for serving an Angular SPA from ASP.NET Core,
            // see https://go.microsoft.com/fwlink/?linkid=864501

            spa.Options.SourcePath = "ClientApp";

                if (app.Environment.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
                
            });
            
            try
            {
                System.Threading.Thread.Sleep(2000);
                Console.Title = $"Clinic Service (Version: {FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion})";
            }
            catch (Exception)
            {
            }
app.Run();