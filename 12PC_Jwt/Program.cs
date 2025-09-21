using Jwt.BusinessLayer.Abstract;
using Jwt.BusinessLayer.Concrete;
using Jwt.BusinessLayer.ML;
using Jwt.DataAccessLayer.Abstract;
using Jwt.DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<IUserDal, EfUserDal>();
builder.Services.AddScoped<IPackageService, PackageManager>();
builder.Services.AddScoped<IPackageDal, EfPackageDal>();
builder.Services.AddScoped<IPlaylistService, PlaylistManager>();
builder.Services.AddScoped<IPlaylistDal, EfPlaylistDal>();
builder.Services.AddScoped<IPlaylistSongService, PlaylistSongManager>();
builder.Services.AddScoped<IPlaylistSongDal, EfPlaylistSongDal>();
builder.Services.AddScoped<IRecommendationService, RecommendationManager>();
builder.Services.AddScoped<IRecommendationDal, EfRecommendationDal>();
builder.Services.AddScoped<ISongService, SongManager>();
builder.Services.AddScoped<ISongDal, EfSongDal>();
builder.Services.AddScoped<IUserSongHistoryService, UserSongHistoryManager>();
builder.Services.AddScoped<IUserSongHistoryDal, EfUserSongHistoryDal>();

builder.Services.AddSingleton(sp =>
{
    var engine = new RecommendationEngine();

    // model dosyasý varsa yükle, yoksa boþ dön
    if (File.Exists("recommendationModel.zip"))
    {
        var model = engine.LoadModel("recommendationModel.zip");
        return model;
    }
    return null;
});

var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<JwtContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS ekle
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .WithOrigins("https://localhost:7179") // Frontend port
            .AllowAnyMethod()
            .AllowAnyHeader());
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

// CORS’u uygula
app.UseCors("AllowAll");

app.MapControllers();

app.Run();
