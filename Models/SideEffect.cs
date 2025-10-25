using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace G3MWL.Models
{
    public class SideEffect
    {
        [BindNever]
        [ValidateNever] //  This stops validation from running on _id
        public string _id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Severity is required")]
        public string Severity { get; set; } = "mild";
    }
}

