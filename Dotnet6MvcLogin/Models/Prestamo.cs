using SistemaBiblioteca.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace SistemaBiblioteca.Models
{
    public class Prestamo
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int LibroId { get; set; }
        public int TiempoPrestamo { get; set; }
        public string? Observaciones { get; set; }

        public DateTime? FechaPrestamo { get; set; }
        public Libro? Libro{ get; set; }
        public ApplicationUser? User{ get; set; }
    }
}
