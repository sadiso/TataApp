using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TataApp.Domain
{
    public class DocumentType
    {
        [Key]
        public int DocumentTypeId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The maximo")]
        [Display(Name = "Descripción")]
        //[indexer (Document)]
        public string Descripción { get; set; }
    }
}
