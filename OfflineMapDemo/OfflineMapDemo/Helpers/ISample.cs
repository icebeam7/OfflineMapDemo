using Mapsui.UI;

namespace OfflineMapDemo.Helpers
{
    public interface ISample
    {
        string Name { get; }
        string Category { get; }
        void Setup(IMapControl mapControl);
    }
}
