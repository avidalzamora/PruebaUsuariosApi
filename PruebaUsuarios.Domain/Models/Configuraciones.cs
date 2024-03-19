using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaUsuarios.Domain.Models
{
    public class Configuraciones
    {
        public EmailConfig? Emailing { get; set; }
    }

    public class EmailConfig
    {
        public string? Servidor { get; set; }
        public int Puerto { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? NombreCorreo { get; set; }
        public bool EnableSSL { get; internal set; }
       
    }
}
