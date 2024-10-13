using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NZWalksAPI.Data;
using NZWalksAPI.Mappings;
using NZWalksAPI.Middlewares;
using NZWalksAPI.Repositories;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var logger = new LoggerConfiguration().WriteTo.Console().WriteTo.File("Logs/NzWalks_Log.txt", rollingInterval: RollingInterval.Day).MinimumLevel.Information().CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Nz Walks API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme // เพิ่มการกำหนดความปลอดภัย ให้กับ Swagger โดยใช้ JWT Authentication
    {
        Name = "Authorization", // Name: กำหนดให้เป็น "Authorization" ซึ่งจะเป็นชื่อของ Header ที่ใช้ส่ง JWT token มาใน API.
        In = ParameterLocation.Header, // In: กำหนดให้ข้อมูล JWT ถูกส่งผ่าน Header ของ HTTP request.
        Type = SecuritySchemeType.ApiKey, // Type: กำหนดเป็น ApiKey ซึ่งหมายความว่า JWT จะถูกใช้เป็น "API Key" ในคำขอ.
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    // เพิ่มข้อกำหนดความปลอดภัยให้กับเอกสาร Swagger API เพื่อให้ Swagger UI แสดงกลไกการยืนยันตัวตนด้วย JWT.
    // ใช้ OpenApiSecurityRequirement เพื่อกำหนดว่า JWT ต้องเป็นส่วนหนึ่งของการเรียกใช้งาน API ทั้งหมด (ขึ้นอยู่กับการตั้งค่า).
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference // Reference: อ้างอิงถึงการตั้งค่าการรักษาความปลอดภัยที่กำหนดไว้ใน AddSecurityDefinition
                {
                    Type = ReferenceType.SecurityScheme, // Type: เป็น SecurityScheme ซึ่งหมายถึงการใช้ JWT authentication.
                    Id = JwtBearerDefaults.AuthenticationScheme // Id: อ้างอิงถึง JwtBearerDefaults.AuthenticationScheme เพื่อบอกว่าเรากำลังใช้ JWT.
                },
                Scheme = "Oauth2", // Scheme: กำหนดเป็น "Oauth2", ซึ่งเป็นโครงสร้างพื้นฐานที่ทำงานกับ JWT.
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header // In: กำหนดให้ JWT ถูกส่งมาผ่าน Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddDbContext<NzWalksDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("NzWalksConnectionString")));

builder.Services.AddDbContext<NzWalksAuthDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("NzWalkAuthConnectionString")));

builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();
builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IImageRepository, LocalImageRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NzWalks")
    .AddEntityFrameworkStores<NzWalksAuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true, // ตรวจสอบผู้ออก
    ValidateAudience = true, // ตรวจสอบผู้ชม
    ValidateLifetime = true, // ตรวจสอบอายุการใช้งาน
    ValidateIssuerSigningKey = true, // ตรวจสอบรหัสการลงนามของผู้ออก
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
// It is a middleware used to enable ASP.NET Core.
app.UseStaticFiles(new StaticFileOptions
{
    // PhysicalFileProvider ใช้ในการกำหนดตำแหน่งของไฟล์ที่เก็บอยู่ในเครื่องเซิร์ฟเวอร์ (Physical Files)
    // Path.Combine(Directory.GetCurrentDirectory(), "Images"): จะกำหนดให้โฟลเดอร์ Images ใน directory ปัจจุบัน (ที่โปรเจกต์ ASP.NET Core ทำงานอยู่) เป็นที่เก็บไฟล์ static
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
    // https://localhost:1234/Images
});

app.MapControllers();

app.Run();
