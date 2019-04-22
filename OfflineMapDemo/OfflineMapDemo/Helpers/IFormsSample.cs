using System;

namespace OfflineMapDemo.Helpers
{
    public interface IFormsSample : ISample
    {
        bool OnClick(object sender, EventArgs args);
    }
}
