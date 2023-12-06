using BusinessLogic.DTOs.ProductsDto;
using BusinessLogic.Interfaces.IAuthService;
using BusinessLogic.Interfaces.ICartService;
using BusinessLogic.Interfaces.ProductServices;
using BusinessLogic.Services.AuthServices;
using BusinessLogic.Services.CartService;
using BusinessLogic.Services.ProductServices;
using Entities.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Restaurant.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureLoggerService();
builder.Services.ConfigureConnectionString(builder.Configuration);

//Add Auto-Mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Add Identity
builder.Services.ConfigureIdentity();

//Add Jwt
builder.Services.ConfigureJWT(builder.Configuration);

//Add custom Services
builder.Services.AddScoped<IProductService<ProductDto>, ProductService>();
builder.Services.AddScoped<IRegisterService<IdentityResult>, RegisterService>();
builder.Services.AddScoped<ICartService<CartItem>, CartService>();

builder.Services.AddControllers();

//Add Authentication
builder.Services.AddAuthentication();

//Add Swagger
builder.Services.ConfigureSwagger();
builder.Services.ConfigureSession();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSession();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
