using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.PlugIn;
using EPiServer.Shell;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Zone.Episerver.PropertyViewer.Core.Services;
using Zone.Episerver.PropertyViewer.Models;
using PlugInArea = EPiServer.PlugIn.PlugInArea;

namespace Zone.Episerver.PropertyViewer.Controllers
{
    [GuiPlugIn(
        DisplayName = "Property Viewer",
        Area = PlugInArea.AdminMenu,
        Url = "~/plugins/propertyviewer")]
    [Authorize(Roles = "Administrators,WebAdmins")]
    public class PropertyViewerController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IContentTreeService _contentTreeService;

        public PropertyViewerController(
            IPropertyService propertyService,
            IContentTreeService contentTreeService)
        {
            _propertyService = propertyService;
            _contentTreeService = contentTreeService;
        }

        public ViewResult Index()
        {
            return View(GetPath("Index"), new PropertyViewerModel());
        }

        public ContentResult GetContentTree(int id = 1)
        {
            var tree = _contentTreeService.GetContentFamily(id);
            return CamelCaseJson(tree);
        }

        public PartialViewResult GetProperties(int pageId)
        {
            var model = new PropertyListModel
            {
                PageProperties = _propertyService.GetPropertyNames(pageId)
            };

            return PartialView(GetPath("_PropertyList"), model);
        }

        public PartialViewResult GetPropertyValues(PropertyReference reference)
        {
            if (_propertyService.IsBlock(reference))
            {
                var blockModel =  new BlockPropertyListModel
                {
                    BlockProperties = _propertyService.GetBlockPropertyNames(reference)
                };

                return PartialView(GetPath("_BlockPropertyList"), blockModel);
            }

            var model = new PropertyValuesModel
            {
                PropertyValues = _propertyService.GetPropertyValues(reference)
            };

            return PartialView(GetPath("_PropertyValues"), model);
        }

        public PartialViewResult GetBlockPropertyValues(LocalBlockPropertyReference propertyReference)
        {
            var model = new PropertyValuesModel
            {
                PropertyValues = _propertyService.GetBlockPropertyValues(propertyReference)   
            };

            return PartialView(GetPath("_PropertyValues"), model);
        }

        private static string GetPath(string viewName)
        {
            return Paths.ToResource("Zone.Episerver.PropertyViewer", $"Views/PropertyViewer/{viewName}.cshtml");
        }

        private ContentResult CamelCaseJson(object data)
        {
            var camelCaseFormatter = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var json = JsonConvert.SerializeObject(data, camelCaseFormatter);

            return Content(json, "application/json");
        }
    }
}
