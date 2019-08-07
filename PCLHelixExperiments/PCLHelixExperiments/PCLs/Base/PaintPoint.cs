using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SoftPainter.Core.Base;
using SoftPainter.Serial;

namespace SoftPainter.Core.Base
{

    public enum ArcStatusType
    {
        On,         //角度开启
        Off,        //角度关闭
        Maintain    //角度保持
    }
    public enum FacadeAccelerateStatusType
    {
        Invalid,    //非正面
        False,      //非加速点
        True,       //加速运行
        FastSpeed,  //高速运行
        SuperSpeed,  //超高速运行
        InsideCurveSpeed,  //内圈速度运行
        RobotMoveSpeed
    }
    public enum SpeedVarNoType
    {
        Facade = 0,             //正面速度
        Inside = 1,             //内圈速度
        Outer = 2,              //外圈速度
        InsideCorner = 3,       //内圈转角速度
        FacadeAccelerate = 4,   //正面加速点速度
        RobotMove = 5,          //机器人运行速度
        FastSpeed = 6,          //高速
        SuperSpeed = 7          //超高速
    }
    public enum PositionType
    {
        None,
        OuterBegin,             //外圈起始点
        OuterEnd,               //外圈结束点
        InsideBegin,            //内圈起始点
        InsideEnd,              //内圈结束点
        FacadeBegin,            //正面起始点
        FacadeEnd,              //正面结束点
        FacadeResetBegin,       //正面IF起始点
        FacadeResetEnd          //正面IF结束点
    }

    public class PaintPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double Rx { get; set; }
        public double Ry { get; set; }
        public double Rz { get; set; }

        public double ACC { get; set; }
        public double DEC { get; set; }

        public int VarNo { get; set; }
        public PLType PL { get; set; }
        public SpeedVarNoType SpeedVarNo { get; set; }
        public double Speed { get; set; }
        public SprayGunStatusType SprayGunStatus { get; set; }
        public ArcStatusType ArcStatus { get; set; }
        public FacadeAccelerateStatusType FacadeAccelerateStatus { get; set; }
        public PositionType Position { get; set; }
        public ACC_DEC_TYPE ACC_DEC_Status { get; set; }

        public string CallJobNameBefore { get; set; }
        public string CallJobNameAfter { get; set; }

        public PaintPoint(SprayGunStatusType sprayGunStatusType = SprayGunStatusType.Maintain, PLType pLType = PLType.PL_1)
        {
            SprayGunStatus = sprayGunStatusType;
            PL = pLType;
        }

        public PaintPoint(PointXYZR pt, SprayGunStatusType sprayGunStatusType = SprayGunStatusType.Maintain, FacadeAccelerateStatusType facadeAccelerateStatusType = FacadeAccelerateStatusType.Invalid, PLType pLType = PLType.PL_1, double deltaRx = 0, double deltaRy = 0, double deltaRz = 0, ACC_DEC_TYPE acc_dec_type = ACC_DEC_TYPE.None, double acc_ratio = 100, double dec_ratio = 100)
        {
            X = pt.X;
            Y = pt.Y;
            Z = pt.Z;
            Rx = pt.Rx + deltaRx;
            Ry = pt.Ry + deltaRy;
            Rz = pt.Rz + deltaRz;

            SprayGunStatus = sprayGunStatusType;
            FacadeAccelerateStatus = facadeAccelerateStatusType;
            PL = pLType;
            ACC_DEC_Status = acc_dec_type;
            ACC = acc_ratio;
            DEC = dec_ratio;
        }

        //public PaintPoint(RobotPositionData rpd)
        //{
        //    X = rpd.X;
        //    Y = rpd.Y;
        //    Z = rpd.Z;
        //    Rx = rpd.Rx;
        //    Ry = rpd.Ry;
        //    Rz = rpd.Rz;

        //    switch (rpd.speedType)
        //    {
        //        //外框2，内框1，正面0，移动4，内框转角3
        //        case 0: Speed = StaticAll.Parameters.zhengmianSpeed; SpeedVarNo = SpeedVarNoType.Facade; break;
        //        case 1: Speed = StaticAll.Parameters.neiquanSpeed; SpeedVarNo = SpeedVarNoType.Inside; break;
        //        case 2: Speed = StaticAll.Parameters.waiquanSpeed; SpeedVarNo = SpeedVarNoType.Outer; break;
        //        case 3: Speed = StaticAll.Parameters.neiquanSpeed * StaticAll.Parameters.cornerSpeedCoef; SpeedVarNo = SpeedVarNoType.InsideCorner; break;
        //        case 4: Speed = StaticAll.Parameters.robotMoveSpeed; SpeedVarNo = SpeedVarNoType.RobotMove; break;
        //        default: if (rpd.speedType >= 50) Speed = StaticAll.Parameters.neiquanSpeed * StaticAll.Parameters.cornerSpeedCoef; SpeedVarNo = SpeedVarNoType.InsideCorner; break;
        //    }

        //    switch (rpd.paintType)
        //    {
        //        //2表示PAINTOFF，1表示PAINTON，0表示无，4表示开第一把枪，5表示开第二把枪
        //        case 0: SprayGunStatus = SprayGunStatusType.Maintain; break;
        //        case 1: SprayGunStatus = SprayGunStatusType.Open; break;
        //        case 2: SprayGunStatus = SprayGunStatusType.Close; break;
        //        case 4: SprayGunStatus = SprayGunStatusType.OpenAndCloseSecond; break;
        //        case 5: SprayGunStatus = SprayGunStatusType.OpenAndCloseFirst; break;
        //        default: break;
        //    }

        //    switch (rpd.pl)
        //    {
        //        case 0: PL = PLType.None; break;
        //        case 1: PL = PLType.PL_1; break;
        //        default: break;
        //    }
        //}

        public PaintPoint(PaintPoint old, double rz)
        {
            this.X = old.X;
            this.Y = old.Y;
            this.Z = old.Z;
            this.Rx = old.Rx;
            this.Ry = old.Ry;
            this.Rz = rz;
            this.PL = old.PL;
            this.SpeedVarNo = old.SpeedVarNo;
            this.Speed = old.Speed;
            this.ArcStatus = old.ArcStatus;
            this.FacadeAccelerateStatus = old.FacadeAccelerateStatus;
            this.SprayGunStatus = SprayGunStatusType.Maintain;
        }

        public PaintPoint(PaintPoint old,double rx, double ry, double rz)
        {
            this.X = old.X;
            this.Y = old.Y;
            this.Z = old.Z;
            this.Rx = old.Rx;
            this.Ry = old.Ry;
            this.Rz = rz;
            this.PL = old.PL;
            this.SpeedVarNo = old.SpeedVarNo;
            this.Speed = old.Speed;
            this.ArcStatus = ArcStatusType.Maintain;
            this.FacadeAccelerateStatus = FacadeAccelerateStatusType.Invalid;
            this.SprayGunStatus = SprayGunStatusType.Maintain;
        }

        public PaintPoint(PaintPoint old)
        {
            this.X = old.X;
            this.Y = old.Y;
            this.Z = old.Z;
            this.Rx = old.Rx;
            this.Ry = old.Ry;
            this.Rz = old.Rz;
            this.PL = old.PL;
            this.SpeedVarNo = old.SpeedVarNo;
            this.Speed = old.Speed;
            this.ArcStatus = old.ArcStatus;
            this.FacadeAccelerateStatus = old.FacadeAccelerateStatus;
            this.SprayGunStatus = SprayGunStatusType.Maintain;
        }
    }
}
