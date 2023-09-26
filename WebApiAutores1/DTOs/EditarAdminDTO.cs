using System.ComponentModel.DataAnnotations;

namespace WebApiAutores1.DTOs
{
    public class EditarAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
