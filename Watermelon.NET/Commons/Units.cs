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

        public static bool TryParse(string s, out UnitsType unitsType)
        {
            s = s.ToLower();

            if (s != "metric" && s != "imperial" && s != "standard")
            {
                unitsType = UnitsType.Standard;
                return false;
            }

            unitsType = s switch
            {
                "metric" => UnitsType.Metric,
                "imperial" => UnitsType.Imperial,
                "standard" => UnitsType.Standard,
                _ => UnitsType.Standard
            };
            return true;
        }

        public static UnitsType Parse(string s)
        {
            s = s.ToLower();

            if (s != "metric" && s != "imperial" && s != "standard")
                return UnitsType.Standard;

            return s switch
            {
                "metric" => UnitsType.Metric,
                "imperial" => UnitsType.Imperial,
                "standard" => UnitsType.Standard,
                _ => UnitsType.Standard
            };
        }
    }
}