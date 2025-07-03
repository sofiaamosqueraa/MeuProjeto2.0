using MeuProjeto2._0.Services;

var builder = WebApplication.CreateBuilder(args);

// Adicionar MVC + Serviços personalizados
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<OpenRouterService>();
builder.Services.AddScoped<XmlParserService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // ? wwwroot
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Form}/{action=Index}/{id?}");

app.Run();
