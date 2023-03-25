namespace Core.Loyal.Models.FTMUSIC
{
    public class CancionModel
    {
        public long? Id { get; set; }
        public long? IdUsuario { get; set; }
        public long? IdAlbun { get; set; }
        public string? Nombre { get; set; }
        public string? Genero { get; set; }
        public string? Letra { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public long? NumeroMegusta { get; set; }
        public string? Link { get; set; }
        public string? Estado { get; set; }
    }
}