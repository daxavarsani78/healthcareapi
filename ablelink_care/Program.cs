using Data.Data;
using Data.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services.Services.PublicServices;
using Services.Services.UserIdentity;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string connectionString = "Server=DAXAPC\\MSSQLSERVER01;Database=AbleLinkDb;Integrated Security=True";
builder.Services.AddDbContext<AppDB>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentity<Tbl_Users, IdentityRole>().AddEntityFrameworkStores<AppDB>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
  .AddJwtBearer(option =>
  {
      option.SaveToken = true;
      option.RequireHttpsMetadata = false;
      option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
      {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidAudience = "User",
          ValidIssuer = "https://localhost:44317/",
          TryAllIssuerSigningKeys = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsMySecureKey12345678"))
      };
  });
builder.Services.AddControllers();
builder.Services.AddTransient<IUserIdentityService, UserIdentityService>();
builder.Services.AddTransient<IPublicService, PublicService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.UseCors("AllowAll");
app.Run();
app.Run();
