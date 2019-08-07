using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace SoftPainter.Serial
{
    /// <summary>
    /// 喷涂点位
    /// </summary>
    [ProtoContract]
    public class RobotPosition
    {
        /// <summary>
        /// 点位序号
        /// </summary>
        [ProtoMember(1)]
        public int Num { get; set; }
        /// <summary>
        /// 轨迹X坐标（单位：mm）
        /// </summary>
        [ProtoMember(2)]
        public double X { get; set; }
        /// <summary>
        /// 轨迹Y坐标（单位：mm）
        /// </summary>
        [ProtoMember(3)]
        public double Y { get; set; }
        /// <summary>
        /// 轨迹Z坐标（单位：mm）
        /// </summary>
        [ProtoMember(4)]
        public double Z { get; set; }
        /// <summary>
        /// 轨迹Rx坐标（单位：度）
        /// </summary>
        [ProtoMember(5)]
        public double Rx { get; set; }
        /// <summary>
        /// 轨迹Ry坐标（单位：度）
        /// </summary>
        [ProtoMember(6)]
        public double Ry { get; set; }
        /// <summary>
        /// 轨迹Rz坐标（单位：度）
        /// </summary>
        [ProtoMember(7)]
        public double Rz { get; set; }
        /// <summary>
        /// 轨迹速度（单位：mm/s）
        /// </summary>
        [ProtoMember(8)]
        public double Speed { get; set; }
        /// <summary>
        /// 定位精度（PL=0-9）
        /// </summary>
        [ProtoMember(9)]
        public PLType PositionAccuracy { get; set; }
        /// <summary>
        /// 喷枪状态
        /// </summary>
        [ProtoMember(10)]
        public SprayGunStatusType SprayGunStatusType { get; set; }

        /// <summary>
        /// X值基于的角位置
        /// </summary>
        [ProtoMember(13, IsRequired = false)]
        public PositionBase strBaseHornX { get; set; }

        /// <summary>
        /// Y值基于的角位置
        /// </summary>
        [ProtoMember(14, IsRequired = false)]
        public PositionBase strBaseHornY { get; set; }

        /// <summary>
        /// 点位前调用JOB
        /// </summary>
        [ProtoMember(15, IsRequired = false)]
        public string CallJobNameBefore { get; set; }

        /// <summary>
        /// 点位后调用JOB
        /// </summary>
        [ProtoMember(16, IsRequired = false)]
        public string CallJobNameAfter { get; set; }

        /// <summary>
        /// X是否按比例
        /// </summary>
        [ProtoMember(17, IsRequired = false)]
        public bool IsXScale { get; set; }

        /// <summary>
        /// Y是否按比例
        /// </summary>
        [ProtoMember(18, IsRequired = false)]
        public bool IsYScale { get; set; }
        ///<summary>
        ///工具坐标编号
        ///</summary>
        [ProtoMember(19, IsRequired = false)]
        public int ToolNo { get; set; }
        ///<summary>
        ///CTP
        ///</summary>
        [ProtoMember(20, IsRequired = false)]
        public int CTP { get; set; }

        public RobotPosition(int num, double x, double y, double z, double rx, double ry, double rz, double speed, PLType positionAccuracy, SprayGunStatusType sprayGunStatusType)
        {
            this.Num = num;
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Rx = rx;
            this.Ry = ry;
            this.Rz = rz;
            this.Speed = speed;
            this.PositionAccuracy = positionAccuracy;
            this.SprayGunStatusType = sprayGunStatusType;
        }

        public RobotPosition(RobotPosition old)
        {
            this.Num = old.Num;
            this.X = old.X;
            this.Y = old.Y;
            this.Z = old.Z;
            this.Rx = old.Rx;
            this.Ry = old.Ry;
            this.Rz = old.Rz;
            this.Speed = old.Speed;
            this.PositionAccuracy = old.PositionAccuracy;
            this.SprayGunStatusType = old.SprayGunStatusType;
            this.strBaseHornX = old.strBaseHornX;
            this.strBaseHornY = old.strBaseHornY;
            this.CallJobNameBefore = old.CallJobNameBefore;
            this.CallJobNameAfter = old.CallJobNameAfter;
            this.IsXScale = old.IsXScale;
            this.IsYScale = old.IsYScale;
            this.ToolNo = old.ToolNo;
            this.CTP = old.CTP;
        }

        public RobotPosition() { }
    }
}
