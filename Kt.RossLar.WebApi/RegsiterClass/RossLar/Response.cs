using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kt.RossLar.WebApi.Model.RepModels;

namespace RollLar
{
    public class ResponseTo
    {

        public Dictionary<string,List<Rep>> ListIDmatching ;
        public List<RossReq> Listreq ;
        public List<Rep> _Listrep;
        public ResponseTo()
        {
        
        }
        public void Init()
        {
            ListIDmatching = new Dictionary<string, List<Rep>>();
            Listreq = new List<RossReq>();
            _Listrep = new List<Rep>();
        }
        public void AddRep(string tb, int RossID, long KtID)
        {
            Rep _Rep = new Rep();
            _Rep.Tb = tb;
            _Rep.RossId = RossID;
            _Rep.KtId = KtID;
            _Listrep.Add(_Rep);
        }
        public void AddRossReq(string GUID, JObject oo)
        {
            RossReq O = new RossReq
            {
                GUID=GUID,
                Obj=oo
            };
            Listreq.Add(O);
        }
        public void SetDict(string Guid)
        {
            ListIDmatching.Add(Guid, _Listrep);
            _Listrep.Clear();
        }
    }
}
