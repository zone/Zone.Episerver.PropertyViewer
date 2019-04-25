using System.Web.Mvc;
using System.Web.Routing;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace Zone.Episerver.PropertyViewer.Initialization
{
    [InitializableModule]
    public class CustomRouteInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            RouteTable.Routes.MapRoute(
                null,
                "plugins/propertyviewer/{action}",
                new { controller = "PropertyViewer", action = "Index" });
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}
