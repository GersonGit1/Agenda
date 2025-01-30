namespace Agenda.Models
{
    public class Conexion
    {
        private string cadConexion = string.Empty;
        public Conexion()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            cadConexion = builder.GetSection("ConnectionStrings:CadenaConexion").Value;
        }
        public string getCadConexion()
        {
            return cadConexion;
        }
    }
}
