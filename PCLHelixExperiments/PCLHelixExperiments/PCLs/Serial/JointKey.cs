using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace SoftPainter.Serial
{
    /// <summary>
    /// 复合主键（最多5个）
    /// </summary>
    [ProtoContract]
    public class JointKey
    {
        /// <summary>
        /// 3号主键：类别（Door|Furniture）
        /// </summary>
        [ProtoMember(3)]
        public string Category { get; set; }
        /// <summary>
        /// 4号主键：类型（ac-002|ac-020等）
        /// </summary>
        [ProtoMember(4)]
        public string Type { get; set; }
        /// <summary>
        /// 5号主键（清油-clear|混油-mix）或（正面-facade|反面-reverse）
        /// </summary>
        [ProtoMember(5)]
        public string PaintType { get; set; }

        public override bool Equals(object obj)
        {
            if (null == obj)
                return false;
            if (obj.GetType() != this.GetType())
                return false;

            JointKey old = obj as JointKey;
            bool flag1 = old.Category == this.Category;
            bool flag2 = old.Type == this.Type;
            bool flag3 = old.PaintType == this.PaintType;
            return flag1 && flag2 && flag3;
        }
        public override int GetHashCode()
        {
            return this.Category.GetHashCode() + this.Type.GetHashCode() + this.PaintType.GetHashCode();
        }

        public string FileName { get { return string.Format("{0}_{1}_{2}.pb", Category, Type, PaintType); }}
        public TimesMode ModeTimes
        {
            get
            {
                if (Category != ConstString.CATEGORY_Furniture)
                    throw new Exception("Category not Furniture error");
                switch (PaintType)
                {
                    case ConstString.PAINTTYPE_Furniture_Single: return TimesMode.Single;
                    case ConstString.PAINTTYPE_Furniture_Double_Front:
                    case ConstString.PAINTTYPE_Furniture_Double_Back: return TimesMode.Double;
                    default: throw new Exception("JointKey.ModelTimes error");
                }
            }
        }


        public JointKey(JointKey old)
        {
            this.Category = old.Category;
            this.Type = old.Type;
            this.PaintType = old.PaintType;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="category">ConstString.CATEGORY_*</param>
        /// <param name="type">AI识别类型</param>
        /// <param name="painttype">ConstString.PAINTTYPE_*</param>
        public JointKey(string category, string type, string painttype)
        {
            Category = category;
            Type = type;
            PaintType = painttype;
        }

        public JointKey(string category, string type, OilType oilType)
        {
            Category = category;
            Type = type;
            PaintType = PaintTypeFormat(oilType);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="category">ConstString.CATEGORY_*</param>
        /// <param name="type">AI识别类型</param>
        /// <param name="times">单面|双面</param>
        /// <param name="face">正面|反面</param>
        public JointKey(string category, string type, TimesMode times, Face face)
        {
            Category = category;
            Type = type;
            PaintType = PaintTypeFormat(times, face);
        }

        public JointKey() { }

        public static string PaintTypeFormat(OilType oilType)
        {
            switch (oilType)
            {
                case OilType.Clear: return ConstString.PAINTTYPE_DOOR_Clear;
                case OilType.Mixed: return ConstString.PAINTTYPE_DOOR_Mix;
                default: throw new Exception("PaintTypeFormat error");
            }
        }

        public static string PaintTypeFormat(TimesMode times, Face face)
        {
            switch (times)
            {
                case TimesMode.Single: return ConstString.PAINTTYPE_Furniture_Single;
                case TimesMode.Double:
                    switch (face)
                    {
                        case Face.Front: return ConstString.PAINTTYPE_Furniture_Double_Front;
                        case Face.Back: return ConstString.PAINTTYPE_Furniture_Double_Back;
                        default: throw new Exception("PaintTypeFormat error");
                    }
                default: throw new Exception("PaintTypeFormat error");
            }
        }
    }
}
