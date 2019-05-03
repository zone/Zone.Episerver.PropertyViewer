using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Zone.Episerver.PropertyViewer.Core.Services;

namespace Zone.Episerver.PropertyViewer.Core.Initialization
{
    [InitializableModule]
    public class DependencyInitialization : IConfigurableModule 
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<IPropertyService, PropertyService>();
        }

        public void Initialize(InitializationEngine context)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}
