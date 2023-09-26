using System.ComponentModel.DataAnnotations;
using WebApiAutores1.Validaciones;

namespace WebApiAutores1.DTOs
{
    public class LibroDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        //public List<ComentarioDTO> Comentarios { get; set; }
    }
}
