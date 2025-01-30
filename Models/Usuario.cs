using System.ComponentModel.DataAnnotations;

namespace Agenda.Models
{
	public class Usuario
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "El nombre de usuario es requerido")]
		public string Nombre { get; set; }
		[Required(ErrorMessage = "La contraseña es requerida")]
		public string Contraseña { get; set; }
		public string ConfirmarContraseña { get; set; }
		public string FechaRegistro { get; set; }
	}
}
