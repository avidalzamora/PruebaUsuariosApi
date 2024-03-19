using PruebaUsuarios.Domain.Models.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaUsuarios.Domain.Models.Usuario
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public string CodOrgano { get; set; }
        public string CodOficina { get; set; }
        public string? NombreCompleto { get; set; }
        public string PrimerNombre { get; set; }
        public string? SegundoNombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string Sexo { get; set; }
        public string? Nidentificacion { get; set; }
        public string? Ncarnet { get; set; }
        public string? UsuarioNombre { get; set; }
        public string? Contrasenna { get; set; }
        public string? Email { get; set; }
        public bool Activo { get; set; }
        public string? NombreRol { get; set; }
        public string? Organo { get; set; }
        public string? Oficina { get; set; }
        public string? UsuarioReg { get; set; }

        public Usuario() { }
    }
}
