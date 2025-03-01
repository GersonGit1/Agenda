using System.ComponentModel.DataAnnotations;

namespace Agenda.Models
{
    public class Tarea
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Asigna un título a la tarea")]
        [MaxLength(75, ErrorMessage = "Escribe un titulo con menos de 75 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Asigna una descripcion a la tarea")]
        [MaxLength(600, ErrorMessage = "Escribe una descripción con menos de 600 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Establece una fecha y hora para esta tarea")]
        public DateTime fecha { get; set; }
        public DateTime? Fin { get; set; }

        public TimeOnly Notificar { get; set; }
        public int IdUsuario { get; set; }

    }
}
