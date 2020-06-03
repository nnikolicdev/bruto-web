using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bruto_web.Models
{
    public class BrutoModel
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        public int UserId { get; set; }
        public UserModel User { get; set; }

        [Required]
        public int Neto { get; set; }

        [Required]
        public int Bruto { get; set; }
        public DateTime Date { get; set; }
    }
}
