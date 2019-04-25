using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Zone.Episerver.PropertyViewer.Models
{
    public class BlockPropertyListModel
    {
        [Display(Name = "Block Property Name")]
        public string BlockPropertyName { get; set; }

        public IEnumerable<string> BlockProperties { get; set; }
    }
}
