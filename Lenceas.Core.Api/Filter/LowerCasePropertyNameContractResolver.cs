using Newtonsoft.Json.Serialization;

namespace Lenceas.Core.Api
{
    public class LowerCasePropertyNameContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            // 全部转小写
            return propertyName.ToLower();
        }
    }
}