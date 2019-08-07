using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace SoftPainter.Serial
{
    /// <summary>
    /// 特定门型的喷涂轨迹
    /// </summary>
    [ProtoContract]
    public class PaintTrack
    {
        /// <summary>
        /// 指定是何种轨迹类型
        /// </summary>
        [ProtoMember(1)]
        public JointKey GUID { get; set; }
        /// <summary>
        /// 轨迹数组
        /// </summary>
        [ProtoMember(2)]
        public List<RobotPosition> Track { get; set; }
        /// <summary>
        /// 门型轨迹基准方向
        /// </summary>
        [ProtoMember(3, IsRequired = false)]
        public PaintDirection BaseDirect { get; set; }
        /// <summary>
        /// 外圈是否喷涂
        /// </summary>
        [ProtoMember(4, IsRequired = false)]
        public bool OutsidePaint { get; set; }
        /// <summary>
        /// 内圈是否喷涂
        /// </summary>
        [ProtoMember(5, IsRequired = false)]
        public bool InsidePaint { get; set; }
        /// <summary>
        /// 内圈边槽是否喷涂
        /// </summary>
        [ProtoMember(6, IsRequired = false)]
        public bool InnerSideGroove { get; set; }
        /// <summary>
        /// 内圈之间线条是否喷涂
        /// </summary>
        [ProtoMember(7, IsRequired = false)]
        public bool InnerLine { get; set; }
        /// <summary>
        /// 正面大面是否喷涂
        /// </summary>
        [ProtoMember(8, IsRequired = false)]
        public bool FacadeFace { get; set; }
        /// <summary>
        /// 长边对称性
        /// </summary>
        [ProtoMember(9)]
        public bool LongSideSymmetry { get; set; }
        /// <summary>
        /// 短边对称性
        /// </summary>
        [ProtoMember(10)]
        public bool ShortSideSymmetry { get; set; }

        public PaintTrack(JointKey guid, List<RobotPosition> track)
        {
            this.GUID = guid;
            this.Track = track;
        }

        public PaintTrack(JointKey guid, List<RobotPosition> track, PaintDirection baseDirect, bool outsidePaint, bool insidePaint, bool innerSideGroove, bool innerLine)
        {
            this.GUID = guid;
            this.Track = track;
            this.BaseDirect = baseDirect;
            this.OutsidePaint = outsidePaint;
            this.InsidePaint = insidePaint;
            this.InnerSideGroove = innerSideGroove;
            this.InnerLine = innerLine;
        }

        public PaintTrack(PaintTrack old)
        {
            this.GUID = new JointKey(old.GUID);
            this.Track = new List<RobotPosition>();
            foreach (RobotPosition rp in old.Track)
                this.Track.Add(new RobotPosition(rp));
            this.BaseDirect = old.BaseDirect;
            this.OutsidePaint = old.OutsidePaint;
            this.InsidePaint = old.InsidePaint;
            this.InnerSideGroove = old.InnerSideGroove;
            this.InnerLine = old.InnerLine;
            this.FacadeFace = old.FacadeFace;
            this.LongSideSymmetry = old.LongSideSymmetry;
            this.ShortSideSymmetry = old.ShortSideSymmetry;
        }

        public PaintTrack() { }
    }
}
