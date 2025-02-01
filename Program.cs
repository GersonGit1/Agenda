using Agenda.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Agenda
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			//registrar el filtro en el contrenedor de dependencias
			builder.Services.AddScoped<SessionValidationFilter>();
			// Add services to the container.
			builder.Services.AddControllersWithViews();
			//agregar el servicio de las sesiones e toda la aplicación
			builder.Services.AddHttpContextAccessor();
			//agregar autenticación con expreciones lambda
			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opcion =>
			{
				opcion.LoginPath = "/Usuario/Login"; //pagina de inicio de sesion
				opcion.ExpireTimeSpan = TimeSpan.FromMinutes(5);
				opcion.SlidingExpiration = true; //renueva la expiración si el usuario sigue activo
			});
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
            //agregar uso de autenticación
            app.UseAuthentication();

            app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Usuario}/{action=Login}/{id?}");

			app.Run();
		}
	}
}