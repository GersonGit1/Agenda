namespace Agenda.Models
{
    public class UpdateTarea
    {
        //esta clase es para recibir el modelo de datos que se envía desde el frontend
        //cuado manipulamos los eventos y sólamente se actualizan las fechas de inicio y fin
        //ASP.NET no mapea los datos si vienen nulos, entonces estos atributos no podemos añadirlos 
        //a la clase tarea

        public int Id { get; set; }
        public string Start { get; set; }
        public string? End { get; set; }
    }
}
