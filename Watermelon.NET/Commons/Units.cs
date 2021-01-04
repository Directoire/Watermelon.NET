using System;

namespace Watermelon.NET.Commons
{
    public static class Units
    {
        public enum UnitsType
        {
            Standard,
            Metric,
            Imperial
        }

        private const UnitsType Default = UnitsType.Metric;

        /// <summary>
        /// Tries parsing the provided <see cref="string"/> to a <see cref="UnitsType"/>
        /// </summary>
        /// <param name="s"></param>
        /// <param name="unitsType"></param>
        /// <returns>A boolean and out <see cref="UnitsType"/></returns>
        public static bool TryParse(string s, out UnitsType unitsType)
        {
            s = s.ToLower();

            if (!Contains(s))
            {
                unitsType = Default;
                return false;
            }

            unitsType = Parse(s);
            
            return true;
        }

        /// <summary>
        /// Parses the provided <see cref="string"/> to a <see cref="UnitsType"/>
        /// </summary>
        /// <param name="s"></param>
        /// <returns>The parsed <see cref="UnitsType"/> or the default</returns>
        public static UnitsType Parse(string s)
        {
            s = s.ToLower();

            if (!Contains(s))
                return Default;

            return s switch
            {
                "metric" => UnitsType.Metric,
                "imperial" => UnitsType.Imperial,
                "kelvin" => UnitsType.Standard,
                _ => Default
            };
        }

        /// <summary>
        /// Determines whether or not the provided <see cref="string"/> contains a units type.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool Contains(string s) =>
            s switch
            {
                "metric" => true,
                "imperial" => true,
                "kelvin" => true,
                _ => false
            };
    }
}