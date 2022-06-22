using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kt.RossLar.WebApi.Model
{
    public class RepModels
    {
        public class Rep
        {
            public int RossId { get; set; }
            public long KtId { get; set; }
            public string Tb { get; set; }
        }
        public class RossReq
        {
            public string GUID { get; set; }
            public JObject Obj { get; set; } = new JObject();
        }
    }
}
