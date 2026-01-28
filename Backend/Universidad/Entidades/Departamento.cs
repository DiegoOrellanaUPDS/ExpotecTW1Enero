using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entidades// Asegurate que el namespace sea correcto
{
    
    public class Departamento
    {
        
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty; 
        public string? Descripcion { get; set; } 
        
        public string Encargado { get; set; } = string.Empty;

        public bool Estado { get; set; } = true; 
    }
}