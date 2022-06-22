using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperTools
{
    public interface IHttpFactoryOwner
    {
        Task<string> GetFactory(Dictionary<string, string> parameters, string requestUri);
        Task<string> PostFactory(List<KeyValuePair<string, string>> Json, string requestUri);
    }
}
