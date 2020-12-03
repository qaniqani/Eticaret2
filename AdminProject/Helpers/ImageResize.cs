using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace AdminProject.Helpers
{
    public class ImageResize
    {
        public static Image ScaleByPercent(Image imgPhoto, int percent)
        {
            var nPercent = (float)percent / 100;

            var sourceWidth = imgPhoto.Width;
            var sourceHeight = imgPhoto.Height;
            const int sourceX = 0;
            const int sourceY = 0;

            const int destX = 0;
            const int destY = 0;
            var destWidth = (int)(sourceWidth * nPercent);
            var destHeight = (int)(sourceHeight * nPercent);

            var bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format16bppRgb555);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            var grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.Low;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public static byte[] FixedSize(string imagePath, int width, int height, string colorCode)
        {
            using (var fs = File.Open(imagePath, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                using (var imgPhoto = Image.FromStream(fs))
                {
                    //Image imgPhoto = Image.FromFile(imagePath);
                    var sourceWidth = imgPhoto.Width;
                    var sourceHeight = imgPhoto.Height;
                    const int sourceX = 0;
                    const int sourceY = 0;
                    var destX = 0;
                    var destY = 0;

                    float nPercent;

                    var nPercentW = (width / (float)sourceWidth);
                    var nPercentH = (height / (float)sourceHeight);

                    //if we have to pad the height pad both the top and the bottom
                    //with the difference between the scaled height and the desired height
                    if (nPercentH < nPercentW)
                    {
                        nPercent = nPercentH;
                        destX = (int)((width - (sourceWidth * nPercent)) / 2);
                    }
                    else
                    {
                        nPercent = nPercentW;
                        destY = (int)((height - (sourceHeight * nPercent)) / 2);
                    }

                    var destWidth = (int)(sourceWidth * nPercent);
                    var destHeight = (int)(sourceHeight * nPercent);

                    var bmPhoto = new Bitmap(width, height, PixelFormat.Format64bppPArgb);
                    bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

                    var grPhoto = Graphics.FromImage(bmPhoto);
                    grPhoto.Clear(ColorTranslator.FromHtml(colorCode));
                    grPhoto.InterpolationMode = InterpolationMode.Low;

                    grPhoto.DrawImage(imgPhoto,
                        new Rectangle(destX, destY, destWidth, destHeight),
                        new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                        GraphicsUnit.Pixel);

                    byte[] buffer;
                    using (var ms = new MemoryStream())
                    {
                        bmPhoto.Save(ms, ImageFormat.Jpeg);
                        buffer = ms.ToArray();
                    }

                    imgPhoto.Dispose();
                    grPhoto.Dispose();
                    bmPhoto.Dispose();

                    return buffer;
                }
            }
        }

        public static byte[] ImgCrop(string imagePath, int width, int height)
        {
            try
            {
                using (var fs = File.Open(imagePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    using (var imgPhoto = Image.FromStream(fs))
                    {
                        //Image imgPhoto = Image.FromFile(imagePath);
                        var sourceWidth = imgPhoto.Width;
                        var sourceHeight = imgPhoto.Height;
                        const int sourceX = 0;
                        const int sourceY = 0;
                        var destX = 0;
                        var destY = 0;

                        float nPercent;

                        var nPercentW = (width / (float)sourceWidth);
                        var nPercentH = (height / (float)sourceHeight);

                        if (nPercentH < nPercentW)
                        {
                            nPercent = nPercentW;
                            destY = (int)((height - (sourceHeight * nPercent)) / 2);
                        }
                        else
                        {
                            nPercent = nPercentH;
                            destX = (int)((width - (sourceWidth * nPercent)) / 2);
                        }

                        var destWidth = (int)(sourceWidth * nPercent);
                        var destHeight = (int)(sourceHeight * nPercent);

                        var bmPhoto = new Bitmap(width, height, PixelFormat.Format16bppRgb555);
                        bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

                        var grPhoto = Graphics.FromImage(bmPhoto);
                        grPhoto.Clear(Color.White);
                        grPhoto.CompositingQuality = CompositingQuality.HighSpeed;
                        grPhoto.InterpolationMode = InterpolationMode.Low;
                        grPhoto.SmoothingMode = SmoothingMode.Default;
                        //draw the image into the target bitmap

                        grPhoto.DrawImage(imgPhoto,
                            new Rectangle(destX, destY, destWidth, destHeight),
                            new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                            GraphicsUnit.Pixel);

                        byte[] buffer;
                        using (var ms = new MemoryStream())
                        {
                            bmPhoto.Save(ms, ImageFormat.Jpeg);
                            buffer = ms.ToArray();
                        }
                        imgPhoto.Dispose();
                        grPhoto.Dispose();
                        bmPhoto.Dispose();

                        return buffer;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }
    }
}