using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaUsuarios.Data.Repositorios;
using PruebaUsuarios.Domain.Models.Usuario;

namespace PruebaUsuarios.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuario usuariorepository;
        private readonly ICorreo correorepo;

        public UsuariosController(IUsuario usuario, ICorreo correo)
        {
            usuariorepository = usuario;
            correorepo = correo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsuarios()
        {
            return Ok(await usuariorepository.GetUsuarios());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuarioDetail(int id)
        {
            return Ok(await usuariorepository.GetDetails(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUsuario([FromBody] Usuario usuario)
        {
            if(usuario == null)
                return BadRequest();
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await usuariorepository.InsertarUsuario(usuario);
            if (created.Ok)
            {
                var enviado = correorepo.EnviarCorreo("Usuario Creado", "Se creo el usuario: " + usuario.PrimerNombre, "correoprueba@gmail.com", "");
                if (!enviado)
                    created.Resultado = created.Resultado + "; No se envio el correo de confirmación";
            }
            return Created("Creado", created);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUsuario([FromBody] Usuario usuario)
        {
            if (usuario == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await usuariorepository.UpdateUsuario(usuario);
            //return Ok(await usuariorepository.UpdateUsuario(usuario));
            return Created("Actualizado", created);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            //await usuariorepository.DeleteUsuario(new Usuario { IdUsuario = id});
            return Ok(await usuariorepository.DeleteUsuario(new Usuario { IdUsuario = id }));
            //return NoContent();
        }

    }
}
