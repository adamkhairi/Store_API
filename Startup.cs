using System.Reflection;
using System.Text;
using Auth_API.Helpers;
using Auth_API.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Store_API.Data;
using Store_API.Services.Auth;
using Store_Shared.Models;

//using Store_API.Services.Categories;

namespace Store_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Jwt>(Configuration.GetSection("Jwt"));

            services.AddCors();

            //!! Add Identity with Roles ===>

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();


            //!! Add DBContext ===>
            services.AddDbContext<AppDbContext>(option =>
                    //option.UseSqlServer(Configuration.GetConnectionString("Omar"))
                    option.UseSqlServer(Configuration.GetConnectionString("Adam"))
                //option.UseSqlServer(Configuration.GetConnectionString("Salma"))
            );


            //!! _ DependencyInjection _ ===>
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();


            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //services.AddAutoMapper(typeof(Startup));


            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>

                {
                    o.RequireHttpsMetadata = false;
                    //o.Authority = "https://localhost:5001";
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });


            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Store_API", Version = "v1"}); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Store_API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
// global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}