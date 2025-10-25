using G3MWL.Models;
using G3MWL.Services;
using QuestPDF.Infrastructure; //  Add this for license declaration

var builder = WebApplication.CreateBuilder(args);

//  Declare QuestPDF license type
QuestPDF.Settings.License = LicenseType.Community;

//  Register HttpClient for Node.js API
builder.Services.AddHttpClient("G3MWLApi", client =>
{
    client.BaseAddress = new Uri("https://g3mwl-backend-updated.onrender.com/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

//  Register IHttpContextAccessor for services that need access to HttpContext
builder.Services.AddHttpContextAccessor();

//  Register authentication scheme for session-based auth
builder.Services.AddAuthentication("SessionScheme")
    .AddCookie("SessionScheme", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

//  Register custom services
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<ISideEffectService, SideEffectService>();
builder.Services.AddScoped<IVisitService, VisitService>();
builder.Services.AddScoped<IAuthService, AuthService>(); // for login/register

//  Enable session for token storage
builder.Services.AddSession();

//  MVC and Razor Views
builder.Services.AddControllersWithViews();

var app = builder.Build();

//  Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();         // Enable session before auth
app.UseAuthentication();  // Add authentication middleware
app.UseAuthorization();   // Then authorization

//  Default route to Dashboard
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();










