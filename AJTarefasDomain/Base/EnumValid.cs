using AJTarefasDomain.Projeto;
using System;
using System.ComponentModel;
using System.Reflection;

namespace AJTarefasDomain.Base
{
    public static class EnumValid
    {
        public static bool IsValid(this Enum value)
        {
            if (!Enum.IsDefined(value.GetType(), value))
            {
                return false;
            }

            return true;
        }
    }
}
