using HalconDotNet;
using System;

using System.Threading.Tasks;
using VIA.SmartVision.CoreData;

namespace VIA.SmartVision.HalconFunc
{
    public static class HalExtention
    {


        /// <summary>
        /// 从数据中生成CogImage16Range,
        /// </summary>
        /// <param name="cloud">点云数据</param>
        /// <param name="XScale">图像在X方向的像素当量</param>
        /// <param name="YScale">图像在Y方向的像素当量</param>
        /// <param name="lowlim">数据0表示的物理高度</param>
        /// <param name="hilim">数据65535表示的物理高度</param>
        /// <param name="uniformScale">当XScale和XScale不一致时是否将两者统一成较小的像素当量，如果xScale为0.5,yScale为0.3则输出的图的XScale和YScale将都是0.3</param>
        /// <param name="theMask">输出深度图的Mask图像</param>
        /// <returns>CogImage16Range</returns>
         public static void ConverToCogRangeP(PointCloud cloud, float XScale, float YScale, float lowlim, float hilim, out HImage theMask)
        {
            float minX;
            float minY;

            float maxX;
            float maxY;

            minX = cloud.minX;
            minY = cloud.minY;

            maxX = cloud.maxX;
            maxY = cloud.maxY;

            int Width = (int)Math.Ceiling((maxX - minX) / XScale) + 1;
            int Height = (int)Math.Ceiling((maxY - minY) / YScale) + 1;

            int xCenter = (int)(-minX / YScale);
            int yCenter = (int)(-minY / YScale);
            HImage data = new HImage("real", Width, Height);


            float zscale = 65535 / (hilim - lowlim);
            
            {
                Parallel.ForEach<Point3d>(
                    cloud.thePoints, new ParallelOptions { MaxDegreeOfParallelism = 5 },
                    (point) =>
                    {
                        float xRel = point.X - minX;
                        float yRel = point.Y - minY;
                        float z = point.Z;
                        int xIdx = (int)Math.Round(xRel / XScale);
                        int yIdx = (int)Math.Round(yRel / YScale);
                        float val = z;
                        if (val < lowlim || val > hilim)
                        {
                            data.SetGrayval(yIdx, xIdx, 0.0);
                        }
                        else
                        {
                            data.SetGrayval(yIdx, xIdx, val);
                        }
                    }
                    );

            }

            theMask = data;
        }

    }
}