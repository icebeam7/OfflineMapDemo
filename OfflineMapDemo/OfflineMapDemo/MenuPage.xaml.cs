using OfflineMapDemo.Helpers;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OfflineMapDemo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : ContentPage
	{
		public MenuPage ()
		{
			InitializeComponent ();
		}

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var sample = new MbTilesSample();
            Func<object, EventArgs, bool> clicker = sample.OnClick;

            var page = (NavigationPage)Application.Current.MainPage;

            await (page).PushAsync(new MainPage(sample.Setup, clicker));
            //await (page).PushAsync(new MainPage(sample.Setup, null));
        }
    }
}