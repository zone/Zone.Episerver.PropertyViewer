using System.Collections.Generic;
using EPiServer.Core;
using Zone.Episerver.PropertyViewer.Models;

namespace Zone.Episerver.PropertyViewer.Core.Services
{
    public interface IPropertyService
    {
        IEnumerable<string> GetPropertyNames(int pageId);

        IEnumerable<string> GetBlockPropertyNames(PropertyReference reference);

        IEnumerable<PropertyValue> GetPropertyValues(PropertyReference reference);

        IEnumerable<PropertyValue> GetBlockPropertyValues(LocalBlockPropertyReference reference);

        bool IsBlock(PropertyReference reference);
    }
}