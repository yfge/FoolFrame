using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Data
{
    public class PropertyTypeAdaper
    {
        public static PropertyType GetPropertyType(Type type)
        {


            if (type == typeof(int))
                return PropertyType.Int;
            if (type == typeof(uint))
                return PropertyType.UInt;
            if (type == typeof(long))
                return PropertyType.Long;
            if (type == typeof(ulong))
                return PropertyType.ULong;
            if (type == typeof(bool))
                return PropertyType.Boolean;
            if (type == typeof(decimal))
                return PropertyType.Decimal;
            if (type == typeof(char))
                return PropertyType.Char;
            if (type == typeof(byte))
                return PropertyType.Byte;
            if (type == typeof(string))
                return PropertyType.String;
            if (type == typeof(DateTime))
                return PropertyType.DateTime;
            if (type.IsEnum)
                return PropertyType.Enum;
            if (type == typeof(float))
                return PropertyType.Float;
            if (type == typeof(double))
                return PropertyType.Double;
            if (type == typeof(Guid))
                return PropertyType.Guid;
            return PropertyType.BusinessObject;



        }

        public static  object GetDefaultValue(PropertyType property)
        {
            switch (property)
            {
                case PropertyType.Boolean:
                    return false;

                case PropertyType.Byte:

                    byte a = 0;
                    return a;
                case PropertyType.Char:
                    char c = '\0';
                    return c;
                case PropertyType.Date:

                case PropertyType.DateTime:
                    return DateTime.MinValue;
                case PropertyType.Decimal:
                    decimal d = 0;
                    return d;
                case PropertyType.Double:
                    double e = 0;
                    return e;
                case PropertyType.Enum:
                    int enumI =0;// property.Model.EnumValues.First().Value;
                    return enumI;
                case PropertyType.Float:
                    float f = 0;
                    return f;
                case PropertyType.Int:
                    int intresult = 0;
                    return intresult;
                case PropertyType.Long:
                case PropertyType.IdentifyId:
                    long longresult = 0;
                    return longresult;
                case PropertyType.String:
                    return "";
                case PropertyType.Time:
                    return DateTime.MinValue;
                case PropertyType.UInt:
                    uint uintResult = 0;
                    return uintResult;
                case PropertyType.ULong:
                    ulong ulongResult = 0;
                    return ulongResult;
                case PropertyType.Guid:
                    return Guid.Empty;
               
                default:
                    return null;
            }


        }
    }
}
