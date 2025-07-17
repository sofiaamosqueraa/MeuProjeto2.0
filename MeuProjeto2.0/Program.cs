using MeuProjeto2._0;
using MeuProjeto2._0.Models;       // Para reconhecer o MeuProjetoContext
using MeuProjeto2._0.Services;     // Os teus serviços personalizados
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adicionar MVC + Serviços personalizados
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<OpenRouterService>();
builder.Services.AddScoped<XmlParserService>();

// Configurar o Entity Framework com SQL Server, usando a connection string do appsettings.json
builder.Services.AddDbContext<MeuProjetoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configuração para produção (ex: página de erro e HSTS)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();  // para servir CSS, JS, imagens, etc.

app.UseRouting();

app.UseAuthorization();

// Rota padrão do MVC: chama FormController / Index se não especificado
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Form}/{action=Index}/{id?}");

app.Run();
