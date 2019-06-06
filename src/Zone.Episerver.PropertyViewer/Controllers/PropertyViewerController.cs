using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.PlugIn;
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
        UrlFromModuleFolder = "PropertyViewer")]
    [Authorize(Roles = "Administrators,WebAdmins")]
    public class PropertyViewerController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IContentTreeService _contentTreeService;
        private readonly IContentLoader _contentLoader;

        public PropertyViewerController(
            IPropertyService propertyService,
            IContentTreeService contentTreeService,
            IContentLoader contentLoader)
        {
            _propertyService = propertyService;
            _contentTreeService = contentTreeService;
            _contentLoader = contentLoader;
        }

        public ViewResult Index()
        {
            return View(new PropertyViewerModel());
        }

        public ContentResult GetContentTree(int pageId)
        {
            var tree = _contentTreeService.GetContentFamily(pageId);
            return CamelCaseJson(tree);
        }

        public PartialViewResult GetProperties(int pageId)
        {
            var model = new PropertyListModel
            {
                PageProperties = _propertyService.GetPropertyNames(pageId)
            };

            return PartialView("_PropertyList", model);
        }

        public PartialViewResult GetPropertyValues(PropertyReference reference)
        {
            if (_propertyService.IsBlock(reference))
            {
                var blockModel =  new BlockPropertyListModel
                {
                    BlockProperties = _propertyService.GetBlockPropertyNames(reference)
                };

                return PartialView("_BlockPropertyList", blockModel);
            }

            var model = new PropertyValuesModel
            {
                PropertyValues = _propertyService.GetPropertyValues(reference)
            };

            return PartialView("_PropertyValues", model);
        }

        public PartialViewResult GetBlockPropertyValues(LocalBlockPropertyReference propertyReference)
        {
            var model = new PropertyValuesModel
            {
                PropertyValues = _propertyService.GetBlockPropertyValues(propertyReference) 
            };

            return PartialView("_PropertyValues", model);
        }

        [ChildActionOnly]
        public PartialViewResult RenderContentReference(ContentReference contentReference, string language)
        {
            if (_contentLoader.TryGet(contentReference, out ImageData _))
            {
                return PartialView("_Image", contentReference);
            }

            var model = new ContentReferenceModel
            {
                ContentReference = contentReference,
                Language = language
            };

            return PartialView("_ContentReference", model);
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
