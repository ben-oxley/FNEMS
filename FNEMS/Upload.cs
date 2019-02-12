using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;

namespace FNEMS
{
    public static partial class Upload
    {
        [FunctionName("upload")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
            [Table("readings", Connection = "FNEMS")]CloudTable tableBinding,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            EnergyReading reading = new EnergyReading()
            {
                RowKey = DateTime.Now.ToString()+DateTime.Now.Millisecond,
                Electricity = ParseDoubleOrDefault(GetParameter(req, "elec")),
                Gas = ParseDoubleOrDefault(GetParameter(req, "gas")),
                Office = ToAzureKeyString(GetParameter(req,"office")),
                ReadingDate = ParseDateOrDefault(GetParameter(req, "date")),
                Comment = ToAzureKeyString(GetParameter(req, "comment"))

            };
            TableOperation updateOperation = TableOperation.Insert(reading);
            TableResult result = tableBinding.Execute(updateOperation);
            return req.CreateResponse(HttpStatusCode.OK, reading);
        }

        private static string GetParameter(HttpRequestMessage req,string param)
        {
            string name = req.GetQueryNameValuePairs()
                            .FirstOrDefault(q => string.Compare(q.Key, param, true) == 0).Value ?? "";

            return name;
        }

        private static double ParseDoubleOrDefault(string str)
        {
            double value = 0;
            double.TryParse(str, out value);
            return value;
        }

        private static DateTime ParseDateOrDefault(string str)
        {
            DateTime value = DateTime.Now;
            DateTime.TryParse(str, out value);
            if (value == DateTime.MinValue) value = DateTime.Now;
            return value;
        }

        public static string ToAzureKeyString(this string str)
        {
            var sb = new StringBuilder();
            foreach (var c in str
                .Where(c => c != '/'
                            && c != '\\'
                            && c != '#'
                            && c != '/'
                            && c != '?'
                            && !char.IsControl(c)))
                sb.Append(c);
            return sb.ToString();
        }
    }
}
