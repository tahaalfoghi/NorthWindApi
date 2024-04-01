using Microsoft.EntityFrameworkCore;
using northwindAPI.Models;
using northwindAPI.MapperService;
using northwindAPI.RepositoryService;
using northwindAPI.PatternService.Repository;
using northwindAPI.PatternService.UnitOfWork;
using northwindAPI.Middleware;
using northwindAPI.PatternService;
using Serilog.AspNetCore;
using Serilog;
using northwindAPI.northwindEFCore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using northwindAPI.EFCore.Data.Models;




var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme{
        In  = ParameterLocation.Header,
        Name ="Authorization",
        Type = SecuritySchemeType.ApiKey,

    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Debug()
    .WriteTo.Console()
    .WriteTo.File("Log/log.txt",rollingInterval:RollingInterval.Minute).CreateLogger();


builder.Services.AddDbContext<AppDbContext>(
    op => op.UseSqlServer(builder.Configuration.GetConnectionString("constr")));

builder.Services.AddDbContext<UserDbContext>(op=> op.UseSqlServer(
    builder.Configuration.GetConnectionString("constr")
));

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<User>()
.AddEntityFrameworkStores<UserDbContext>().AddApiEndpoints(); 

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
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapIdentityApi<User>();
app.UseExceptionHandler();
app.MapControllers();
app.Run();
