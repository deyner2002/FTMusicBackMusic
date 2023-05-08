using Core.Loyal.Providers;
using Core.Loyal.Services;
using CORE.Loyal.Interfaces.Providers;
using CORE.Loyal.Interfaces.Services;
using Support.Loyal.DTOs;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));


builder.Services.AddScoped<ICancionServices, CancionServices>();
builder.Services.AddScoped<ICancionProvider, CancionProvider>();
builder.Services.AddScoped<IComentarioProvider, ComentarioProvider>();
builder.Services.AddScoped<ILikeProvider, LikeProvider>();


builder.Services.AddSwaggerGen();
var _MyCors = "MyCors";
var HostFront = builder.Configuration.GetValue<string>("HostFront");
/*builder.Services.AddCors(options =>
{
    options.AddPolicy(name: _MyCors, builder => builder

               //.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
               .WithOrigins(HostFront)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials()

   );
});*/

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: _MyCors, builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(_MyCors);
app.UseAuthorization();

app.MapControllers();

app.Run();
