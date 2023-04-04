using EmsApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<EmsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnString")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Adding Authentication  
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})



   .AddJwtBearer(options =>
   {
    /*   options.RequireHttpsMetadata = false;
       options.SaveToken = true;*/
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateAudience = false,
           ValidateIssuer = false,
           ValidIssuer = builder.Configuration["Issuer"],
           ValidAudience = builder.Configuration["Audience"],
           //ValidateIssuerSigningKey = true,
           IssuerSigningKey = new SymmetricSecurityKey(
                           Encoding.ASCII.GetBytes(builder.Configuration["Secret"]))
       };
   });



builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Capgemini.EMSApi", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
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
                        new string[]{}
                    }
                });
});

builder.Services.AddCors();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["ConnectionStrings:StorAccConnString:blob"], preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["ConnectionStrings:StorAccConnString:queue"], preferMsi: true);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}
app.UseSwagger();
app.UseSwaggerUI(c =>
c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "Capgemini.EMSApi v1"));

app.UseHttpsRedirection();
app.UseCors(policy => 
    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
