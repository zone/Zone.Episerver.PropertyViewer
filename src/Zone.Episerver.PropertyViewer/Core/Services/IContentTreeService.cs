using Zone.Episerver.PropertyViewer.Models;

namespace Zone.Episerver.PropertyViewer.Core.Services
{
    public interface IContentTreeService
    {
        ContentTreeRootItem GetContentFamily(int rootId);
    }
}