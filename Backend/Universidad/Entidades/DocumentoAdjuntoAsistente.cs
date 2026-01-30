namespace Universidad.Entidades
{
    public class DocumentoAdjuntoAsistente
    {
        public int Id { get; set; }
        public string NombreArchivo { get; set; }
        public string RutaArchivo { get; set; }
        public string TipoArchivo { get; set; } // "PDF", "Imagen", "Word"
        public long TamanioBytes { get; set; }
        public DateTime FechaSubida { get; set; }
        public int SolicitudId { get; set; } // Solo ID
    }
}