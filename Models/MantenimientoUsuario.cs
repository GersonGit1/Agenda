using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;

namespace Agenda.Models
{
    public class MantenimientoUsuario
    {
        //crear un usuario
        public int Crear(Usuario usuario)
        {
            Conexion conexion = new();

            using (SqlConnection con = new(conexion.getCadConexion()))
            {
                //hasheamos la contraseña que el usuario ingresa
                var hasher = new PasswordHasher<Usuario>();
                string hashedPassword = hasher.HashPassword(usuario, usuario.Contraseña);
                string query = "INSERT INTO Usuario (nombre,contraseña) values (@nombre,@contraseña)";
                con.Open();
                using (SqlCommand cmd = new(query,con))
                {
                    cmd.Parameters.Add("@nombre", System.Data.SqlDbType.VarChar);
                    cmd.Parameters.Add("@contraseña", System.Data.SqlDbType.NVarChar);

                    cmd.Parameters["@nombre"].Value = usuario.Nombre;
                    cmd.Parameters["@contraseña"].Value = hashedPassword;

                    int i = cmd.ExecuteNonQuery();
                    return i;
                }
            }
        }

         
    }
}
