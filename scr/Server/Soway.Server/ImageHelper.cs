using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Soway.Service
{
    public class ImageHelper
    {
        public static string GetImageStr(System.Drawing.Image img)
        {
            Stream ms = new System.IO.MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] data = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(data, 0, data.Length);
            
            return Convert.ToBase64String(data);
        }
    }
}