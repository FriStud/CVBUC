using System;
using System.Collections.Generic;
using System.Linq;


namespace CatelVisBeh.Utils
{
    #region EnumUtil static member

    /// <summary>
    /// Generic util for enumeration through an Enum, should be faster then explicit castig and unboxig/boxing.
    /// </summary>
    public static class EnumUtil
    {
        public static IEnumerable<T> GetValues<T>()
        {
            if (typeof(T).BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        #region UseFull for User
        /// <summary>
        /// If not found it returns default Enum type with value 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetEnumType<T>(int value)
        {
            if (typeof(T).BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            foreach (var item in GetValues<T>())
                if (Convert.ToInt32(item) == value)
                    return item;

            return default(T);
        }

        /// <summary>
        /// If not found it returns default Enum type with value 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetEnumType<T>(string value)
        {
            if (typeof(T).BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            if (!Checker.IsOkay(value))
                return default(T);

            if (int.TryParse(value, out int isNum))
                return GetEnumType<T>(isNum);

            foreach (var item in GetValues<T>())
                    if (item.ToString().ToLower() == value.ToLower())
                        return item;

            return default(T);
        }
        #endregion UseFull for User
    }

    #endregion EnumUtil static member
    
}
