using Watermelon.NET.Commons;
using UnitsType = Watermelon.NET.Commons.Units.UnitsType;

namespace Watermelon.NET.Data.Models
{
    public class User
    {
        public ulong Id { get; set; }
        public UnitsType Units { get; set; }
    }
}