using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.PlugIn;
using EPiServer.Shell;
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
        private readonly IContentLoader _contentLoader;
        private readonly IContentRepository _contentRepository;

        public PropertyViewerController(
            IContentLoader contentLoader,
            IContentRepository contentRepository)
        {
            _contentLoader = contentLoader;
            _contentRepository = contentRepository;
        }

        public ViewResult Index()
        {
            return View(Paths.ToResource("Zone.Episerver.PropertyViewer", "Views/PropertyViewer/Index.cshtml"), new PropertyViewerModel());
        }

        public JsonResult GetContentTree(int id = 1)
        {
            var page = GetPage(id);

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
            var page = GetPage(pageId);
            var model = new PropertyListModel
            {
                PageProperties = GetProperties(page)
            };

            return PartialView(Paths.ToResource("Zone.Episerver.PropertyViewer", "Views/PropertyViewer/_PropertyList.cshtml"), model);
        }

        public PartialViewResult GetPropertyValues(PropertyReference reference)
        {
            if (IsBlock(reference))
            {
                var blockModel = BuildBlockPropertyListModel(reference);
                return PartialView(Paths.ToResource("Zone.Episerver.PropertyViewer", "Views/PropertyViewer/_BlockPropertyList.cshtml"), blockModel);
            }

            var model = BuildPropertyValuesModel(reference);

            return PartialView(Paths.ToResource("Zone.Episerver.PropertyViewer", "Views/PropertyViewer/_PropertyValues.cshtml"), model);
        }

        public PartialViewResult GetBlockPropertyValues(int pageId, string propertyName, string blockPropertyName)
        {
            var languageVersions = _contentRepository.GetLanguageBranches<PageData>(new ContentReference(pageId));
            var propertyValues = languageVersions.Select(x => new PropertyValueModel
            {
                Language = x.Language.Name,
                Value = x.Property
                        .GetPropertyValue<BlockData>(propertyName)
                        .GetPropertyValue(blockPropertyName)
            });
            var model = new PropertyValuesModel
            {
                PropertyValues = propertyValues   
            };

            return PartialView(Paths.ToResource("Zone.Episerver.PropertyViewer", "Views/PropertyViewer/_PropertyValues.cshtml"), model);
        }

        private IEnumerable<string> GetProperties(IContentData content)
        {
            return content.Property
                .Where(x => x.IsPropertyData)
                .Select(x => x.Name)
                .OrderBy(x => x)
                .ToList();
        }

        private PageData GetPage(int pageId)
        {
            PageData page;
            _contentLoader.TryGet(new ContentReference(pageId), out page);

            return page;
        }

        private PropertyData GetProperty(PropertyReference reference)
        {
            var page = GetPage(reference.PageId);
            return page.Property.Get(reference.PropertyName);
        }

        private bool IsBlock(PropertyReference reference)
        {
            var property = GetProperty(reference);
            return property.Type == PropertyDataType.Block;
        }

        private PropertyValuesModel BuildPropertyValuesModel(PropertyReference reference)
        {
            var languageVersions = _contentRepository.GetLanguageBranches<PageData>(new ContentReference(reference.PageId));
            var propertyValues = languageVersions.Select(x => new PropertyValueModel
            {
                Language = x.Language.Name,
                ContentLink = x.ContentLink,
                Value = x.GetPropertyValue(reference.PropertyName)
            });

            return new PropertyValuesModel
            {
                PropertyValues = propertyValues
            };
        }

        private BlockPropertyListModel BuildBlockPropertyListModel(PropertyReference reference)
        {
            var property = GetProperty(reference);
            return new BlockPropertyListModel
            {
                BlockProperties = GetProperties((BlockData)property.Value)
            };
        }
    }
}
