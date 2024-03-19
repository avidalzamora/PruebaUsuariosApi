using Dapper;
using MySql.Data.MySqlClient;
using PruebaUsuarios.Domain.Models.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PruebaUsuarios.Data.Repositorios
{
    public class UsuarioRepository : IUsuario
    {
        public MySqlConfig mySqlConfig { get; set; }
        public UsuarioRepository(MySqlConfig mySqlConfig)
        {
            this.mySqlConfig = mySqlConfig;
        }

        protected MySqlConnection dbconnection()
        {
            return new MySqlConnection(mySqlConfig.ConnectionString);
        }

        public async Task<bool> DeleteUsuario(Usuario us)
        {
            var db = dbconnection();
            var sql = "Update\tUSUARIO\r\n\t\t\tSet\tEliminado = 1\r\n\t\tWhere\tIdUsuario = @IdUsuario";
            var result = await db.ExecuteAsync(sql, new { IdUsuario = us.IdUsuario });
            return result>0;
        }

        public async Task<Usuario> GetDetails(int id)
        {
            var sql = "CALL `proyectos`.`UsuarioSelect`(@pID,null, null, null, null, null);";
            var db = dbconnection();
            //var sql = "Select\tIdUsuario\r\n\t\t\t,CodOrgano\r\n\t\t\t,Usuario.IdRol\r\n\t\t\t,CodOficina\r\n\t\t\t,PrimerNombre + CASE SegundoNombre WHEN '' THEN '' ELSE ' ' + SegundoNombre END + ' ' + PrimerApellido + CASE SegundoApellido WHEN '' THEN '' ELSE ' ' + SegundoApellido END AS PrimerNombre\r\n\t\t\t,PrimerNombre\r\n\t\t\t,SegundoNombre\r\n\t\t\t,PrimerApellido\r\n\t\t\t,SegundoApellido\r\n\t\t\t,Sexo\r\n\t\t\t,Nidentificacion\r\n\t\t\t,Ncarnet\r\n\t\t\t,UsuarioNombre\r\n\t\t\t,Email\r\n\t\t\t,Usuario.Activo\r\n\t\t\t,Roles.NombreRol\r\n\tFrom Usuario INNER JOIN Roles ON Usuario.IdRol = Roles.IdRol \r\n\tWhere Usuario.Eliminado = 0 and IdUsuario = @IdUsuario";

            Usuario? res = await db.QueryFirstOrDefaultAsync<Usuario>(sql, new { pID = id });
            return res;
        }

        public async Task<IEnumerable<Usuario>> GetUsuarios()
        {
            var db = dbconnection();
            //var sql = "Select\tIdUsuario\r\n\t\t\t,CodOrgano\r\n\t\t\t,Usuario.IdRol\r\n\t\t\t,CodOficina\r\n\t\t\t,PrimerNombre + CASE SegundoNombre WHEN '' THEN '' ELSE ' ' + SegundoNombre END + ' ' + PrimerApellido + CASE SegundoApellido WHEN '' THEN '' ELSE ' ' + SegundoApellido END AS PrimerNombre\r\n\t\t\t,PrimerNombre\r\n\t\t\t,SegundoNombre\r\n\t\t\t,PrimerApellido\r\n\t\t\t,SegundoApellido\r\n\t\t\t,Sexo\r\n\t\t\t,Nidentificacion\r\n\t\t\t,Ncarnet\r\n\t\t\t,UsuarioNombre\r\n\t\t\t,Email\r\n\t\t\t,Usuario.Activo\r\n\t\t\t,Roles.NombreRol\r\n\tFrom Usuario INNER JOIN Roles ON Usuario.IdRol = Roles.IdRol\r\n\tWhere Usuario.Eliminado = 0";
            var sql = "CALL `proyectos`.`UsuarioSelect`(null,null, null, null, null, null);";
            return await db.QueryAsync<Usuario>(sql, new {});
        }

        public async Task<int> InsertarUsuario(Usuario us)
        {
            var db = dbconnection();
            var sql = "Insert Into USUARIO (IdRol, CodOrgano, CodOficina,PrimerNombre,SegundoNombre,PrimerApellido,SegundoApellido,Sexo,Nidentificacion,Ncarnet,UsuarioNombre,Contrasenna,Email,Activo,UsuarioReg) " +
                "Values (@IdRol, @CodOrgano, @CodOficina,@PrimerNombre,@SegundoNombre,@PrimerApellido,@SegundoApellido,@Sexo,@Nidentificacion,@Ncarnet,@UsuarioNombre,@Contrasenna,@Email,@Activo,@UsuarioReg)";
            return await db.ExecuteAsync(sql, new { us.IdRol, us.CodOrgano, us.CodOficina, us.PrimerNombre, us.SegundoNombre, us.PrimerApellido, us.SegundoApellido, us.Sexo, us.Nidentificacion, us.Ncarnet, us.UsuarioNombre, us.Contrasenna, us.Email, us.Activo, us.UsuarioReg });
        }

        public async Task<int> UpdateUsuario(Usuario us)
        {
            var db = dbconnection();
            var sql = "Update\tUSUARIO\r\n\t\t\tSet\tIdRol = @IdRol,\r\n\t\t\t\tCodOrgano = @CodOrgano,\r\n\t\t\t\tCodOficina = @CodOficina,\r\n\t\t\t\tPrimerNombre = @PrimerNombre,\r\n\t\t\t\tSegundoNombre = @SegundoNombre,\r\n\t\t\t\tPrimerApellido = @PrimerApellido,\r\n\t\t\t\tSegundoApellido = @SegundoApellido,\r\n\t\t\t\tSexo = @Sexo,\r\n\t\t\t\tNidentificacion = @Nidentificacion,\r\n\t\t\t\tNcarnet = @Ncarnet,\r\n\t\t\t\tEmail = @Email,\r\n\t\t\t\tActivo = @Activo,\r\nContrasenna = @Contrasenna\r\n\t\tWhere\tIdUsuario = @IdUsuario";
            var result = await db.ExecuteAsync(sql, new { us.IdUsuario, us.IdRol, us.CodOrgano, us.CodOficina, us.PrimerNombre, us.SegundoNombre, us.PrimerApellido, us.SegundoApellido, us.Sexo, us.Nidentificacion, us.Ncarnet, us.UsuarioNombre, us.Contrasenna, us.Email, us.Activo, us.UsuarioReg });
            return result;
        }
    }
}
