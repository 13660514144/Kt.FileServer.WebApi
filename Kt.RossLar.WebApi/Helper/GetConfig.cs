using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelperTools
{
    public class GetConfig
    {
        public static JObject GetLastId(string Path)
        {
            JObject O = new JObject();
            try
            {
                //ReloadOnChange = true 当appsettings.json被修改时重新加载            
                string Cfg = FileHelper.ReadFileLine(Path);
                O = JObject.Parse(Cfg);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog($"配置文件获取失败", ex);
            }
            return O;
        }
    }
}
