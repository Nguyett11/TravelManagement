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

// Add services to the container.

builder.Services.AddControllers();
// Cấu hình Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Kiểm tra môi trường phát triển và cấu hình Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply middleware
app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/Users/"), appBuilder =>
{
    appBuilder.UseMiddleware<AuthToken>();
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
