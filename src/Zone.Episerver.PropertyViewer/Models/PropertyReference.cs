namespace Zone.Episerver.PropertyViewer.Models
{
    public class PropertyReference
    {
        public PropertyReference()
        {
        }

        public PropertyReference(int pageId, string propertyName) : this()
        {
            PageId = pageId;
            PropertyName = propertyName;
        }

        public int PageId { get; set; }
        public string PropertyName { get; set; }
    }
}
