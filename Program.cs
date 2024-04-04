using System.Text;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using ScriveAPI.Data;
using ScriveAPI.Helpers;
using ScriveAPI.Models;
using ScriveAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    var frontendUrl = builder.Configuration["FrontendUrl"];
    options.AddPolicy("AllowFrontend",
        builder => builder.WithOrigins(frontendUrl)
                            .AllowAnyHeader()
                            .AllowAnyMethod());
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSecret"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddSingleton<TokenValidator>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new TokenValidator(config["JwtSecret"]);
});

builder.Services.AddSingleton<IConfiguration>(sp =>
{
    var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();
    return config;
});

var mongoUrl = MongoUrl.Create(builder.Configuration["MongoDbUrl"]);

builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoUrl));

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(mongoUrl.DatabaseName);
});

builder.Services.AddScoped<IMongoCollection<User>>(sp =>
{
    var database = sp.GetRequiredService<IMongoDatabase>();
    return database.GetCollection<User>("users");
});

builder.Services.AddScoped<IMongoCollection<Blog>>(sp =>
{
    var database = sp.GetRequiredService<IMongoDatabase>();
    return database.GetCollection<Blog>("blogs");
});

builder.Services.AddScoped<UserContext>();
builder.Services.AddTransient<UserServices>();

builder.Services.AddScoped<BlogContext>();
builder.Services.AddScoped<BlogServices>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Scrive API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Scrive API");
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
    RequestPath = "/public"
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();