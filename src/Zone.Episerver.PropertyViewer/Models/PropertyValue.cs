using EPiServer.Core;

namespace Zone.Episerver.PropertyViewer.Models
{
    public class PropertyValue
    {
        public string Language { get; set; }

        public ContentReference ContentLink { get; set; }

        public PropertyData Property { get; set; }
    }
}
