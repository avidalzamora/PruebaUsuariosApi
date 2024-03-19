using PruebaUsuarios.Domain.Models.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaUsuarios.Data.Repositorios
{
    public interface ICorreo
    {
        bool EnviarCorreo(string subject, string CuerpoMensaje, string toEmail, string ccEmail);
        bool EnviarCorreo(string subject, string CuerpoMensaje, string toEmail, string ccEmail, MemoryStream ms, string namedata);
    }
}
