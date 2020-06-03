using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bruto_web.Models
{
    public class UserModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }

        [Required, StringLength(18, MinimumLength = 5)]
        public string Password { get; set; }


        public ICollection<BrutoModel> models { get; set; }

    }
}
