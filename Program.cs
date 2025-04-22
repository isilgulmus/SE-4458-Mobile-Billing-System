using Microsoft.EntityFrameworkCore;
using MobileBillingSystem.Data;
using Microsoft.OpenApi.Models;
using MobileBillingSystem.Service;
using MobileBillingSystem.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<YourDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<BillService>();
builder.Services.AddScoped<UsageService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; 
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = "MobileBillingAPI", 
            ValidAudience = "MobileBillingClient", 
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("UbyNbFvlonVCj-WPoZRnlXWGZRQZTxIAhuIxX7obGt4")) // Secret key for signing tokens
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() 
              .AllowAnyMethod()  
              .AllowAnyHeader(); 
    });
});



builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MobileBilling API", Version = "v1" });

    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddControllers();  
var app = builder.Build();

app.UseCors("AllowAll");


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MobileBilling API v1");
    c.RoutePrefix = string.Empty;  
});



using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<YourDbContext>();
    if (!context.Subscribers.Any())  
    {
        context.Subscribers.AddRange(
            new Subscriber { Name = "John Doe", Email = "john.doe@example.com" },
            new Subscriber { Name = "Jane Smith", Email = "jane.smith@example.com" }
        );
        context.SaveChanges(); 
    }
}

app.MapControllers();  

app.UseHttpsRedirection();

app.UseAuthentication(); 

app.UseAuthorization(); 

app.Run();
