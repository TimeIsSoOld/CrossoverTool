using System;

namespace VIA.SmartVision.DisplayHalImage
{
    public class VIAHalImageDisplay
    {
        /// <summary>
        /// 根据文件名确定当前图显示的位置
        /// </summary>
        /// <param name="safeName"></param>
        /// <returns></returns>
        public HalPosition GetIndex(string safeName)
        {
            int index = Convert.ToInt32(safeName.Split('.')[0]);
            switch (index)
            {
                case 1:
                    return HalPosition.Top;
                case 2:
                    return HalPosition.RightTop;
                case 3:
                    return HalPosition.Right;
                case 4:
                    return HalPosition.RightBottom;
                case 5:
                    return HalPosition.Bottom;
                case 6:
                    return HalPosition.LeftBottom;
                case 7:
                    return HalPosition.Left;
                case 8:
                    return HalPosition.LeftTop;
            }
            return HalPosition.None;
        }
    }

    public enum HalPosition { Top = 1, RightTop, Right, RightBottom, Bottom, LeftBottom, Left, LeftTop, None = -1 }
}
