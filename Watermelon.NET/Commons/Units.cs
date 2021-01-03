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

        public static bool TryParse(string s, out UnitsType unitsType)
        {
            s = s.ToLower();

            if (s != "metric" && s != "imperial" && s != "kelvin")
            {
                unitsType = Default;
                return false;
            }

            unitsType = Parse(s);
            
            return true;
        }

        public static UnitsType Parse(string s)
        {
            s = s.ToLower();

            if (s != "metric" && s != "imperial" && s != "kelvin")
                return Default;

            return s switch
            {
                "metric" => UnitsType.Metric,
                "imperial" => UnitsType.Imperial,
                "kelvin" => UnitsType.Standard,
                _ => Default
            };
        }
    }
}