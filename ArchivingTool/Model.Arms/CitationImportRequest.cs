using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivingTool.Models
{
    public class CitationImportRequest
    {
        public string Citation { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }

}
