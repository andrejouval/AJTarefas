using System;
using System.ComponentModel;
using System.Reflection;

namespace AJTarefasDomain.Base
{
    public static class EnumTextos
    {
        public static string GetEnumTextos(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            if (field != null)
            {
                DescriptionAttribute attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
                if (attribute != null)
                {
                    return attribute.Description;
                }
            }

            return value.ToString(); 
        }
    }
}
