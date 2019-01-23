using System;
using HalconDotNet;
using System.IO;
using VIA.SmartVision.CoreData;

namespace VIA.SmartVision.HalconFunc
{
    public class ConvertToHalImg
    {
        public static void ConvertFunc(out HImage image, string currFile)
        {
            if (string.IsNullOrEmpty(currFile))
            {
                image = null;
                return;
            }
            PointCloud thePoints = new PointCloud();
            FileInfo fileInfo = new FileInfo(currFile);
            long fileSize = fileInfo.Length;
            thePoints.SetCapacity((int)(fileSize / 26));
            int Width, Height;
            using (StreamReader sr = new StreamReader(currFile))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    thePoints.Add(Point3d.ParseFromString(line));
                }
            }
            HalExtention.ConverToCogRangeP(thePoints, 0.0057245f, 0.03f, thePoints.minZ - 0.1f, thePoints.maxZ - 0.1f, out image);
            image.GetImageSize(out Width, out Height);
            int _Height = (int)(Height * 0.03f / 0.0057245f);
            //int _Height = (int)(Height * 0.04f / 0.0055f);
            image = image.ZoomImageSize(Width, _Height, "constant");
        }
    }
}
