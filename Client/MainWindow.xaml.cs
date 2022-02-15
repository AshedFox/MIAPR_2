using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MaxMinLib;
using Color = System.Windows.Media.Color;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MaxMin _kMeans;        
        public MainWindow()
        {
            InitializeComponent();
            
            _kMeans = new MaxMin();
        }

        public GeometryDrawing GeneratePointsGeometry(MaxMinCluster cluster, Color color)
        {
            var geometryGroup = new GeometryGroup { FillRule = FillRule.Nonzero };

            foreach (var point in cluster.Points)
            {
                geometryGroup.Children.Add(new EllipseGeometry(new Point(point.X, point.Y), 1, 1));
            }

            var brush = new SolidColorBrush(color);
            var pen = new Pen(brush, 0);
            var drawing = new GeometryDrawing(brush, pen, geometryGroup);

            return drawing;
        }

        private void RedrawMaxMinImage()
        {
            var colorStep = (int)Math.Floor((decimal)255 / (_kMeans.Clusters.Count - 1));
            var clusters = _kMeans.Clusters;
            
            var drawingGroup = new DrawingGroup();
            var centerColor = Color.FromArgb(255, 255, 0, 150);
            var centerBrush = new SolidColorBrush(centerColor);
            var centerPen = new Pen(centerBrush, 0);

            var i = 0;
            foreach (var cluster in clusters)
            {
                drawingGroup.Children.Add(GeneratePointsGeometry(cluster,
                    Color.FromArgb(255, (byte)(i * colorStep), (byte)(i * colorStep), (byte)(i * colorStep))));

                drawingGroup.Children.Add(new GeometryDrawing(centerBrush, centerPen,
                    new EllipseGeometry(new Point(cluster.Center.X, cluster.Center.Y), 2, 2)));
                
                i++;
            }

            MaxMinImage.Source = new DrawingImage(drawingGroup);
        }

        private void ReInitButton_OnClick(object sender, RoutedEventArgs e)
        {
            _kMeans.Init(PointsGenerator.GeneratePoints(Convert.ToInt32(PointsCountInput.Text)));
            MaxMinImage.Source = null;
            IterateOnceButton.IsEnabled = true;
            IterateAllButton.IsEnabled = true;
            MaxMinStatus.IsChecked = false;
        }

        private void IterateOnceButton_OnClick(object sender, RoutedEventArgs e)
        {
            MaxMinStatus.IsChecked = _kMeans.Iterate();
            RedrawMaxMinImage();
        }

        private void IterateAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            bool isEnd;

            do
            {
                isEnd = _kMeans.Iterate();
                RedrawMaxMinImage();
            } while (!isEnd);

            MaxMinStatus.IsChecked = true;
        }

        private void PointsCountInput_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(Convert.ToChar(e.Text));
        }

        private void MaxMinStatus_OnChecked(object sender, RoutedEventArgs e)
        {
            if (MaxMinStatus.IsChecked is true)
            {
                IterateOnceButton.IsEnabled = false;
                IterateAllButton.IsEnabled = false;
            }
        }
    }
}