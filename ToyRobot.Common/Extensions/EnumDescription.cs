using System.ComponentModel;

namespace ToyRobot.Common.Extensions
{
    public static partial class Extensions
    {
        public static string EnumDescription(this Enum value)
        {
            var valueType = value.GetType();
            var field = valueType.GetField(value.ToString());
            if (field == null)
                return string.Empty;
            var attributeObj = field.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
            if (attributeObj == null)
                return string.Empty;
            if (attributeObj is not DescriptionAttribute attribute)
                return string.Empty;
            return attribute.Description;
        }
        public static string EnumDescription(this Enum value, params object?[] values)
        {
            var description = EnumDescription(value);
            if(description!=null)
            {
                description = string.Format(description, values);
            }
            return description??string.Empty;
        }
    }
}