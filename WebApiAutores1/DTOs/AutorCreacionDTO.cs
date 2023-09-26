using System.ComponentModel.DataAnnotations;
using WebApiAutores1.Validaciones;

namespace WebApiAutores1.DTOs
{
    public class AutorCreacionDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")] // El dato es requerido
        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe tener mas de 5 caracteres")]
        [PrimerLetraMayuscula]
        public string Nombre { get; set; }
    }
}
