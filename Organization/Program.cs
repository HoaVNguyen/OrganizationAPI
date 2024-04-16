using AutoMapper;
using FluentValidation;
using Organization.Domain.Entity;
using Organization.Features.Addition.Map;
using Organization.Features.EmployeeFeatures.Map;
using Organization.Features.OfficeFeatures.Map;
using Organization.Features.ParishFeatures;
using Organization.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<OrganizationDbContext>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddValidatorsFromAssemblyContaining<EmployeeValidator>();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new EmployeeMappingProfile());
    mc.AddProfile(new OfficeMappingProfile());
    mc.AddProfile(new ParishMappingProfile());
    mc.AddProfile(new TeamMappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();

builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();


app.Run();
