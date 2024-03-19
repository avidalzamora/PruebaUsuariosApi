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
        Task<int> InsertarUsuario(Usuario usuario);
        Task<int> UpdateUsuario(Usuario usuario);
        Task<bool> DeleteUsuario(Usuario usuario);
    }
}
