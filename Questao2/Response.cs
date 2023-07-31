using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao2
{
    public class Response
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public int Total { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }
        public Data[] Data { get; set; }
    }
}
