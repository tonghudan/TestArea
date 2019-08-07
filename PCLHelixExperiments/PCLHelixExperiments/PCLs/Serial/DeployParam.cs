using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace SoftPainter.Serial
{
    /// <summary>
    /// 部署参数本地化保存
    /// </summary>
    [ProtoContract]
    public class DeployParam
    {
        [ProtoMember(1)]
        public string Type { get; set; }
        [ProtoMember(2)]
        public LocationModeEnum Location_Mode { get; set; }
        [ProtoMember(3)]
        public TimesMode Times_Mode { get; set; }
        [ProtoMember(4)]
        public int MinZ { get; set; }
        [ProtoMember(5)]
        public int MaxZ { get; set; }
        public DeployParam() { }

        public DeployParam(string _Type, LocationModeEnum _LocationMode, TimesMode _TimesMode, int _MinZ = -9999, int _MaxZ = -9999)
        {
            Type = _Type;
            Location_Mode = _LocationMode;
            Times_Mode = _TimesMode;
            MinZ = _MinZ;
            MaxZ = _MaxZ;
        }
    }
}
