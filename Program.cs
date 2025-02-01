using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NamaProyekAnda.Data;  // Ganti dengan namespace aplikasi Anda
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Konfigurasi DbContext dengan string koneksi dari appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Tambahkan layanan untuk MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Konfigurasi HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // Menambahkan HSTS (HTTP Strict Transport Security)
}

app.UseHttpsRedirection();  // Mengarahkan HTTP ke HTTPS
app.UseStaticFiles();  // Menyajikan file statis seperti CSS dan JS

app.UseRouting();  // Menyusun rute

app.UseAuthentication();  // Menyertakan autentikasi jika diperlukan
app.UseAuthorization();  // Menyertakan otorisasi

// Konfigurasi rute aplikasi
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();  // Menjalankan aplikasi
