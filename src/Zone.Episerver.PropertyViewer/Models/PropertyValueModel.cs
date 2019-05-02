using EPiServer.Core;

namespace Zone.Episerver.PropertyViewer.Models
{
    public class PropertyValueModel
    {
        public string Language { get; set; }

        public ContentReference ContentLink { get; set; }

        public string Value { get; set; }
    }
}
