using Microsoft.AspNetCore.Mvc;
using MISA.WebFresher2023.Demo.BL.Service;
using MISA.WebFresher2023.Demo.DL;
using MISA.WebFresher2023.Demo.DL.Repository;
using MISA.WebFresher2023.Demo.Middleware;
using MISA.WebFresher2023.Demo.Common;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Lấy data từ file 
builder.ReadConfig();

// Tạo config instance

AppConfig appConfig = builder.Configuration.GetConfig();

builder.Services.AddSingleton(appConfig);

// Add services to the container.

builder.Services.AddControllers();

// Định dạnh các thuộc tính trả về là kiểu Pascal
builder.Services.AddControllers().AddJsonOptions(option =>
{
    option.JsonSerializerOptions.PropertyNamingPolicy = null;

});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Thêm Cors cho font-end
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyVueApp",
                      policy =>
                      {
                          policy.WithOrigins("*")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                      });
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Sử dụng automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountPropertyRepository, AccountPropertyRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerAndGroupRepository, CustomerAndGroupRepository>();
builder.Services.AddScoped<ICustomerGroupRepository, CustomerGroupRepository>();
builder.Services.AddScoped<IContactInforRepository, ContactInforRepository>();
builder.Services.AddScoped<ITermsOfPaymentRepository, TermsOfPaymentRepository>();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddScoped<INationRepository, NationRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IDistrictRepository, DistrictRepository>();
builder.Services.AddScoped<ICommuneRepository, CommuneRepository>();
builder.Services.AddScoped<IOtherLocationRepository, OtherLocationRepository>();
builder.Services.AddScoped<ISpecificAddressRepository, SpecificAddressRepository>();
builder.Services.AddScoped<IReceiptRepository, ReceiptRepository>();
builder.Services.AddScoped<IBookKeepingRepository, BookKeepingRepository>();

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountPropertyService, AccountPropertyService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerAndGroupService, CustomerAndGroupService>();
builder.Services.AddScoped<ICustomerGroupService, CustomerGroupService>();
builder.Services.AddScoped<IContactInforService, ContactInforService>();
builder.Services.AddScoped<ITermsOfPaymentService, TermsOfPaymentService>();
builder.Services.AddScoped<IBankAccountService, BankAccountService>();
builder.Services.AddScoped<INationService, NationService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IDistrictService, DistrictService>();
builder.Services.AddScoped<ICommuneService, CommuneService>();
builder.Services.AddScoped<IOtherLocationService, OtherLocationService>();
builder.Services.AddScoped<ISpecificAddressService, SpecificAddressService>();
builder.Services.AddScoped<IReceiptService, ReceiptService>();
builder.Services.AddScoped<IBookKeepingService, BookKeepingService>();

builder.Services.AddScoped<IMSDatabase, MSDatabase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MyVueApp");

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionMiddleware>();

app.Run();
