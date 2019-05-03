using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.SpecializedProperties;
using NSubstitute;
using NUnit.Framework;  
using Zone.Episerver.PropertyViewer.Core.Services;
using Zone.Episerver.PropertyViewer.Models;

namespace Zone.Episerver.PropertyViewer.Tests
{
    [TestFixture]
    public class PropertyServiceTests
    {
        private PropertyService _propertyService;
        private IContentLoader _stubContentLoader;
        private IContentRepository _stubContentRepository;

        [SetUp]
        public void SetUp()
        {
            _stubContentLoader = Substitute.For<IContentLoader>();
            _stubContentRepository = Substitute.For<IContentRepository>();
            _propertyService = new PropertyService(_stubContentLoader, _stubContentRepository);
        }

        [Test]
        public void GetPropertyNames_ValidPageId_ReturnsPropertyNames()
        {
            // Arrange
            const int id = 1;
            var page = new PageData
            {
                Property =
                {
                    new PropertyString {Name = "A property", IsPropertyData = false},
                    new PropertyString {Name = "Another property", IsPropertyData = true},
                    new PropertyString {Name = "Last property", IsPropertyData = true}
                }
            };
            _stubContentLoader.Get<PageData>(new ContentReference(id)).Returns(page);

            // Act
            var result = _propertyService.GetPropertyNames(id);

            // Assert
            CollectionAssert.AreEqual(page.Property.Where(x => x.IsPropertyData).Select(x => x.Name), result);
        }
        
        [Test]
        public void GetBlockPropertyNames_InvalidBlockType_ThrowsException()
        {
            // Arrange
            const int id = 1;
            const string propertyName = "name";
            var page = new PageData {Property = {new PropertyString {Name = propertyName}}};
            _stubContentLoader.Get<PageData>(new ContentReference(id)).Returns(page);

            // Act
            // Assert
            Assert.Throws<Exception>(() =>
                _propertyService.GetBlockPropertyNames(new PropertyReference(id, propertyName)));
        }
             
        [Test]
        public void GetBlockPropertyNames_ValidBlockType_ReturnsPropertyNames()
        {
            // Arrange
            const int id = 1;
            const string propertyName = "name";
            var block = new BlockData
            {
                Property =
                {
                    new PropertyString {Name = "A property", IsPropertyData = false},
                    new PropertyString {Name = "Another property", IsPropertyData = true},
                    new PropertyString {Name = "Last property", IsPropertyData = true}
                }
            };
            var property = new PropertyBlock<BlockData>(block) {Name = propertyName};
            var page = new PageData {Property = {property}};
            _stubContentLoader.Get<PageData>(new ContentReference(id)).Returns(page);

            // Act
            var result = _propertyService.GetBlockPropertyNames(new PropertyReference(id, propertyName));

            // Assert
            CollectionAssert.AreEqual(block.Property.Where(x => x.IsPropertyData).Select(x => x.Name), result);
        }

        [Test]
        public void GetPropertyValues_ValidReference_ReturnsPropertyValues()
        {
            // Arrange
            const int id = 1;
            const string propertyName = "name";
            const string propertyValue = "value";
            const string language = "en";
            var contentReference = new ContentReference(id);
            var page = new PageData
            {
                Property =
                {
                    new PropertyContentReference {Name = "PageLink", Value = contentReference},
                    new PropertyString {Name = "PageLanguageBranch", Value = language},
                    new PropertyString {Name = propertyName, Value = propertyValue}
                }
            };
            _stubContentRepository.GetLanguageBranches<PageData>(Arg.Is(contentReference))
                .Returns(new List<PageData> {page});

            // Act
            var result = _propertyService.GetPropertyValues(new PropertyReference(id, propertyName)).ToList();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(language, result[0].Language);
            Assert.AreEqual(propertyValue, result[0].Value);
            Assert.AreEqual(contentReference, result[0].ContentLink);
        }

        [Test]
        public void GetBlockPropertyValues_ValidReference_ReturnsPropertyValues()
        {
            // Arrange
            const int id = 1;
            const string propertyName = "name";
            const string propertyValue = "value";
            const string blockPropertyName = "block";
            const string language = "en";
            var contentReference = new ContentReference(id);
            var block = new BlockData
            {
                Property =
                {
                    new PropertyString {Name = blockPropertyName, Value = propertyValue}
                }
            };
            var page = new PageData
            {
                Property =
                {
                    new PropertyContentReference {Name = "PageLink", Value = contentReference},
                    new PropertyString {Name = "PageLanguageBranch", Value = language},
                    new PropertyBlock<BlockData> {Name = propertyName, Value = block}
                }
            };
            _stubContentRepository.GetLanguageBranches<PageData>(Arg.Is(contentReference))
                .Returns(new List<PageData> {page});

            // Act
            var result = _propertyService.GetBlockPropertyValues(new LocalBlockPropertyReference(id, propertyName, blockPropertyName)).ToList();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(language, result[0].Language);
            Assert.AreEqual(propertyValue, result[0].Value);
            Assert.AreEqual(contentReference, result[0].ContentLink);
        }

        [Test]
        public void IsBlock_BlockReference_ReturnsTrue()
        {
            // Arrange
            const int id = 1;
            const string propertyName = "name";
            var page = new PageData {Property = {new PropertyBlock<BlockData> {Name = propertyName}}};
            _stubContentLoader.Get<PageData>(new ContentReference(id)).Returns(page);

            // Act
            var result = _propertyService.IsBlock(new PropertyReference(id, propertyName));

            // Assert
            Assert.IsTrue(result);
        }
        
        [Test]
        public void IsBlock_NonBlockReference_ReturnsFalse()
        {
            // Arrange
            const int id = 1;
            const string propertyName = "name";
            var page = new PageData {Property = {new PropertyString {Name = propertyName}}};
            _stubContentLoader.Get<PageData>(new ContentReference(id)).Returns(page);

            // Act
            var result = _propertyService.IsBlock(new PropertyReference(id, propertyName));

            // Assert
            Assert.IsFalse(result);
        }
    }
}