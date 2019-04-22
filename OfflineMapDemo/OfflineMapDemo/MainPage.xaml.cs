using System;
using System.IO;
using OfflineMapDemo.Helpers;
//using OfflineMapDemo.Maps;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mapsui.UI.Forms;
using Mapsui.UI;
using System.Threading.Tasks;
using OfflineMapDemo.Models;
using System.Collections.Generic;
using System.Linq;
using Mapsui.Projection;

namespace OfflineMapDemo
{
    public partial class MainPage : ContentPage
    {
        public Func<MapView, MapClickedEventArgs, bool> Clicker { get; set; }
        List<Lugar> lugares = Servicios.ServicioBD.ObtenerLugares();

        public MainPage()
        {
            InitializeComponent();
        }

        public MainPage(Action<IMapControl> setup, Func<MapView, MapClickedEventArgs, bool> c = null)
        {
            InitializeComponent();

            mapView.RotationLock = false;
            mapView.UnSnapRotationDegrees = 30;
            mapView.ReSnapRotationDegrees = 5;

            mapView.PinClicked += OnPinClicked;
            mapView.MapClicked += OnMapClicked;

            var ciudad = lugares.First(x => x.Nombre == "Torrejon de Ardoz");

            setup(mapView);
            mapView.MyLocationLayer.UpdateMyLocation(new Mapsui.UI.Forms.Position(ciudad.Latitud, ciudad.Longitud));
            var center = SphericalMercator.FromLonLat(ciudad.Longitud, ciudad.Latitud);
            var resolution = 10;
            mapView.Map.Home = n => n.NavigateTo(center, resolution);
            Clicker = c;
        }

        private void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            e.Handled = Clicker != null ? (bool)Clicker?.Invoke(sender as MapView, e) : false;
            //Samples.SetPins(mapView, e);
            //Samples.DrawPolylines(mapView, e);
        }

        private async void OnPinClicked(object sender, PinClickedEventArgs e)
        {
            if (e.Pin != null)
            {
                if (e.NumOfTaps == 2)
                {
                    // Hide Pin when double click
                    await DisplayAlert($"Pin {e.Pin.Label}", $"está en la posición {e.Pin.Position}", "Ok");
                    e.Pin.IsVisible = false;
                }
                if (e.NumOfTaps == 1)
                    e.Pin.IsCalloutVisible = !e.Pin.IsCalloutVisible;
            }

            e.Handled = true;
        }
    }
}