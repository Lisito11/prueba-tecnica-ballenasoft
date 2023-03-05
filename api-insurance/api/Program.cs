using api.Helpers;
using api.Middlewares;
using api.Repositories;
using api.Services;

var builder = WebApplication.CreateBuilder(args);

// DBContext.
builder.Services.ConfigureDBContext(builder.Configuration);

//Service
builder.Services.AddScoped<IInsuranceService, InsuranceService>();

//Repository
builder.Services.AddScoped<IInsuranceRepository, InsuranceRepository>();

//AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();

app.UseCors("AllowOrigin");

app.UseAuthorization();

app.MapControllers();

app.UseCustomMiddleware();

app.Run();

