using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaBiblioteca.Models.Helpers
{
    public class ModelLibroHelper
    {
        public int LibroId { get; set; }
        public List<SelectListItem> Libros { set; get; }
    }
}
