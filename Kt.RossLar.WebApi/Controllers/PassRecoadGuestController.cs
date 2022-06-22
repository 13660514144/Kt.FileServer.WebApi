using HelperTools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.IO;
using System.Text;

namespace Kt.RossLar.WebApi.Controllers
{
    [Consumes("application/json", "multipart/form-data")]//此处为新增
    [ApiController]
    [Route("Api/PassRecoadGuest")]    
    public class PassRecoadGuestController : ControllerBase
    {        
        
        private IServiceProvider _ServiceProvider;
        private ILogger _ILogger;
        private string Dbconn= AppConfigurtaionServices.Configuration["ConnectionStrings:MysqlConnection"].ToString().Trim();
        public PassRecoadGuestController(ILogger<PassRecoadGuestController> logger)
        {                        
            _ILogger = logger;
        }
        /// <summary>
        /// 版本检查实体
        /// </summary>
        public class VerMsg
        { 
            public string Ver { get; set; }
            public string DeviceType { get; set; }
        }
        /// <summary>
        /// 版本更新记录实体
        /// </summary>
        public class UpVerMsg
        {
            public string CurrentVer { get; set; }
            public string UpVer { get; set; }
            public string DeviceType { get; set; }
            public string Ip { get; set; }
            public string Port { get; set; }
            public string UpFlg { get; set; }
        }
        /// <summary>
        /// 版本更新信息
        /// </summary>
        /// <param name="Paras"></param>
        /// <returns></returns>
        [HttpPost("GetVer")]
        public async Task<dynamic> GetVer([FromBody] object Paras)
        {
            dynamic JsonObj = new DynamicObj();
            string Result = string.Empty;
            string SQL=string.Empty;
            JObject Obj = new JObject();
            try
            {
                Obj = JObject.Parse(Paras.ToString());
                int FirstItem = (int)Obj["rows"];
                int EndItem = (int)Obj["pages"]+FirstItem;
                IDalMySql dal;
                MySqlDataHandle.WorkResult WordArea = new MySqlDataHandle.WorkResult();
                MySqlDataHandle Mo = new MySqlDataHandle();
                WordArea.SqlCmd = $@"
			        select  CurrentVer,UpVer,DeviceType,Ip,`Port`,UpFlg,`UpDate`,id
                        from upvermessage 
                    where 
                ";                
                
                string Dev = string.Empty;
                if (!string.IsNullOrEmpty((string)Obj["dev"]))
                {                    
                    Dev = $"  DeviceType=@DeviceType or Ip=@Ip";
                }
                if (!string.IsNullOrEmpty(Dev))
                {
                    WordArea.SqlCmd += Dev;
                    WordArea.SqlParas.ValuePara.Add(new MySqlDataHandle.SQLtype()
                    {
                        ColName = "@DeviceType",
                        ColType = Mo.ConvertMySQLType("nvarchar"),
                        ColLeng = 50,
                        ColValue = $@"{Obj["dev"]}"
                    });
                    WordArea.SqlParas.ValuePara.Add(new MySqlDataHandle.SQLtype()
                    {
                        ColName = "@Ip",
                        ColType = Mo.ConvertMySQLType("nvarchar"),
                        ColLeng = 50,
                        ColValue = $@"{Obj["dev"]}"
                    });
                }
                else
                {                    
                    WordArea.SqlCmd += $@" id>=@FirstItem and id<=@EndItem  LIMIT {Obj["pages"]}";
                    WordArea.SqlParas.ValuePara.Add(new MySqlDataHandle.SQLtype()
                    {
                        ColName = "@FirstItem",
                        ColType = Mo.ConvertMySQLType("int"),
                        ColLeng = 4,
                        ColValue = $@"{FirstItem}"
                    });
                    WordArea.SqlParas.ValuePara.Add(new MySqlDataHandle.SQLtype()
                    {
                        ColName = "@EndItem",
                        ColType = Mo.ConvertMySQLType("int"),
                        ColLeng = 4,
                        ColValue = $@"{EndItem}"
                    });
                }
                DataTable Tb = new DataTable();
                dal = DalFactory.CreateMySqlDal("mysql", Dbconn);
                Tb = dal.ListData(WordArea.SqlCmd, WordArea.SqlParas);                
                Result = Reponse("200", "1", $"查询成功", Tb);
            }
            catch (Exception ex)
            {              
                Result = Reponse("500", "0", $"查询失败", new JArray());
                _ILogger.LogInformation($"{ex.Message}");
                _ILogger.LogInformation($"{SQL}");
            }      
            return Result;
        }
        /// <summary>
        /// 版本更新记录
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        [HttpPost("UpVerMessage")]
        public async Task<string> UpVerMessage(UpVerMsg Obj)
        {
            dynamic JsonObj = new DynamicObj();
            string Result = string.Empty;
            try
            {
                IDalMySql dal;
                
                MySqlDataHandle.WorkResult WordArea = new MySqlDataHandle.WorkResult();
                MySqlDataHandle Mo = new MySqlDataHandle();
                WordArea.SqlCmd = @"
                    insert into upvermessage (CurrentVer,UpVer,DeviceType,Ip,Port,UpFlg)
                        values 
                    (@CurrentVer,@UpVer,@DeviceType,@Ip,@Port,@UpFlg)
                ";
                WordArea.SqlParas.ValuePara.Add(new MySqlDataHandle.SQLtype()
                {
                    ColName = "@CurrentVer",
                    ColType = Mo.ConvertMySQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = $@"{Obj.CurrentVer}"
                });
                WordArea.SqlParas.ValuePara.Add(new MySqlDataHandle.SQLtype()
                {
                    ColName = "@UpVer",
                    ColType = Mo.ConvertMySQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = $@"{Obj.UpVer}"
                });
                WordArea.SqlParas.ValuePara.Add(new MySqlDataHandle.SQLtype()
                {
                    ColName = "@DeviceType",
                    ColType = Mo.ConvertMySQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = $@"{Obj.DeviceType}"
                });
                WordArea.SqlParas.ValuePara.Add(new MySqlDataHandle.SQLtype()
                {
                    ColName = "@Ip",
                    ColType = Mo.ConvertMySQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = $@"{Obj.Ip}"
                });
                WordArea.SqlParas.ValuePara.Add(new MySqlDataHandle.SQLtype()
                {
                    ColName = "@Port",
                    ColType = Mo.ConvertMySQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = $@"{Obj.Port}"
                });
                WordArea.SqlParas.ValuePara.Add(new MySqlDataHandle.SQLtype()
                {
                    ColName = "@UpFlg",
                    ColType = Mo.ConvertMySQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = $@"{Obj.UpFlg}"
                });
                dal = DalFactory.CreateMySqlDal("mysql", Dbconn);
                long r = dal.ExeScalar(WordArea.SqlCmd,WordArea.SqlParas);               
                Result = Reponse("200", "1", $"版本记录更新成功", new JArray());
            }
            catch (Exception ex)
            {               
                Result = Reponse("500", "0", $"版本记录更新失败{ex.Message}", Obj);
                LogHelper.ErrorLog(ex);
            }
            _ILogger.LogInformation($"upvermsg={JsonConvert.SerializeObject(Obj)}");
            LogHelper.InfoLog($"upvermsg={JsonConvert.SerializeObject(Obj)}");
            return Result;
        }
        /// <summary>
        /// 版本检查
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        [HttpPost("ChkVer")]
        public async Task<string> ChkVer(VerMsg Obj)
        {
            _ILogger.LogInformation($"CHKVER={JsonConvert.SerializeObject(Obj)}");
            dynamic JsonObj = new DynamicObj();
            string Result = string.Empty;
            StringBuilder Sb=new StringBuilder();
            bool ChangVer = false;
            try
            {
                StreamReader sr = new StreamReader("Ver.json", System.Text.Encoding.Default);
                string line;
                while ((line = sr.ReadLine()) != null)
                {                   
                    Sb.Append(line);
                }
                sr.Close();
                sr.Dispose();
                JArray Arr = JArray.Parse(Sb.ToString());
                
                int len = Arr.Count - 1;
                for (int x = len; x>=0; x--)
                {
                    JObject J = (JObject)Arr[x];
                    if (J["DeviceType"].ToString().Trim() == Obj.DeviceType)
                    {
                        if (J["Ver"].ToString().Trim() != Obj.Ver)
                        {
                            //LINUX
                            //string Pkzip=AppDomain.CurrentDomain.BaseDirectory+$@"wwwroot/dw/{J["PackageFile"]}";
                            //WINDOWS
                            string Pkzip = AppDomain.CurrentDomain.BaseDirectory + $@"/wwwroot/dw/{J["PackageFile"]}";
                            long Isize = new FileInfo(Pkzip).Length;
                            ChangVer = true;
                            JsonObj.DeviceType = Obj.DeviceType;
                            JsonObj.Ver = J["Ver"].ToString().Trim();
                            JsonObj.PackageFile = J["PackageFile"].ToString().Trim();
                            JsonObj.FileSize = Isize;
                            JsonObj.Flg = 1;                            
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.LogInformation($"CHKVER={ex.Message}");
                LogHelper.InfoLog($"CHKVER={ex.Message}");
            }
            if (ChangVer == false)
            {
                JsonObj.DeviceType = string.Empty;
                JsonObj.Ver = string.Empty;
                JsonObj.PackageFile = string.Empty;
                JsonObj.FileSize = 0;
                JsonObj.Flg = 0;
            }
            Result= JsonConvert.SerializeObject(JsonObj._values);
            return Result;
        }
        [HttpPost("GetLastVer")]
        public async Task<string> GetLastVer([FromBody] object Paras)
        {
            dynamic JsonObj = new DynamicObj();
            String Result = string.Empty;
            string Ver = "最后一个版本号：";
            try
            {
                JObject Mo = new JObject();
                Mo = JObject.Parse($"{Paras}");
                StreamReader sr = new StreamReader("Ver.json", System.Text.Encoding.Default);
                StringBuilder Sb = new StringBuilder();
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Sb.Append(line);
                }
                sr.Close();
                sr.Dispose();
                JArray Arr = JArray.Parse(Sb.ToString());
                JObject Obj = new JObject();
                int len = Arr.Count - 1;
                for (int x = len; x >= 0; x--)
                {
                    Obj = (JObject)Arr[x];
                    if ((string)Mo["DeviceType"] == (string)Obj["DeviceType"])
                    {
                        Ver += $"{Obj["Ver"]}";
                        break;
                    }
                }               
                Result = Reponse("200", "1", $"{Ver}", new JArray());
                return Result;
            }
            catch (Exception ex)
            {
                Result=Reponse("500", "0", $"{ex.Message}", new JArray());                
            }

            return Result;
        }
        /// <summary>
        /// 文件上传
        /// DisableRequestSizeLimit  取消文件大小限制
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost("PostFile"), DisableRequestSizeLimit]
        public async Task<string> PostFile([FromForm] IFormCollection formCollection)
        {

            dynamic JsonObj = new DynamicObj();
            String Result = string.Empty;
            try
            {             
                var Ver= formCollection["Ver"];
                var DeviceType = formCollection["DeviceType"];
                string PackageFile = string.Empty;
                FormFileCollection fileCollection = (FormFileCollection)formCollection.Files;
                String name = string.Empty;
                foreach (IFormFile file in fileCollection)
                {
                    StreamReader reader = new StreamReader(file.OpenReadStream());
                    String content = reader.ReadToEnd();
                    name = file.FileName;
                    PackageFile = AppDomain.CurrentDomain.BaseDirectory + $@"/wwwroot/dw/" + name;
                    if (System.IO.File.Exists(PackageFile))
                    {
                        System.IO.File.Delete(PackageFile);
                    }
                    using (FileStream fs = System.IO.File.Create(PackageFile))
                    {
                        // 复制文件
                        file.CopyTo(fs);
                        // 清空缓冲区数据
                        fs.Flush();
                    }
                    break;
                }
                JObject O = new JObject
                {
                    new JProperty("DeviceType", $"{DeviceType}"),
                    new JProperty("Ver", $"{Ver}"),
                    new JProperty("PackageFile", $"{name}")
                };
                StreamReader sr = new StreamReader("Ver.json", System.Text.Encoding.Default);
                StringBuilder Sb = new StringBuilder();
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Sb.Append(line);
                }
                sr.Close();
                sr.Dispose();
                JArray Arr = JArray.Parse(Sb.ToString());
                Arr.Add(O);

                string filePath = $@"Ver.json";
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(JsonConvert.SerializeObject(Arr));
                        sw.Flush();
                        sw.Dispose();
                    }
                }
               
                Result = Reponse("200", "1", $"上传成功", new JArray());     
                return Result;
            }
            catch (Exception ex)
            {
                Result = Reponse("500","0", $"{ex.Message}", new JArray());                
                return Result;
            }
        }
        private string Reponse(string code,string Flg,string Message,object Data)
        {
            //dynamic JsonObj = new DynamicObj();
            restobj JsonObj = new restobj();
            JsonObj.code = Convert.ToInt32(code);
            JsonObj.Flg = Convert.ToInt32(Flg);
            JsonObj.Message = Message;
            JsonObj.Data = Data;
            return JsonConvert.SerializeObject(JsonObj);
            //return JsonConvert.SerializeObject(JsonObj._values);
        }
        public class UploadModel
        {
            public string Ver { get; set; }
            public string DeviceType { get; set; }
            public IFormFile Files { get; set; } 
        }
        public class restobj
        { 
            public int code { get; set; }
            public int Flg { get; set; }
            public string Message { get; set; }
            public object Data { get; set; }
        }
    }    
}
