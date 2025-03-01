using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Agenda.Models
{
    public class MantenimientoTarea
    {
        //crear tareas
        public int AñadirTarea(Tarea tarea)
        {   
            Conexion conexion = new Conexion();
            using (SqlConnection con = new(conexion.getCadConexion()))
            {
                string query = "INSERT INTO Tareas VALUES (@titulo, @descripcion, @fecha, @IdUsuario, @fin)";
                con.Open();
                using (SqlCommand cmd = new(query, con))
                {
                    cmd.Parameters.Add("@titulo", System.Data.SqlDbType.VarChar);
                    cmd.Parameters.Add("@descripcion", System.Data.SqlDbType.VarChar);
                    cmd.Parameters.Add("@fecha", System.Data.SqlDbType.DateTime);
                    cmd.Parameters.Add("@fin", System.Data.SqlDbType.DateTime);
                    cmd.Parameters.Add("@IdUsuario", System.Data.SqlDbType.Int);
                    //cmd.Parameters.Add("@notificacion",System.Data.SqlDbType.Time);

                    cmd.Parameters["@titulo"].Value = tarea.Titulo;
                    cmd.Parameters["@descripcion"].Value = tarea.Descripcion;
                    cmd.Parameters["@fecha"].Value = tarea.fecha;
                    cmd.Parameters["@fin"].Value = tarea.Fin;
                    cmd.Parameters["@IdUsuario"].Value = tarea.IdUsuario;

                    int i = cmd.ExecuteNonQuery();
                    return i;
                }
            };
        }

        //Leer y mostrar las tareas
        public JsonResult MostrarTareas(int id)
        {
            Conexion conexion = new Conexion();
            var eventos = new List<object>();
            using (SqlConnection con = new(conexion.getCadConexion()))
            {
                string query = "select Id, titulo as title, descripcion, inicio as 'start', fin as 'end' from Tareas WHERE Id_Usuario = @Usuario";
                con.Open();
                using (SqlCommand cmd = new(query, con))
                {
                    cmd.Parameters.Add("@Usuario",System.Data.SqlDbType.Int).Value = id;
                    var reader = cmd.ExecuteReader();                    
                    while (reader.Read())
                    {
                        eventos.Add(new
                        {
                            id = reader["Id"],
                            title = reader["title"],
                            start = ((DateTime)reader["start"]).ToString("yyyy-MM-ddTHH:mm:ss"),
                            end = reader["end"] == DBNull.Value ?null : ((DateTime)reader["end"]).ToString("yyyy-MM-ddTHH:mm:ss"),
                             descripcion = reader["descripcion"] 
                        });
                    }
                    return new JsonResult(eventos);
                }
            };
        }

        //Actualizar la hora de la tarea

        public int ActualizarTarea(Tarea tarea)
        {
            Conexion con = new();
            using (SqlConnection conn = new(con.getCadConexion()))
            {
                string query = "UPDATE Tareas SET inicio = @fecha, fin = @fin WHERE Id = @Id";
                conn.Open();
                using (SqlCommand cmd = new(query,conn))
                {
                    cmd.Parameters.Add("@fecha", System.Data.SqlDbType.DateTime);
                    cmd.Parameters.Add("@fin", System.Data.SqlDbType.DateTime);
                    cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int);

                    cmd.Parameters["@fecha"].Value = tarea.fecha;
                    cmd.Parameters["@fin"].Value = tarea.Fin;
                    cmd.Parameters["@Id"].Value = tarea.Id;

                    int i = cmd.ExecuteNonQuery();
                    return i;
                }
            }
        }
    }
}
