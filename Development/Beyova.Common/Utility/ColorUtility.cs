using System;
using System.Collections.Generic;
using System.Drawing;

namespace Beyova
{
    /// <summary>
    /// Singleton utility class for color.
    /// </summary>
    public class ColorUtility
    {
        /// <summary>
        /// The known colors
        /// </summary>
        protected static KnownColor[] knownColors = null;

        #region Constructor

        /// <summary>
        /// Initializes the <see cref="ColorUtility" /> class.
        /// </summary>
        protected ColorUtility()
        {
            List<KnownColor> colorList = new List<KnownColor>();
            foreach (KnownColor knownColor in Enum.GetValues(typeof(KnownColor)))
            {
                Color bc = Color.FromKnownColor(knownColor);
                if (!bc.IsSystemColor && bc != Color.Transparent)
                {
                    colorList.Add(knownColor);
                }
            }
            knownColors = colorList.ToArray();
        }

        #endregion Constructor

        /// <summary>
        /// Gets the known color set.
        /// </summary>
        /// <returns>List&lt;Color&gt;.</returns>
        public static List<Color> GetKnownColorSet()
        {
            List<Color> colors = new List<Color>();

            foreach (KnownColor knownColor in Enum.GetValues(typeof(KnownColor)))
            {
                Color bc = Color.FromKnownColor(knownColor);
                if (!bc.IsSystemColor && bc != Color.Transparent)
                    colors.Add(bc);
            }

            return colors;
        }

        /// <summary>
        /// Gets the color of the closest.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>KnownColor.</returns>
        public static KnownColor GetClosestColor(Color color)
        {
            int offset = 255 * 3;
            KnownColor closestColor = (KnownColor)1;
            foreach (KnownColor knownColor in knownColors)
            {
                int currentOffset = ColorDiff(color, Color.FromKnownColor(knownColor));
                if (currentOffset < offset)
                {
                    closestColor = knownColor;
                    offset = currentOffset;
                }
            }

            return closestColor;
        }

        /// <summary>
        /// Colors the diff.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="curr">The curr.</param>
        /// <returns>System.Int32.</returns>
        public static int ColorDiff(Color color, Color curr)
        {
            return Math.Abs(color.R - curr.R) + Math.Abs(color.G - curr.G) + Math.Abs(color.B - curr.B);
        }

        /// <summary>
        /// Gets the image feature set.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>Dictionary&lt;System.String, System.Int32&gt;.</returns>
        public Dictionary<string, int> GetImageFeatureSet(Bitmap image)
        {
            Dictionary<string, int> features = new Dictionary<string, int>();

            foreach (KnownColor knownColor in knownColors)
            {
                features.Add(knownColor.ToString(), 0);
            }

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    KnownColor knownColor = GetClosestColor(pixelColor);
                    features[knownColor.ToString()]++;
                }
            }

            return features;
        }
    }
}