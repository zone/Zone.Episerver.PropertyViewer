namespace Zone.Episerver.PropertyViewer.Models
{
    public class LocalBlockPropertyReference : PropertyReference
    {
        public LocalBlockPropertyReference() : base()
        {
        }

        public LocalBlockPropertyReference(
            int pageId, 
            string propertyName,          
            string blockPropertyName) 
            : base(pageId, propertyName)
        {
            BlockPropertyName = blockPropertyName;
        }

        public string BlockPropertyName { get; set; }
    }
}
