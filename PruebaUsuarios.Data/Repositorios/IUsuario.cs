using MySqlX.XDevAPI.Common;
using PruebaUsuarios.Domain.Models;
using PruebaUsuarios.Domain.Models.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PruebaUsuarios.Data.Repositorios
{
    public interface IUsuario
    {
        Task<IEnumerable<Usuario>> GetUsuarios();
        Task<Usuario> GetDetails(int id);
        Task<ResultDb> InsertarUsuario(Usuario usuario);
        Task<ResultDb> UpdateUsuario(Usuario usuario);
        Task<ResultDb> DeleteUsuario(Usuario usuario);
    }
}
