using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Adbp.Extensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// 返回值示例，ENUM_Result_Success
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static string SouceName<TEnum>(this TEnum value)
            where TEnum: Enum
        {
            //ENUM_Result_Success
            return $"ENUM_{typeof(TEnum).Name}_{value.ToString()}";
        }

        public static List<TEnum> GetFlags<TEnum>(this TEnum value)
            where TEnum : Enum
        {
            return GetEnums<TEnum>().Where(x => value.HasFlag(x)).ToList();
        }

        public static IEnumerable<TEnum> GetEnums<TEnum>()
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }
    }
}
