using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tabletop.ViewModels
{
    public class UniqueNamesViewModel
    {
        [MaxLength(80)]
        public string Name { get; set; }

        public IEnumerable<string> List { get; set; }
    }
}
