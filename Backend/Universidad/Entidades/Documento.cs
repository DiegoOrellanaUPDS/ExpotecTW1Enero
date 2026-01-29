using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Universidad.Entidades
{
    public class Documento
    {
        [Key]
        public int id { get; set; }
        public string codigo { get; set; } = string.Empty; 
        public int nroVersion { get; set; }
        public int IdDocumento { get; set; }
        public string url_cloudinary { get; set; } 
        public string nombreArchivoOriginal { get; set; }
        public long tamanoarchivo { get; set; }
        public string estado { get; set; } 
        public DateOnly fechaPublicada { get; set; }
    }
}