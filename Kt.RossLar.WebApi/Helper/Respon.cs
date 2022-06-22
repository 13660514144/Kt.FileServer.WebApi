using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace HelperTools
{
    public class Respon
    {
        public static string ReqUrl = AppConfigurtaionServices.Configuration["PassLog"].ToString();
        public static string BuildRespon(int Code,string Message,object Data)
        {
            var Obj = new { 
                code=Code,
                message=Message,
                data=Data
            };
            return JsonConvert.SerializeObject(Obj);
        }
        public static bool ReposeHttp(DataTable Dt)
        {            
            bool Flg = false;                    
            int counter = 0;
            string Result = string.Empty;
            HttpFactoryOwner Hreq = new HttpFactoryOwner();
            try
            {
                counter = Dt.Rows.Count;
                LogHelper.InfoLog($"总记录数：{counter}");
                for (int x = 0; x < Dt.Rows.Count; x++)
                {                    
                    var parameters = new Dictionary<string, object>();
                    parameters.Add("equipmentId", Dt.Rows[x]["IdReader"].ToString());
                    parameters.Add("extra", string.Empty);
                    parameters.Add("equipmentType", "IC");
                    parameters.Add("accessType", "IC_CARD");
                    parameters.Add("accessToken", Dt.Rows[x]["iCardCode"].ToString());
                    parameters.Add("accessDate", Dt.Rows[x]["dtEventReal"].ToString().Replace("T"," ").Replace("/","-"));
                    if ((bool)Dt.Rows[x]["bReaderOut"] == false)
                    {
                        parameters.Add("wayType", "INLET");
                    }
                    else
                    {
                        parameters.Add("wayType", "OUTLET");
                    }
                    parameters.Add("description", string.Empty);
                    parameters.Add("temperature", string.Empty);
                    parameters.Add("mask", string.Empty);
                    LogHelper.InfoLog($"{JsonConvert.SerializeObject(parameters)}");
                    counter--;
                    Task.Run(async () => {
                        await Hreq.HttpWebRequestPost(ReqUrl,parameters);
                    });                   
                }
                Flg = true;
            }
            catch (Exception ex)
            {
                LogHelper.InfoLog($"剩余记录数：{counter}");
                LogHelper.ErrorLog($"send passrecord ex==>{ex}");
            }
            LogHelper.InfoLog($"剩余记录数：{counter}");
            return Flg;
        }
    }
}
