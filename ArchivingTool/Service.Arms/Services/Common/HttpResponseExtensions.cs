using ArchivingTool.Model.Arms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ArchivingTool.Service.Arms.Services.Common
{
    public static class HttpResponseExtensions
    {
        public static bool IsSuccessStatusCode<T>(this CommonResponse<T> response)
        {
            return response != null &&
                   response.StatusCode >= HttpStatusCode.OK &&
                   response.StatusCode < HttpStatusCode.MultipleChoices;
        }
    }
}
