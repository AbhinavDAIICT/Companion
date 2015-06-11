using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Device.Location;
using System.Diagnostics;
using Windows.Devices.Geolocation;
using System.Windows.Threading;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Companion
{
    public partial class ReDraw : PhoneApplicationPage
    {
        List<GeoCoordinate> coordList;

        Geolocator geolocator = null;
        string sb;
        private DispatcherTimer timer = new DispatcherTimer();
        bool tracking = false;
        private GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
        private MapPolyline line;

        public ReDraw()
        {
            InitializeComponent();
            ShowMyLocationOnTheMap();
            timer.Interval = TimeSpan.FromSeconds(1);
            line = new MapPolyline();
            line.StrokeColor = Colors.Blue;
            line.StrokeThickness = 10;
            reMap.MapElements.Add(line);
           // drawNow();
        }

        public void drawNow()
        {
            foreach (GeoCoordinate coord in coordList)
            {
                line.Path.Add(coord);
            }
            //for (int i = 0; i < coordList.Capacity; i++)
            //{
            //    line.Path.Add(coordList[i]);
            //}
        }

        private async void ShowMyLocationOnTheMap()
        {
            // Get my current location.
            Geolocator myGeolocator = new Geolocator();
            Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
            Geocoordinate myGeocoordinate = myGeoposition.Coordinate;
            GeoCoordinate myGeoCoordinate =
            LocationOps.ConvertGeocoordinate(myGeocoordinate);
            this.reMap.Center = myGeoCoordinate;
            this.reMap.ZoomLevel = 13;
            Ellipse myCircle = new Ellipse();
            myCircle.Fill = new SolidColorBrush(Colors.Blue);
            myCircle.Height = 4;
            myCircle.Width = 4;
            myCircle.Opacity = 50;

            // Create a MapOverlay to contain the circle.
            MapOverlay myLocationOverlay = new MapOverlay();
            myLocationOverlay.Content = myCircle;
            myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
            myLocationOverlay.GeoCoordinate = myGeoCoordinate;

            // Create a MapLayer to contain the MapOverlay.
            MapLayer myLocationLayer = new MapLayer();
            myLocationLayer.Add(myLocationOverlay);

            // Add the MapLayer to the Map.
            reMap.Layers.Add(myLocationLayer);
        }

        private async void drawButton_Click(object sender, RoutedEventArgs e)
        {
            FileOps fileOps = new FileOps();
         //   fileOps.generateCoordList("def.txt");
            await fileOps.readCoordinate();
            coordList = fileOps.getCoordList();
          //  Debug.WriteLine("got It");
            drawNow();
        }
    }
}