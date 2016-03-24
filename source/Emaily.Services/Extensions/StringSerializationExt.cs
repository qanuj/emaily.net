using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Emaily.Services.Extensions
{
    public static class StringSerializationExt
    {
        public static T To<T>(this string value, bool camel = false)
        {
            if (string.IsNullOrWhiteSpace(value)) return default(T);
            if (!camel) return JsonConvert.DeserializeObject<T>(value);
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static dynamic ToDynamic(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return JsonConvert.DeserializeObject<dynamic>(value);
        }

        public static string ToCamel(this string value)
        {
            return char.ToUpper(value[0]) + value.Substring(1);
        }
        public static string Random(this string value, int maxChars = 8)
        {
            return "";
        }

        public static string ToJson(this object value, bool camel = false)
        {
            return camel ? JsonConvert.SerializeObject(value, Camel()) : JsonConvert.SerializeObject(value);
        }

        public static string ToJson(this IDictionary<string, string> value)
        {
            return JsonConvert.SerializeObject(value, new KeyValuePairConverter());
        }

        public static Dictionary<string, string> ToDictionary(this string value)
        {
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (string l in value.Split('&'))
            {
                string line = l.Trim();
                int equalPox = line.IndexOf('=');
                if (equalPox >= 0)
                    values.Add(line.Substring(0, equalPox), line.Substring(equalPox + 1));
            }
            return values;
        }
        public static string TryGet(this Dictionary<string, string> values, string name)
        {
            string val; values.TryGetValue(name, out val);
            return val;
        }
        public static decimal TryGetCurrency(this Dictionary<string, string> values, string name)
        {
            decimal total = decimal.Zero;
            try { total = decimal.Parse(values[name], new CultureInfo("en-US")); } catch { }
            return total;
        }


        public static string ToGravatar(this string email)
        {
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(email));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(i.ToString("x2"));
            }
            return string.Format("http://www.gravatar.com/avatar/{0}", sBuilder);
        }


        private static JsonSerializerSettings Camel()
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            return jsonSerializerSettings;
        }
    }
}
