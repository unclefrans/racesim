using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;

namespace WpfApp1
{
    public static class ImageController
    {
        private static Dictionary<string, Bitmap> _imageCache = new Dictionary<string, Bitmap>();
        
        /// <summary>
        /// Makes or gets an image cache,
        ///  if the url is 'empty', make the background with calculated width and height
        /// </summary>
        /// <param name="url">url of the value of a graphic</param>
        /// <param name="width">width of the empty map</param>
        /// <param name="height">height of the empty map</param>
        /// <returns>New Bitmap</returns>
        public static Bitmap GetImageCache(string url, int width, int height)
        {
            if (_imageCache.TryGetValue(url, out var value))
            {
                return value;
            }
            if (url == "empty")
            {
                var newBitmap = EmptyImageBitmap(width, height);
                _imageCache.Add(url, newBitmap);
                return newBitmap;
            }
            else
            {
                var newBitmap = new Bitmap(url);
                _imageCache.Add(url, newBitmap);
                return newBitmap;
            }
        }

        /// <summary>
        /// Clears the image cache
        /// </summary>
        public static void ClearCache()
        {
            _imageCache.Clear();
        }
        
        /// <summary>
        /// Makes an empty image bitmap with colored background
        /// </summary>
        /// <param name="width">Width of the entire race circuit</param>
        /// <param name="height">Height of the entire race circuit</param>
        /// <returns>The background where everything else will be drawn on</returns>
        public static Bitmap EmptyImageBitmap(int width, int height)
        {
            var bitMap = new Bitmap(width, height);
            using var gfx = Graphics.FromImage(bitMap);
            using var brush = new SolidBrush(Color.CadetBlue);
            gfx.FillRectangle(brush, 0, 0, width, height);
            return bitMap;
        }

        /// <summary>
        /// Creates BitmapSource from Bitmap
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns>BitmapSource</returns>
        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            try
            {
                var size = (rect.Width * rect.Height) * 4;
                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
    }
}
