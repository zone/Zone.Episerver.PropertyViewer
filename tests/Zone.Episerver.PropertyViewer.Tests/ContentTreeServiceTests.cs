using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using NSubstitute;
using NUnit.Framework;  
using Zone.Episerver.PropertyViewer.Core.Services;

namespace Zone.Episerver.PropertyViewer.Tests
{
    [TestFixture]
    public class ContentTreeServiceTests
    {
        private ContentTreeService _contentTreeService;
        private IContentLoader _stubContentLoader;

        [SetUp]
        public void SetUp()
        {
            _stubContentLoader = Substitute.For<IContentLoader>();
            _contentTreeService = new ContentTreeService(_stubContentLoader);
        }

        [Test]
        public void GetContentFamily_ValidRootId_ReturnsContentTreeRootItem()
        {
            // Arrange
            const int rootId = 1;
            const string rootName = "root";
            var rootReference = new ContentReference(rootId);
            const int childId1 = 2;
            const string childName1 = "child";
            var childReference1 = new ContentReference(childId1);
            const int childId2 = 3;
            const string childName2 = "child";
            var childReference2 = new ContentReference(childId2);
            var rootPage = new PageData
            {
                Property =
                {
                    new PropertyString {Name = "PageName", Value = rootName},
                    new PropertyContentReference {Name = "PageLink", Value = rootReference}
                }
            };
            var childPage1 = new PageData
            {
                Property =
                {
                    new PropertyString {Name = "PageName", Value = childName1},
                    new PropertyContentReference {Name = "PageLink", Value = childReference1}
                }
            };
            var childPage2 = new PageData
            {
                Property =
                {
                    new PropertyString {Name = "PageName", Value = childName2},
                    new PropertyContentReference {Name = "PageLink", Value = childReference2}
                }
            };
            _stubContentLoader.Get<PageData>(rootReference).Returns(rootPage);
            _stubContentLoader.GetChildren<PageData>(rootReference, Arg.Any<LanguageSelector>()).Returns(new List<PageData>{childPage1, childPage2});
            _stubContentLoader.GetChildren<PageData>(childReference1, Arg.Any<LanguageSelector>()).Returns(new List<PageData>{new PageData()});
            _stubContentLoader.GetChildren<PageData>(childReference2, Arg.Any<LanguageSelector>()).Returns(Enumerable.Empty<PageData>());

            // Act
            var result = _contentTreeService.GetContentFamily(rootId);

            // Assert
            Assert.AreEqual(rootId, result.Id);
            Assert.AreEqual(rootName, result.Text);
            Assert.AreEqual(2, result.Children.Count());
            Assert.AreEqual(childId1, result.Children.First().Id);
            Assert.AreEqual(childName1, result.Children.First().Text);
            Assert.IsTrue(result.Children.First().Children);
            Assert.AreEqual(childId2, result.Children.Last().Id);
            Assert.AreEqual(childName1, result.Children.Last().Text);
            Assert.IsFalse(result.Children.Last().Children);
        }
    }
}