using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Universidad.Entidades
{
    public class UsuarioCIIT
    {
        [Key]
        public int id{get;set;}
        public string nombre {get;set;}
        public string token{get;set;}
        public string rol {get;set;}
        public DateTime expiracion{get;set;}
    }
}