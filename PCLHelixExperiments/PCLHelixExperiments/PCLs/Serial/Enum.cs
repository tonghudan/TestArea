using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftPainter.Serial
{
    public enum ACC_DEC_TYPE
    {
        None = 0,
        ACC,
        DEC,
        ACCDEC
    }
    /// <summary>
    /// 喷枪状态
    /// </summary>
    public enum SprayGunStatusType
    {
        Open,                   //喷枪开启
        Close,                  //喷枪关闭
        Maintain,               //喷枪状态保持
        OpenAndCloseFirst,      //第一单喷枪开启（仅在勾选"大面末道单枪"时有效）
        OpenAndCloseSecond      //第二单喷枪开启（仅在勾选"大面末道单枪"时有效）
    }
    /// <summary>
    /// 定位精度
    /// </summary>
    public enum PLType
    {
        None = -1,      //空
        PL_0 = 0,       //PL=0|绝对定位精度
        PL_1 = 1,       //PL=1|0.5mm
        PL_2 = 2,       //PL=2|1.0mm
        PL_3 = 3,       //PL=3|1.5mm
        PL_4 = 4,       //PL=4|2.0mm
        PL_5 = 5,       //PL=5|2.5mm
        PL_6 = 6,       //PL=6|3.0mm
        PL_7 = 7,       //PL=7|3.5mm
        PL_8 = 8,       //PL=8|4.0mm
        PL_9 = 9        //PL=9|4.5mm
    }
    /// <summary>
    /// 木门喷涂方向
    /// </summary>
    public enum PaintDirection
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public enum PositionBase
    {
        None,
        LU,
        LD,
        RU,
        RD
    }

    public enum OilType
    {
        Clear,      //清油
        Mixed       //混油
    }

    public enum TimesMode
    {
        Single,             //仅正面/单面喷涂
        Double              //正反面/双面喷涂
    }

    public enum Face
    {
        Front,              //正面
        Back                //背面
    }

    public enum LocationModeEnum
    {
        NoPallet,   //无底托
        WithPallet, //有底托
        Suspend,    //悬挂
        Customize   //自定义
    }

    public class ConstString
    {
        public const string CATEGORY_Door = "Door";
        public const string CATEGORY_Furniture = "Furniture";

        public const string PAINTTYPE_Furniture_Single = "Single";
        public const string PAINTTYPE_Furniture_Double_Front = "Double_Front";
        public const string PAINTTYPE_Furniture_Double_Back = "Double_Back";

        public const string PAINTTYPE_DOOR_Clear = "Clear";
        public const string PAINTTYPE_DOOR_Mix = "Mix";
    }
}
