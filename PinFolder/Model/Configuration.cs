using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinFolder.Model
{
    class Configuration
    {
        public List<Calendar> calendar { get; set; }
        public string prev_dir { get; set; }
        public string folder_dir { get; set; }
        public bool verbose { get; set; }
        public int verbose_sleep_time { get; set; }
    }
}
