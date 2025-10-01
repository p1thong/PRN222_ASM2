using ASM1.Repository.Data;
using ASM1.Repository.Repositories;
using ASM1.Repository.Repositories.Interfaces;
using ASM1.Service.Services;
using ASM1.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var options = new WebApplicationOptions
{
    Args = args,
    WebRootPath = "wwwroot"
};

var builder = WebApplication.CreateBuilder(options);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//add connection String
builder.Services.AddDbContext<CarSalesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CarSalesDB"))
);


//add repositories
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IQuotationRepository, QuotationRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ISalesContractRepository, SalesContractRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
builder.Services.AddScoped<IVehicleModelRepository, VehicleModelRepository>();
builder.Services.AddScoped<IVehicleVariantRepository, VehicleVariantRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//add services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IQuotationService, QuotationService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ISalesContractService, SalesContractService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IManufacturerService, ManufacturerService>();
builder.Services.AddScoped<IVehicleModelService, VehicleModelService>();
builder.Services.AddScoped<IVehicleVariantService, VehicleVariantService>();
builder.Services.AddScoped<IPromotionRuleService, PromotionRuleService>();

//add AutoMapper - cần import từ assembly chứa MappingProfile
builder.Services.AddAutoMapper(typeof(ASM1.WebMVC.Models.MappingProfile));
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<ITestDriveRepository, TestDriveRepository>();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
builder.Services.AddScoped<IDealerContractRepository, DealerContractRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();

//add services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IDealerService, DealerService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<IDealerContractService, DealerContractService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddScoped<IManufacturerService, ManufacturerService>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
