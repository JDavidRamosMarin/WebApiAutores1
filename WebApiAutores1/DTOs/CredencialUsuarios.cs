using System.ComponentModel.DataAnnotations;

namespace WebApiAutores1.DTOs
{
    public class CredencialUsuarios
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
