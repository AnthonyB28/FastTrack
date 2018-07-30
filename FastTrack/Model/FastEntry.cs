using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTrack.Model
{
    class FastEntry
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<float> Weights { get; set; } = new List<float>();
    }
}
