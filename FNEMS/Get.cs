using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using static FNEMS.Upload;

namespace FNEMS
{
    public static class Get
    {
        [FunctionName("Get")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Function, "get")]HttpRequestMessage req, 
            [Table("readings", Connection = "FNEMS")]IQueryable<EnergyReading> inTable, 
            TraceWriter log)
        {
            var query = from person in inTable select person;
            return req.CreateResponse(HttpStatusCode.OK, inTable.ToList());
        }

    }
}
