using Microsoft.EntityFrameworkCore;
using northwindAPI.Models;
using northwindAPI.MapperService;
using northwindAPI.RepositoryService;
using northwindAPI.PatternService.Repository;
using northwindAPI.PatternService.UnitOfWork;
using northwindAPI.Middleware;
using northwindAPI.PatternService;




var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(
    op => op.UseSqlServer(builder.Configuration.GetConnectionString("constr")));
builder.Services.AddLogging(loggingBuilder =>loggingBuilder .AddDebug());
// Ensure you're referencing the correct AddAutoMapper method
builder.Services.AddAutoMapper(typeof(MapperConfig));
builder.Services.AddScoped(typeof(IRepository<>),typeof(RepositoryBase<>));
builder.Services.AddScoped(typeof(IUnitOfWork),typeof(UnitOFWork));
builder.Services.AddScoped(typeof(CustomerRepository));
builder.Services.AddScoped(typeof(OrderDetailsRepository));
builder.Services.AddScoped(typeof(TerritoryRepository));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.MapControllers();

app.Run();
