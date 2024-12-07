using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Backend.DataConnection;
using Microsoft.OpenApi.Models;
using Backend.Middleware;
using System.Reflection.PortableExecutable;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});



// Cấu hình DbContext
builder.Services.AddDbContext<DataBaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("QLDuLich"));
});



// Cấu hình Authentication và Authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "Test.com",
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("d1455409-2844-45b4-b8b6-a2c063a1d46e"))

        };
    });

builder.Services.AddAuthorization();
//Chạy lại thử i còn bên TourController có sửa gig kh, kh
// Add services to the container.

builder.Services.AddControllers();
// Cấu hình Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();

// Thêm session vào DI container
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian tồn tại của session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Cookie này cần thiết để hoạt động
});



var app = builder.Build();


// Kiểm tra môi trường phát triển và cấu hình Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

// Thêm Middleware định tuyến trước khi cấu hình Endpoints
// app.UseRouting();

// Sử dụng session middleware
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// Apply middleware
app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/Users/"), appBuilder =>
{
    appBuilder.UseMiddleware<AuthToken>();
});
    app.MapControllers();

app.Run();