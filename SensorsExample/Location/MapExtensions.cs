using System;
using Windows.ApplicationModel;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Bing.Maps;
using Microsoft.Xaml.Interactivity;

namespace SensorsExample
{
    public class MapExtensions : DependencyObject, IBehavior
    {
        #region Fields
        private Map _map; 
        #endregion

        #region Position

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            "Position", typeof(BasicGeoposition), typeof(MapExtensions), new PropertyMetadata(default(BasicGeoposition), OnPositionPropertyChanged));

        private static void OnPositionPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (DesignMode.DesignModeEnabled) return;

            var newValue = (BasicGeoposition)args.NewValue;
            var extensionInstance = (MapExtensions)dependencyObject;
            var mapCenter = extensionInstance._map.Center;
            if (mapCenter.Latitude != newValue.Latitude || mapCenter.Longitude != newValue.Longitude)
            {
                extensionInstance._map.SetView(new Location(newValue.Latitude, newValue.Longitude));
            }
        }

        public BasicGeoposition Position
        {
            get { return (BasicGeoposition)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        } 

        #endregion

        #region Heading

        public static readonly DependencyProperty HeadingProperty = DependencyProperty.Register(
            "Heading", typeof(Double), typeof(MapExtensions), new PropertyMetadata(default(Double), OnHeadingPropertyChanged));

        private static void OnHeadingPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (DesignMode.DesignModeEnabled) return;

            var newValue = (Double)args.NewValue;
            var extensionInstance = (MapExtensions)dependencyObject;
            var mapHeading = extensionInstance._map.Heading;
            if (extensionInstance._map.RotationEnabled && mapHeading != newValue)
            {
                extensionInstance._map.SetHeading(newValue);
            }
        }

        public Double Heading
        {
            get { return (Double)GetValue(HeadingProperty); }
            set { SetValue(HeadingProperty, value); }
        } 

        #endregion

        #region Zoom Level

        public static readonly DependencyProperty ZoomLevelProperty = DependencyProperty.Register(
            "ZoomLevel", typeof(Double), typeof(MapExtensions), new PropertyMetadata(default(Double), OnZoomLevelPropertyChanged));

        private static void OnZoomLevelPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (DesignMode.DesignModeEnabled) return;

            var newValue = (Double)args.NewValue;
            var extensionInstance = (MapExtensions)dependencyObject;
            var zoomLevel = extensionInstance._map.ZoomLevel;
            if (zoomLevel != newValue)
            {
                extensionInstance._map.SetZoomLevel(newValue);
            }
        }

        public Double ZoomLevel
        {
            get { return (Double)GetValue(ZoomLevelProperty); }
            set { SetValue(ZoomLevelProperty, value); }
        } 

        #endregion

        #region IBehavior Implementation

        /// <summary>
        /// Attaches to the specified object.
        /// </summary>
        /// <param name="associatedObject">The <see cref="T:Windows.UI.Xaml.DependencyObject"/> to which the <seealso cref="T:Microsoft.Xaml.Interactivity.IBehavior"/> will be attached.</param>
        public void Attach(DependencyObject associatedObject)
        {
            var associatedMap = associatedObject as Map;
            if (associatedMap == null) throw new InvalidOperationException("Behavior must be applied to a Map control");
            _map = associatedMap;
            _map.ViewChanged += HandleMapViewChanged;
        }

        private void HandleMapViewChanged(Object sender, ViewChangedEventArgs args)
        {
            Position = new BasicGeoposition { Latitude = _map.Center.Latitude, Longitude = _map.Center.Longitude };
            ZoomLevel = _map.ZoomLevel;
        }

        /// <summary>
        /// Detaches this instance from its associated object.
        /// </summary>
        public void Detach()
        {
            _map.ViewChanged -= HandleMapViewChanged;
        }

        /// <summary>
        /// Gets the <see cref="T:Windows.UI.Xaml.DependencyObject"/> to which the <seealso cref="T:Microsoft.Xaml.Interactivity.IBehavior"/> is attached.
        /// </summary>
        public DependencyObject AssociatedObject
        {
            get { return _map; }
        } 

        #endregion
    }
}