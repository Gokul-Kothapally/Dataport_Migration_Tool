using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivingTool.Model.Arms
{
    public class CitationImportResponse
    {
        public int CreatedCount { get; set; }
        public int UpdatedCount { get; set; }
        public int FailedCount { get; set; }
        public List<CitationImportErrorResponse> CitationImportError { get; set; } = new List<CitationImportErrorResponse>();
        public Guid HistoryId { get; set; }  
    }
}
