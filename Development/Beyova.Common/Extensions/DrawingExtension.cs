using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Beyova
{
    /// <summary>
    /// Extension class for drawing
    /// </summary>
    public static class DrawingExtension
    {
        #region Image

        /// <summary>
        /// Pads the image.
        /// </summary>
        /// <param name="originalImage">The original image.</param>
        /// <returns>Image.</returns>
        public static Image PadImage(this Image originalImage)
        {
            int largestDimension = Math.Max(originalImage.Height, originalImage.Width);
            Size squareSize = new Size(largestDimension, largestDimension);
            Bitmap squareImage = new Bitmap(squareSize.Width, squareSize.Height);
            using (Graphics graphics = Graphics.FromImage(squareImage))
            {
                graphics.FillRectangle(Brushes.White, 0, 0, squareSize.Width, squareSize.Height);
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                graphics.DrawImage(originalImage, (squareSize.Width / 2) - (originalImage.Width / 2), (squareSize.Height / 2) - (originalImage.Height / 2), originalImage.Width, originalImage.Height);
            }
            return squareImage;
        }

        #endregion Image

        #region Bitmap

        /// <summary>
        /// Gets the image encoder.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>ImageCodecInfo.</returns>
        public static ImageCodecInfo GetImageEncoder(this ImageFormat format)
        {
            return ImageCodecInfo.GetImageDecoders().FirstOrDefault(one => one.FormatID == format.Guid);
        }

        /// <summary>
        /// To the bitmap.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>Bitmap.</returns>
        public static Bitmap ToBitmap(this byte[] bytes)
        {
            if (bytes != null)
            {
                return Image.FromStream(bytes.ToStream()) as Bitmap;
            }

            return null;
        }

        /// <summary>
        /// Resizes the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="maxWidth">The maximum width.</param>
        /// <param name="maxHeight">The maximum height.</param>
        /// <param name="keepRatio">if set to <c>true</c> [keep ratio].</param>
        /// <returns>Bitmap.</returns>
        public static Bitmap Resize(this Bitmap image, int maxWidth, int maxHeight, bool keepRatio = true)
        {
            Bitmap bmpOut = null;

            try
            {
                image.CheckNullObject("image");

                if (image.Width < maxWidth && image.Height < maxHeight)
                {
                    bmpOut = image;
                }
                else
                {
                    var newWidth = 0;
                    var newHeight = 0;

                    if (keepRatio)
                    {
                        decimal ratio;

                        if (image.Width > image.Height)
                        {
                            ratio = (decimal)maxWidth / image.Width;
                            newWidth = maxWidth;
                            var lnTemp = image.Height * ratio;
                            newHeight = (int)lnTemp;
                        }
                        else
                        {
                            ratio = (decimal)maxHeight / image.Height;
                            newHeight = maxHeight;
                            var lnTemp = image.Width * ratio;
                            newWidth = (int)lnTemp;
                        }
                    }
                    else
                    {
                        newWidth = (image.Width > maxWidth) ? maxWidth : image.Width;
                        newHeight = (image.Height > maxHeight) ? maxHeight : image.Height;
                    }

                    bmpOut = new Bitmap(newWidth, newHeight);
                    var g = Graphics.FromImage(bmpOut);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.FillRectangle(Brushes.White, 0, 0, newWidth, newHeight);
                    g.DrawImage(image, 0, 0, newWidth, newHeight);

                    image.Dispose();
                }
            }
            catch
            {
                return null;
            }

            return bmpOut;
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="imageFormat">The image format.</param>
        /// <param name="qualityLevel">The quality level.
        /// <remarks>
        /// A quality level of 0 corresponds to the greatest compression, and a quality level of 100 corresponds to the least compression.
        /// https://msdn.microsoft.com/library/bb882583(v=vs.110).aspx
        /// </remarks></param>
        /// <returns>System.Byte[].</returns>
        /// <exception cref="OperationFailureException">GetBytes</exception>
        public static byte[] GetBytes(this Bitmap image, ImageFormat imageFormat = null, long qualityLevel = 100)
        {
            try
            {
                image.CheckNullObject("image");

                if (imageFormat == null)
                {
                    imageFormat = ImageFormat.Jpeg;
                }

                if (qualityLevel > 100 || qualityLevel < 1)
                {
                    qualityLevel = 100;
                }

                using (var memoryStream = new MemoryStream())
                {
                    var jpegEncoder = GetImageEncoder(imageFormat);
                    var encoder = Encoder.Quality;
                    var encoderParameters = new EncoderParameters(1);

                    var myEncoderParameter = new EncoderParameter(encoder, qualityLevel);
                    encoderParameters.Param[0] = myEncoderParameter;
                    image.Save(memoryStream, jpegEncoder, encoderParameters);

                    return memoryStream.ToBytes();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Crops the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>Bitmap.</returns>
        public static Bitmap Crop(this Bitmap image, int x, int y, int width, int height)
        {
            try
            {
                Rectangle cropRect = new Rectangle(x, y, width, height);
                Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(image, new Rectangle(0, 0, target.Width, target.Height),
                                     cropRect,
                                     GraphicsUnit.Pixel);
                }

                return target;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { x, y, width, height });
            }
        }

        #endregion Bitmap
    }
}