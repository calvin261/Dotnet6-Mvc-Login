using System.ComponentModel.DataAnnotations;

namespace SistemaBiblioteca.Models
{
    public class Libro
    {
        [Key]
        public int Id { get; set; }
        public string Autor { get; set; }
        public string Titulo { get; set; }
        public string Materia { get; set; }
        public string Estado { get; set; }
        [Display(Name = "Tipo de Libro")]
        public string TipoLibro { get; set; }
        [Display(Name = "Categoría por edad")]
        public string CategoriaEdad { get; set; }
        [DataType(DataType.Date)]
        public DateTime? CreatedAt { get; set; }
        [DataType(DataType.Date)]
        public DateTime? UpdatedAt { get; set; }

    }
}
