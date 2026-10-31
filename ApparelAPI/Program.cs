using CapaNegocio;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Capa_Datos.Mongo;            
using ApparelAPI.Middleware;       

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingresa 'Bearer' seguido de tu token JWT"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// JWT
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

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                builder.Configuration["Jwt:Key"]!
            )
        )
    };
});

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirApparelFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://127.0.0.1:5502",
                "http://localhost:5502",
                "http://127.0.0.1:5500",
                "https://white-sand-0fc57e010.7.azurestaticapps.net",
                "https://www.apparelpos.lat",
                "https://apparelmovilapi-dgdpgjc5gafxfyfp.canadacentral-01.azurewebsites.net"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Capa de datos y negocio
builder.Services.AddScoped<VentaDAL>(provider =>
    new VentaDAL(
        builder.Configuration.GetConnectionString("DefaultConnection")!
    ));

builder.Services.AddScoped<VentaBLL>();

builder.Services.AddSingleton(new MongoMetricasContext(
    builder.Configuration["MongoDBSettings:ConnectionString"] ?? string.Empty,
    builder.Configuration["MongoDBSettings:DatabaseName"] ?? string.Empty));

var app = builder.Build();

var mongo = app.Services.GetRequiredService<MongoMetricasContext>();
if (!mongo.EstaConfigurado)
{
    app.Logger.LogWarning("Logs deshabilitados: {Estado}", mongo.Estado);
}

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("PermitirApparelFrontend");

app.UseAuthentication();
app.UseMiddleware<RegistroPeticionesMiddleware>();   
app.UseAuthorization();

app.MapControllers();

app.Run();