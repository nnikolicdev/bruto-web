using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bruto_web.Models
{
    public class CombinedModel
    {
        public UserModel user { get; set; }
        public BrutoModel model { get; set; }
        public BrutoViewModel viewModel { get; set; }
    }
}
