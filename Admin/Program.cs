using RealEstate.Admin.Services;
using Common;
using Common.ExceptionHandle;
using Common.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RealEstate.Application.Services;
using System.Text;
using RealEstate.Application.Handlers;
using RealEstate.Application;
using RealEstate.Internal.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v2", new OpenApiInfo { Title = "Admin Application", Version = "v1" });
    //x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    //{
    //    Name = "Authorization",
    //    Type = SecuritySchemeType.ApiKey,
    //    Scheme = "Bearer",
    //    BearerFormat = "JWT",
    //    In = ParameterLocation.Header,
    //    Description = "Please enter token enter 'bearer' [space] <token>"
    //});
    //x.AddSecurityRequirement(new OpenApiSecurityRequirement {
    //                {
    //                    new OpenApiSecurityScheme
    //                    {
    //                        Reference=new OpenApiReference
    //                        {
    //                            Type=ReferenceType.SecurityScheme,
    //                            Id="Bearer"
    //                        }
    //                    },
    //                    new string[]{ }
    //                }
    //            });
});

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Configure Authorization
builder.Services.AddAuthorization();

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<ITestServices, TestServices>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add services to the container.
builder.Services.Configure<GoogleMapsSettings>(builder.Configuration.GetSection("GoogleMaps"));
builder.Services.AddHttpClient(); // Add IHttpClientFactory

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(TestServicesHandler).Assembly)); // Replace typeof(Program).Assembly with the assembly containing your handlers.
builder.Services.AddInfrastructureServices();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "Admin app v1");
    });
}



app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseRouting();

app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
