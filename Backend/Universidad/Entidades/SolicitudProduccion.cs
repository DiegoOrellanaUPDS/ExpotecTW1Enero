namespace Universidad.Entidades
{
    public class SolicitudProduccion
    {
        public int id { get; set; }
        public string nombresolicitud { get; set; }
        public string tipocontenido { get; set; }
        public string plataforma { get; set; }
        public DateTime fechasolicitud { get; set; }
        public DateTime fechalimite { get; set; }
        public string prioridad { get; set; }
        public string estadopedido { get; set; }
    }
}
