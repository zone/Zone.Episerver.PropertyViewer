using System.Collections.Generic;
using Zone.Episerver.PropertyViewer.Models;

namespace Zone.Episerver.PropertyViewer.Core.Services
{
    public interface IPropertyService
    {
        IReadOnlyList<string> GetPropertyNames(int pageId);

        IReadOnlyList<string> GetBlockPropertyNames(PropertyReference reference);

        IReadOnlyList<PropertyValue> GetPropertyValues(PropertyReference reference);

        IReadOnlyList<PropertyValue> GetBlockPropertyValues(LocalBlockPropertyReference reference);

        bool IsBlock(PropertyReference reference);
    }
}