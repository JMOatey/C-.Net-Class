using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup.Localizer;
using System.Windows.Media.Imaging;

namespace DeadOfWinterWPF.Classes
{
    public static class ImageLoader
    {
        private static BitmapImage LoadBitmap(string filepath, double decodeWidth)
        {
            var theBitmap = new BitmapImage();
            theBitmap.BeginInit();
            var basePath = System.IO.Path.Combine(Environment.CurrentDirectory, @"Images\");
            var path = System.IO.Path.Combine(basePath, filepath);
            theBitmap.UriSource = new Uri(path, UriKind.Absolute);
            theBitmap.DecodePixelWidth = (int)decodeWidth;
            theBitmap.EndInit();

            return theBitmap;
        }

        public static Image InitializeMedia(string filePath)
        {
            const int width = 64;
            var myImage = new Image { Width = width };
            var bitmapImage = LoadBitmap(filePath, width);
            myImage.Source = bitmapImage;

            return myImage;
        }

        public static void AddSurvivor(this Location location, Image image)
        {
            var i = 50 * location.SurvivorSpots;
            location.Canvas.Children.Add(image);
            Canvas.SetLeft(image, i);
            Canvas.SetTop(image, 50);
            location.SurvivorSpots--;
        }

        public static void RemoveImage(this Location location, Image image)
        {
            location.Canvas.Children.Remove(image);
        }

        public static void AddZombie(this Location location, Image zombie)
        {
            var i = location.OutsideArea.OutsideSpots * 50;
            location.Canvas.Children.Add(zombie);
            Canvas.SetLeft(zombie, i);
            Canvas.SetLeft(zombie, i);
            Canvas.SetBottom(zombie, 50);
            location.OutsideArea.OutsideSpots--;
            location.OutsideArea.Zombies++;
        }
    }
}