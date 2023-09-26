using System.ComponentModel.DataAnnotations;
using WebApiAutores1.Validaciones;

namespace WebApiAutores1.DTOs
{
    public class LibroCreacionDTO
    {
        [PrimerLetraMayuscula]
        [StringLength(maximumLength: 250)]
        [Required]
        public string Titulo { get; set; }
        public DateTime fechaPublicacion { get; set; }
        public List<int> AutoresIds { get; set; }
    }
}
