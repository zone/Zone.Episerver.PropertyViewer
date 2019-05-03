using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.PlugIn;
using EPiServer.Shell;
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
        private readonly IContentLoader _contentLoader;

        public PropertyViewerController(
            IPropertyService propertyService,
            IContentLoader contentLoader)
        {
            _propertyService = propertyService;
            _contentLoader = contentLoader;
        }

        public ViewResult Index()
        {
            return View(GetPath("Index"), new PropertyViewerModel());
        }

        public JsonResult GetContentTree(int id = 1)
        {
            _contentLoader.TryGet(new ContentReference(id), out PageData page);

            return Json(new
            {
                id,
                text = page.Name,
                children = _contentLoader.GetChildren<PageData>(page.ContentLink)?.Select(s => new
                {
                    text = s.Name,
                    id = s.ContentLink.ToReferenceWithoutVersion().ID,
                    children = _contentLoader.GetChildren<PageData>(s.ContentLink)?.Any()
                })
            }, JsonRequestBehavior.AllowGet);
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
    }
}
