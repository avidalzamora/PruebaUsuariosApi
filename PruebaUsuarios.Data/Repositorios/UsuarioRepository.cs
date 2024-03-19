using Dapper;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using PruebaUsuarios.Domain.Models;
using PruebaUsuarios.Domain.Models.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public IEnumerable<T> ConvertToEnumerable<T>(MySqlDataReader dr) where T : class, new()
        {
            List<string> lstColumns = Enumerable.Range(0, dr.FieldCount).Select(dr.GetName).ToList();
            List<PropertyInfo> lstProperties = typeof(T).GetProperties().Where(x => lstColumns.Contains(x.Name, StringComparer.OrdinalIgnoreCase)).ToList();
            while (dr.Read())
            {
                var entity = new T();
                lstProperties.Where(w => dr[w.Name] != System.DBNull.Value).ToList().ForEach(i => i.SetValue(entity, dr[i.Name], null));
                yield return entity;
            }
        }

        

        public async Task<ResultDb> DeleteUsuario(Usuario us)
        {
            //var db = dbconnection();
            //var sql = "Update\tUSUARIO\r\n\t\t\tSet\tEliminado = 1\r\n\t\tWhere\tIdUsuario = @IdUsuario";
            //var result = await db.ExecuteAsync(sql, new { IdUsuario = us.IdUsuario });
            
            MySqlCommand command = new MySqlCommand();
            command.Connection = dbconnection();
            command.CommandText = "proyectos.UsuarioDelete";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("pIdUsuario", us.IdUsuario);
            command.Parameters.AddWithValue("pEliminar", 1);
            command.Parameters.AddWithValue("pUserReg", "");
            command.Parameters["pIdUsuario"].Direction = System.Data.ParameterDirection.Input;
            command.Parameters["pEliminar"].Direction = System.Data.ParameterDirection.Input;
            command.Parameters["pUserReg"].Direction = System.Data.ParameterDirection.Input;
            command.Connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            var result = ConvertToEnumerable<ResultDb>(reader).FirstOrDefault();
            result.Ok = result.MessageError == null || result.MessageError.Length == 0 ? true : false;
            command.Connection.Close();
            return await Task.FromResult<ResultDb>(result);
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

        public async Task<ResultDb> InsertarUsuario(Usuario us)
        {
            //var db = dbconnection();
            //var sql = "Insert Into USUARIO (IdRol, CodOrgano, CodOficina,PrimerNombre,SegundoNombre,PrimerApellido,SegundoApellido,Sexo,Nidentificacion,Ncarnet,UsuarioNombre,Contrasenna,Email,Activo,UsuarioReg) " +
            //    "Values (@IdRol, @CodOrgano, @CodOficina,@PrimerNombre,@SegundoNombre,@PrimerApellido,@SegundoApellido,@Sexo,@Nidentificacion,@Ncarnet,@UsuarioNombre,@Contrasenna,@Email,@Activo,@UsuarioReg)";
            //return await db.ExecuteAsync(sql, new { us.IdRol, us.CodOrgano, us.CodOficina, us.PrimerNombre, us.SegundoNombre, us.PrimerApellido, us.SegundoApellido, us.Sexo, us.Nidentificacion, us.Ncarnet, us.UsuarioNombre, us.Contrasenna, us.Email, us.Activo, us.UsuarioReg });
            MySqlCommand command = new MySqlCommand();
            command.Connection = dbconnection();
            command.CommandText = "proyectos.UsuarioSave";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("pIdUsuario", 0);
            command.Parameters.AddWithValue("pIdRol", us.IdRol);
            command.Parameters.AddWithValue("pCodOrgano", us.CodOrgano);
            command.Parameters.AddWithValue("pCodOficina", us.CodOficina);
            command.Parameters.AddWithValue("pPrimerNombre", us.PrimerNombre);
            command.Parameters.AddWithValue("pSegundoNombre", us.SegundoNombre);
            command.Parameters.AddWithValue("pPrimerApellido", us.PrimerApellido);
            command.Parameters.AddWithValue("pSegundoApellido", us.SegundoApellido);
            command.Parameters.AddWithValue("pSexo", us.Sexo);
            command.Parameters.AddWithValue("pNidentificacion", us.Nidentificacion);
            command.Parameters.AddWithValue("pNcarnet", us.Ncarnet);
            command.Parameters.AddWithValue("pUsuarioNombre", us.UsuarioNombre);
            command.Parameters.AddWithValue("pContrasenna", us.Contrasenna);
            command.Parameters.AddWithValue("pEmail", us.Email);
            command.Parameters.AddWithValue("pActivo", us.Activo);
            command.Parameters.AddWithValue("pUsuarioReg", us.UsuarioReg);
            command.Connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            var result = ConvertToEnumerable<ResultDb>(reader).FirstOrDefault();
            result.Ok = result.MessageError == null || result.MessageError.Length == 0 ? true : false;
            command.Connection.Close();
            return await Task.FromResult<ResultDb>(result);
        }

        public async Task<ResultDb> UpdateUsuario(Usuario us)
        {
            //var db = dbconnection();
            //var sql = "Update\tUSUARIO\r\n\t\t\tSet\tIdRol = @IdRol,\r\n\t\t\t\tCodOrgano = @CodOrgano,\r\n\t\t\t\tCodOficina = @CodOficina,\r\n\t\t\t\tPrimerNombre = @PrimerNombre,\r\n\t\t\t\tSegundoNombre = @SegundoNombre,\r\n\t\t\t\tPrimerApellido = @PrimerApellido,\r\n\t\t\t\tSegundoApellido = @SegundoApellido,\r\n\t\t\t\tSexo = @Sexo,\r\n\t\t\t\tNidentificacion = @Nidentificacion,\r\n\t\t\t\tNcarnet = @Ncarnet,\r\n\t\t\t\tEmail = @Email,\r\n\t\t\t\tActivo = @Activo,\r\nContrasenna = @Contrasenna\r\n\t\tWhere\tIdUsuario = @IdUsuario";
            //var result = await db.ExecuteAsync(sql, new { us.IdUsuario, us.IdRol, us.CodOrgano, us.CodOficina, us.PrimerNombre, us.SegundoNombre, us.PrimerApellido, us.SegundoApellido, us.Sexo, us.Nidentificacion, us.Ncarnet, us.UsuarioNombre, us.Contrasenna, us.Email, us.Activo, us.UsuarioReg });
            //return result;

            MySqlCommand command = new MySqlCommand();
            command.Connection = dbconnection();
            command.CommandText = "proyectos.UsuarioSave";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("pIdUsuario", us.IdUsuario);
            command.Parameters.AddWithValue("pIdRol", us.IdRol);
            command.Parameters.AddWithValue("pCodOrgano", us.CodOrgano);
            command.Parameters.AddWithValue("pCodOficina", us.CodOficina);
            command.Parameters.AddWithValue("pPrimerNombre", us.PrimerNombre);
            command.Parameters.AddWithValue("pSegundoNombre", us.SegundoNombre);
            command.Parameters.AddWithValue("pPrimerApellido", us.PrimerApellido);
            command.Parameters.AddWithValue("pSegundoApellido", us.SegundoApellido);
            command.Parameters.AddWithValue("pSexo", us.Sexo);
            command.Parameters.AddWithValue("pNidentificacion", us.Nidentificacion);
            command.Parameters.AddWithValue("pNcarnet", us.Ncarnet);
            command.Parameters.AddWithValue("pUsuarioNombre", us.UsuarioNombre);
            command.Parameters.AddWithValue("pContrasenna", us.Contrasenna);
            command.Parameters.AddWithValue("pEmail", us.Email);
            command.Parameters.AddWithValue("pActivo", us.Activo);
            command.Parameters.AddWithValue("pUsuarioReg", us.UsuarioReg);
            command.Connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            var result = ConvertToEnumerable<ResultDb>(reader).FirstOrDefault();
            result.Ok = result.MessageError == null || result.MessageError.Length == 0 ? true : false;
            command.Connection.Close();
            return await Task.FromResult<ResultDb>(result);
        }
    }
}
