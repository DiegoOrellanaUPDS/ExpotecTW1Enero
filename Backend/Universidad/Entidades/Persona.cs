using System;

namespace Entidades
{
    public class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public int CI { get; set; }
        public bool Estado { get; set; } = true;
    }
}