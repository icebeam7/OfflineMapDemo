using System;
using System.IO;
using BruTile.MbTiles;
using Mapsui;
using Mapsui.Layers;
using Mapsui.UI;
using Mapsui.UI.Forms;
using SQLite;

using Mapsui.UI.Objects;
using System.Reflection;
using Xamarin.Forms;

namespace OfflineMapDemo.Helpers
{
    public class MbTilesSample : ISample
    {
        static int markerNum = 1;
        static Random rnd = new Random();

        // This is a hack used for iOS/Android deployment
        public static string MbTilesLocation { get; set; } = @"." + Path.DirectorySeparatorChar + "MbTiles";

        public string Name => "1 MbTiles";
        public string Category => "Data";

        public void Setup(IMapControl mapControl)
        {
            mapControl.Map = CreateMap();
            ((MapView)mapControl).UseDoubleTap = true;
        }

        public static Map CreateMap()
        {
            var map = new Map();
            map.Layers.Add(CreateMbTilesLayer(Path.Combine(
                MbTilesLocation, "world.mbtiles")));
            return map;
        }

        /*
        public bool OnClick(object sender, EventArgs args)
        {
            var mapView = sender as MapView;
            var e = args as MapClickedEventArgs;

            mapView.MyLocationLayer.IsMoving = mapView.MyLocationEnabled;
            mapView.MyLocationEnabled = true;
            mapView.UseDoubleTap = true;

            return false;
        }
        */

        public bool OnClick(object sender, EventArgs args)
        {
            var mapView = sender as MapView;
            var e = args as MapClickedEventArgs;

            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            foreach (var str in assembly.GetManifestResourceNames())
                System.Diagnostics.Debug.WriteLine(str);

            string device;

            switch (Device.RuntimePlatform)
            {
                case "Android":
                    device = "Android";
                    break;
                case "iOS":
                    device = "iOS";
                    break;
                case "macOS":
                    device = "Mac";
                    break;
                default:
                    device = $"{Device.RuntimePlatform}";
                    break;
            }

            switch (e.NumOfTaps)
            {
                case 1:
                    var pin = new Pin(mapView)
                    {
                        Label = $"PinType.Pin {markerNum++}",
                        Address = e.Point.ToString(),
                        Position = e.Point,
                        Type = PinType.Pin,
                        Color = new Color(rnd.Next(0, 255) / 255.0, rnd.Next(0, 255) / 255.0, rnd.Next(0, 255) / 255.0),
                        Transparency = 0.5f,
                        Scale = rnd.Next(50, 130) / 100f,
                    };
                    pin.CalloutAnchor = new Point(0, pin.Height * pin.Scale);
                    pin.Callout.RectRadius = rnd.Next(0, 20);
                    pin.Callout.ArrowHeight = rnd.Next(0, 20);
                    pin.Callout.ArrowWidth = rnd.Next(0, 20);
                    pin.Callout.ArrowAlignment = (ArrowAlignment)rnd.Next(0, 4);
                    pin.Callout.ArrowPosition = rnd.Next(0, 100) / 100;
                    pin.Callout.SubtitleLabel.LineBreakMode = LineBreakMode.NoWrap;
                    mapView.Pins.Add(pin);
                    break;
            }

            return true;
        }

        public static TileLayer CreateMbTilesLayer(string path)
        {
            var mbTilesTileSource = new MbTilesTileSource(new SQLiteConnectionString(path, true));
            var mbTilesLayer = new TileLayer(mbTilesTileSource);
            return mbTilesLayer;
        }
    }
}
