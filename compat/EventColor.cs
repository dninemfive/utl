using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl.compat
{
    public static class GoogleCalendar
    {
        public record EventColor
        {
            public readonly string UiName;
            public readonly string HexCode;
            public readonly string PastHexCode;
            public readonly int ApiId;
            public readonly string ApiHexCode;
            internal EventColor(string uiName, string hexCode, string pastHexCode, int apiId, string apiHexCode)
            {
                UiName = uiName;
                HexCode = hexCode;
                PastHexCode = pastHexCode;
                ApiId = apiId;
                ApiHexCode = apiHexCode;
            }
        }
        public static IReadOnlyDictionary<string, EventColor> EventColors => _eventColors;
        private static readonly Dictionary<string, EventColor> _eventColors = new()
        {
            { "Tomato", new("Tomato", "#d50000", "#f2b3b3", 11, "#dc2127") },
            {  }
        };
    }
}
