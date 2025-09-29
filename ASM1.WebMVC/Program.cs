using ASM1.Repository.Data;
using ASM1.Repository.Repositories;
using ASM1.Repository.Repositories.Interfaces;
using ASM1.Service.Services;
using ASM1.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


//add repositories
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IQuotationRepository, QuotationRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ISalesContractRepository, SalesContractRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//add services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IQuotationService, QuotationService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ISalesContractService, SalesContractService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

//add AutoMapper - cần import từ assembly chứa MappingProfile
builder.Services.AddAutoMapper(typeof(ASM1.Service.Models.MappingProfile));


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
