using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Solicitud
    {
        public int Id { get; set; }
        public string CodigoSolicitud { get; set; }
        public string Tipo { get; set; }   
        public DateTime Fecha { get; set; }
    }
}
