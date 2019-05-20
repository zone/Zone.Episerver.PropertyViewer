using System.Collections.Generic;
using EPiServer;
using NSubstitute;
using NUnit.Framework;
using EPiServer.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Zone.Episerver.PropertyViewer.Controllers;
using Zone.Episerver.PropertyViewer.Core.Services;
using Zone.Episerver.PropertyViewer.Models;

namespace Zone.Episerver.PropertyViewer.Tests.Controllers
{
    [TestFixture]
    public class PropertyViewerControllerTests
    {
        private PropertyViewerController _propertyViewerController;
        private IPropertyService _stubPropertyService;
        private IContentTreeService _stubContentTreeService;
        private IContentLoader _stubContentLoader;

        [SetUp]
        public void SetUp()
        {
            _stubPropertyService = Substitute.For<IPropertyService>();
            _stubContentTreeService = Substitute.For<IContentTreeService>();
            _stubContentLoader = Substitute.For<IContentLoader>();
            _propertyViewerController = new PropertyViewerController(_stubPropertyService, _stubContentTreeService, _stubContentLoader);
        }

        [Test]
        public void Index_Request_ReturnsModel()
        {
            // Arrange

            // Act
            var result = _propertyViewerController.Index();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<PropertyViewerModel>(result.Model);
        }

        [Test]
        public void GetContentTree_ValidPageId_ReturnsTree()
        {            
            // Arrange
            const int pageId = 123;
            var root = new ContentTreeRootItem();
            _stubContentTreeService.GetContentFamily(pageId).Returns(root);

            // Act
            var result = _propertyViewerController.GetContentTree(pageId);

            // Assert
            Assert.AreEqual(SerializeToJson(root), result.Content);
        }

        [Test]
        public void GetProperties_ValidPageId_ReturnsPropertyListView()
        {            
            // Arrange
            const int pageId = 123;
            _stubPropertyService.GetPropertyNames(pageId).Returns(new string[0]);

            // Act
            var result = _propertyViewerController.GetProperties(pageId);

            // Assert
            Assert.AreEqual("_PropertyList", result.ViewName);
        }

        [Test]
        public void GetProperties_ValidPageId_ReturnsPropertyListModel()
        {            
            // Arrange
            const int pageId = 123;
            var propertyNames = new [] {"a property"};
            _stubPropertyService.GetPropertyNames(pageId).Returns(propertyNames);

            // Act
            var result = _propertyViewerController.GetProperties(pageId);
            var model = result.Model as PropertyListModel;

            // Assert
            Assert.IsNotNull(model);
            Assert.AreEqual(propertyNames, model.PageProperties);
        }

        [Test]
        public void GetPropertyValues_ValidPropertyReference_ReturnsPropertyValuesView()
        {
            // Arrange
            var propertyReference = new PropertyReference(1, "name");
            _stubPropertyService.GetPropertyValues(propertyReference).Returns(new List<PropertyValue>());

            // Act
            var result = _propertyViewerController.GetPropertyValues(propertyReference);

            // Assert
            Assert.AreEqual("_PropertyValues", result.ViewName);
        }

        [Test]
        public void GetPropertyValues_ValidPropertyReference_ReturnsPropertyValuesModel()
        {
            // Arrange
            var propertyReference = new PropertyReference(1, "name");
            var propertyValues = new List<PropertyValue> {new PropertyValue()};
            _stubPropertyService.GetPropertyValues(propertyReference).Returns(propertyValues);

            // Act
            var result = _propertyViewerController.GetPropertyValues(propertyReference);
            var model = result.Model as PropertyValuesModel;

            // Assert
            Assert.IsNotNull(model);
            Assert.AreEqual(propertyValues, model.PropertyValues);
        }

        [Test]
        public void GetPropertyValues_ValidBlockPropertyReference_ReturnsBlockPropertyListView()
        {
            // Arrange
            var propertyReference = new PropertyReference(1, "name");
            _stubPropertyService.IsBlock(propertyReference).Returns(true);
            _stubPropertyService.GetPropertyValues(propertyReference).Returns(new List<PropertyValue>());

            // Act
            var result = _propertyViewerController.GetPropertyValues(propertyReference);

            // Assert
            Assert.AreEqual("_BlockPropertyList", result.ViewName);
        }

        [Test]
        public void GetPropertyValues_ValidBlockPropertyReference_ReturnsBlockPropertyValuesModel()
        {
            // Arrange
            var propertyReference = new PropertyReference(1, "name");
            var propertyNames = new [] {"a property"};
            _stubPropertyService.IsBlock(propertyReference).Returns(true);
            _stubPropertyService.GetBlockPropertyNames(propertyReference).Returns(propertyNames);

            // Act
            var result = _propertyViewerController.GetPropertyValues(propertyReference);
            var model = result.Model as BlockPropertyListModel;

            // Assert
            Assert.IsNotNull(model);
            Assert.AreEqual(propertyNames, model.BlockProperties);
        }

        [Test]
        public void GetBlockPropertyValues_ValidBlockPropertyReference_ReturnsPropertyValuesView()
        {
            // Arrange
            var propertyReference = new LocalBlockPropertyReference(1, "name", "block name");
            _stubPropertyService.GetBlockPropertyValues(propertyReference).Returns(new List<PropertyValue>());

            // Act
            var result = _propertyViewerController.GetBlockPropertyValues(propertyReference);

            // Assert
            Assert.AreEqual("_PropertyValues", result.ViewName);
        }

        [Test]
        public void GetBlockPropertyValues_ValidBlockPropertyReference_ReturnsPropertyValuesModel()
        {
            // Arrange
            var propertyReference = new LocalBlockPropertyReference(1, "name", "block name");
            var propertyValues = new List<PropertyValue> {new PropertyValue()};
            _stubPropertyService.GetBlockPropertyValues(propertyReference).Returns(propertyValues);

            // Act
            var result = _propertyViewerController.GetBlockPropertyValues(propertyReference);
            var model = result.Model as PropertyValuesModel;

            // Assert
            Assert.IsNotNull(model);
            Assert.AreEqual(propertyValues, model.PropertyValues);
        }

        [Test]
        public void RenderContentReference_ImageContentReference_ReturnsImageView()
        {
            // Arrange
            var contentReference = new ContentReference(123);
            _stubContentLoader.TryGet(contentReference, out ImageData _).Returns(true);

            // Act
            var result = _propertyViewerController.RenderContentReference(contentReference);

            // Assert
            Assert.AreEqual("_Image", result.ViewName);
        }

        [Test]
        public void RenderContentReference_DefaultContentReference_ReturnsDefaultView()
        {
            // Arrange
            var contentReference = new ContentReference(123);

            // Act
            var result = _propertyViewerController.RenderContentReference(contentReference);

            // Assert
            Assert.AreEqual("_ContentReference", result.ViewName);
        }

        private static string SerializeToJson(object model)
        {
            var camelCaseFormatter = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            
            return JsonConvert.SerializeObject(model, camelCaseFormatter);
        }
    }
}