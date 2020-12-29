using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using ZXing.QrCode;

namespace QRCodeHelper
{
    public class QRCodeTool
    {
        #region html show Exmaple

        /*
         *  <img src="data:image/png;base64,@(GenerateBase64QrCodeString)"/>
         */

        #endregion html show Exmaple

        /// <summary>
        /// 產生Base64圖檔
        /// </summary>
        /// <param name="text">需要轉換的內容</param>
        /// <param name="height">圖片高度</param>
        /// <param name="width">圖片寬度</param>
        /// <returns></returns>
        public string GenerateBase64QrCode(string text, int height = 300, int width = 300)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width,
                }
            };

            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                writer.Write(text).Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                byte[] imageBytes = stream.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }
    }
}