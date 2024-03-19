using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaUsuarios.Domain.Models
{
    public class ResultDb
    {
        public int? RowId { get; set; }
        public int? Rowcount { get; set; }
        public string? Resultado { get; set; }
        public string? MessageError { get; set; }
        public bool Ok { get; set; }
        public ResultDb() { }
    }
}
