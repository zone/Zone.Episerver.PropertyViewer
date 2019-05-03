using System.Linq;
using EPiServer;
using EPiServer.Core;
using Zone.Episerver.PropertyViewer.Models;

namespace Zone.Episerver.PropertyViewer.Core.Services
{
    public class ContentTreeService : IContentTreeService
    {        
        private readonly IContentLoader _contentLoader;

        public ContentTreeService(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public ContentTreeRootItem GetContentFamily(int rootId)
        {
            var page = _contentLoader.Get<PageData>(new ContentReference(rootId));
            var children = _contentLoader
                .GetChildren<PageData>(page.ContentLink)
                .Select(s => new ContentTreeChildItem
                {
                    Id = s.ContentLink.ToReferenceWithoutVersion().ID,
                    Text = s.Name,
                    Children = _contentLoader.GetChildren<PageData>(s.ContentLink).Any()
                });

            return new ContentTreeRootItem
            {
                Id = page.ContentLink.ToReferenceWithoutVersion().ID,
                Text = page.Name,
                Children = children
            };    
        }
    }
}