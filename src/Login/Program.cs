using Login.DataContext;
using Login.Identity;
using Login.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Login
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                //configures PascalCase formatting instead of the default camelCase formatting
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            var policyName = builder.Configuration["Config:PolicyName"];

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(policyName, builder => builder

#if DEBUG
                .WithOrigins("https://localhost:4200")
#else
                
#endif

                //.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlite(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)
                    ));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<JwtHandler>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Config:TokenAudience"],
                        ValidIssuer = builder.Configuration["Config:TokenIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Config:TokenKey"]))
                    };

                });
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors(policyName);
            app.UseAuthentication();
            app.UseAuthorization();

            using (var srv = builder.Services.BuildServiceProvider().GetService<ApplicationContext>())
            {
                DataSeeder.Initialize(srv).Wait();
            }    
            //var serviceProvider = app.Services.GetService<ApplicationContext>();
            //DataSeeder.Initialize(serviceProvider).Wait();

            app.MapControllers();

            app.Run();
        }
    }
}