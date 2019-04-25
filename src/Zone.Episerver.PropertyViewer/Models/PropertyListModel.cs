using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Zone.Episerver.PropertyViewer.Models
{
    public class PropertyListModel
    {
        [Display(Name = "Property Name")]
        public string PropertyName { get; set; }

        public IEnumerable<string> PageProperties { get; set; }
    }
}
