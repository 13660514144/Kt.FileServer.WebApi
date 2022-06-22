using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedWebBrowserSolution
{
    public static class JsonHelper
    {
        /// <summary>
        /// Jobject is null?
        /// </summary>
        /// <param name="JsonName"></param>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public static bool IsJtokenExist(JToken token)
        {
            bool flg = false;            
            if (token != null)
            {
                flg = true;
            }
            return flg;
        }
        public static bool JsonIsNullOrEmpty(this JToken token)
        {
            bool flg = false;
            if ((token == null) ||
                   (token.Type == JTokenType.Array && !token.HasValues) ||
                   (token.Type == JTokenType.Object && !token.HasValues) ||
                   (token.Type == JTokenType.String && token.ToString() == String.Empty) ||
                   (token.Type == JTokenType.Null))
            {
                flg = true;
            }
            return flg;
        }

    }
}
