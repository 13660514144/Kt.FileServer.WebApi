using HelperTools;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kt.RossLar.WebApi.Model
{
    public class ConFigHelper
    {
        public CfgModel _CfgModel;
        public ConFigHelper()
        {
            
        }
        public void Init()
        {
            _CfgModel = new CfgModel();
            //JObject Lid = GetConfig.GetLastId(@"LastRecord.json");
            //_CfgModel.LastId= (long)Lid["lastid"]; 
            _CfgModel.JobSleep= Convert.ToInt32(AppConfigurtaionServices.Configuration["JobSleep"]);
            _CfgModel.QueueSleep = Convert.ToInt32(AppConfigurtaionServices.Configuration["QueueSleep"]);            
        }
        public class CfgModel
        {
            public string CurrentPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
            public int JobSleep { get; set; } = 0;
            public int QueueSleep { get; set; } = 0;
            public long LastId { get; set; } = 0;
        }
        public bool InitCommunication()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog($"err:{ex.Message}");
                return false;
            }

        }
    }
}
