using Agenda.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Security.Claims;

namespace Agenda.Controllers
{
	public class UsuarioController : Controller
	{
		// GET: UsuarioController
		public ActionResult Index()
		{
			return View();
		}

		// GET: UsuarioController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: UsuarioController/Create
		public ActionResult Create()
		{
			return View("Crear");
		}

		// POST: UsuarioController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				//validamos que las contraseñas no sean distintas
				if (collection["contraseña"] != collection["ConfirmarContraseña"])
				{
					ViewBag.Error = "Las contraseñas deben ser iguales";
					return View("Crear");
				}

				Usuario usuario = new();
				MantenimientoUsuario mantenimiento = new();
				usuario.Nombre = collection["nombre"];
				usuario.Contraseña = collection["contraseña"];
				mantenimiento.Crear(usuario);
				return RedirectToAction("Login");
			}
			catch
			{
				return View();
			}
		}

		// GET: UsuarioController/Edit/5{
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: UsuarioController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: UsuarioController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: UsuarioController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		//login de los usuarios
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(Usuario user)
		{   
            try
            {
				Conexion conex = new();

				using (SqlConnection con = new(conex.getCadConexion()))
				{
					string query = "SELECT Id, nombre,contraseña FROM Usuario WHERE nombre = @nombre";
					using (SqlCommand cmd = new(query, con))
					{
						con.Open();
						cmd.Parameters.Add("@nombre",System.Data.SqlDbType.VarChar).Value = user.Nombre;
						var usuario = cmd.ExecuteReader();
							//si la consulta devuelve un valor
						if (usuario.Read())
						{
							int id = usuario.GetInt32(0);
							string hashedPassword = usuario.GetString(2);
							var hasher = new PasswordHasher<object?>();
							var resultado = hasher.VerifyHashedPassword(null,hashedPassword,user.Contraseña);
							if (resultado == PasswordVerificationResult.Success)
							{
								List<Claim> claims = new List<Claim>()
								{
									new Claim(ClaimTypes.NameIdentifier, id.ToString()),
									new Claim(ClaimTypes.Name, user.Nombre)
								};


								//definir la identidad de la afirmación
								ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                                //crear objeto para definir las propiedades de autenticacion
                                AuthenticationProperties propiedadesDeAutenticacion = new();
                                //propiedades para que el usuario pueda actualizar la sesion
                                propiedadesDeAutenticacion.AllowRefresh = true;

								//establecer la duración de la sesión
								propiedadesDeAutenticacion.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(5);


                                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), propiedadesDeAutenticacion);

                                return RedirectToAction("Index", "Tarea");
                            }
						}
                        else
                        {
                            ViewBag.Error = "Credenciales incorrectas o cuenta no registrada";
                        }
                    }
					//retornamos una vista, como no especificamos el nombre, buscará en Views/Usuario/Login
                    return View();
                }
			}
			catch (Exception e)
			{
                ViewBag.Error = e.Message;
                return View();
            }
		}
		[HttpGet]
        public IActionResult Login()
        {
            //para gestion del usuario actualmente autenticado
            ClaimsPrincipal usuarioActual = HttpContext.User;
            //verificar si hay alguien conectado
            if (usuarioActual.Identity != null)
            {
                if (usuarioActual.Identity.IsAuthenticated)
                {   //si está autenticado lo dirigimos a la página principal
                    return RedirectToAction("Index", "Usuario");
                }
            }
            //si no ha iniciado sesión correctamente los enviamos d nuevo a login
            return View();
        }

		//Cerrar sesion
		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			//cerrar sesión borrando la cookie
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login");
		}
    }
}
