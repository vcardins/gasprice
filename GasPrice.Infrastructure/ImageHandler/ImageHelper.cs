#region credits
// ***********************************************************************
// Assembly	: GasPrice.Infrastructure
// Author	: Victor Cardins
// Created	: 03-16-2013
// 
// Last Modified By : Victor Cardins
// Last Modified On : 03-28-2013
// ***********************************************************************
#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace GasPrice.Infrastructure.ImageHandler
{
    #region

    

    #endregion

    /// <summary>
    /// Provides various image untilities, such as high quality resizing and the ability to save a JPEG.
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// The _encoders.
        /// </summary>
        private static Dictionary<string, ImageCodecInfo> _encoders;

        /// <summary>
        /// A quick lookup for getting image encoders
        /// </summary>
        public static Dictionary<string, ImageCodecInfo> Encoders
        {
            // get accessor that creates the dictionary on demand
            get
            {
                // if the quick lookup isn't initialised, initialise it
                if (_encoders == null)
                {
                    _encoders = new Dictionary<string, ImageCodecInfo>();
                }

                // if there are no codecs, try loading them
                if (_encoders.Count == 0)
                {
                    // get all the codecs
                    foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders())
                    {
                        // add each codec to the quick lookup
                        _encoders.Add(codec.MimeType.ToLower(), codec);
                    }
                }

                // return the lookup
                return _encoders;
            }
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">
        /// The image to resize.
        /// </param>
        /// <param name="width">
        /// The width to resize to.
        /// </param>
        /// <param name="height">
        /// The height to resize to.
        /// </param>
        /// <returns>
        /// The resized image.
        /// </returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            // a holder for the result
            var result = new Bitmap(width, height);

            // set the resolutions the same to avoid cropping due to resolution differences
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            // use a graphics object to draw the resized image into the bitmap
            using (Graphics graphics = Graphics.FromImage(result))
            {
                // set the resize quality modes to high quality
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                // draw the image into the target bitmap
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            // return the resulting bitmap
            return result;
        }

        /// <summary>
        /// The resize image.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        /// <returns>
        /// The System.Drawing.Bitmap.
        /// </returns>
        public static Bitmap ResizeImage(Stream stream, int width, int height)
        {
            Image image = Image.FromStream(stream);

            return ResizeImage(image, width, height);
        }

        /// <summary>
        /// 
        /// Saves an image as a jpeg image, with the given quality 
        /// </summary>
        /// <param name="path">
        /// Path to which the image would be saved.
        /// </param>
        /// <param name="image">
        /// Image File
        /// </param>
        /// <param name="quality">
        /// An integer from 0 to 100, with 100 being the 
        /// highest quality
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// An invalid value was entered for image quality.
        /// </exception>
        public static void SaveImage(string path, Image image, int quality)
        {
            SaveImage(path, image, quality, "image/jpeg");
        }

        /// <summary>
        /// The save image.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="image">
        /// The image.
        /// </param>
        /// <param name="quality">
        /// The quality.
        /// </param>
        /// <param name="mimeType">
        /// The mime type.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        public static void SaveImage(string path, Image image, int quality, string mimeType)
        {
            // ensure the quality is within the correct range
            if ((quality < 0) || (quality > 100))
            {
                // create the error message
                string error =
                    string.Format(
                        "Jpeg image quality must be between 0 and 100, with 100 being the highest quality.  A value of {0} was specified.", 
                        quality);

                // throw a helpful exception
                throw new ArgumentOutOfRangeException(error);
            }

            // create an encoder parameter for the image quality
            var qualityParam = new EncoderParameter(Encoder.Quality, quality);

            // get the jpeg codec
            ImageCodecInfo encoderInfo = GetEncoderInfo(mimeType);

            // create a collection of all parameters that we will pass to the encoder
            var encoderParams = new EncoderParameters(1);

            // set the quality parameter for the codec
            encoderParams.Param[0] = qualityParam;

            // save the image using the codec and the parameters
            image.Save(path, encoderInfo, encoderParams);
        }

        /// <summary>
        /// Returns the image codec with the given mime type
        /// </summary>
        /// <param name="mimeType">
        /// The mime Type.
        /// </param>
        /// <returns>
        /// The System.Drawing.Imaging.ImageCodecInfo.
        /// </returns>
        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // do a case insensitive search for the mime type
            string lookupKey = mimeType.ToLower();

            // the codec to return, default to null
            ImageCodecInfo foundCodec = null;

            // if we have the encoder, get it to return
            if (Encoders.ContainsKey(lookupKey))
            {
                // pull the codec from the lookup
                foundCodec = Encoders[lookupKey];
            }

            return foundCodec;
        }
    }
}