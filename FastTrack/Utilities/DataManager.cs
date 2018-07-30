using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTrack.Utilities
{
    using FastTrack.Model;
    using Newtonsoft.Json;

    static class DataManager
    {
        public const String MainDataFileName = "data.json";

        public static void Save(IEnumerable<FastEntry> entries, String filepath)
        {
            System.IO.File.WriteAllText(filepath, JsonConvert.SerializeObject(entries, Formatting.Indented));
        }

        public static List<FastEntry> Load(String filepath)
        {
            if (System.IO.File.Exists(filepath))
            {
                return JsonConvert.DeserializeObject<List<FastEntry>>(System.IO.File.ReadAllText(filepath));
            }

            return new List<FastEntry>();
        }
    }
}
