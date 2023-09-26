using System.ComponentModel.DataAnnotations;
using WebApiAutores1.Validaciones;

namespace WebApiAutores1.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [PrimerLetraMayuscula]
        [StringLength(maximumLength: 250)]
        public string Titulo { get; set; }
        public DateTime? fechaPublicacion { get; set; }
        public List<Comentario> Comentarios { get; set; }
        public List<AutorLibro> AutoresLibros { get; set; }
    }
}
