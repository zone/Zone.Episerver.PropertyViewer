using System.Collections.Generic;

namespace Zone.Episerver.PropertyViewer.Models
{
    public class ContentTreeRootItem : ContentTreeItem
    {
        public IEnumerable<ContentTreeChildItem> Children { get; set; }
    }
}