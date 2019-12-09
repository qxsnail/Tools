using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EIP.Library
{
    public static class JsonExtension
    {
        private static readonly JsonSerializerSettings settings;

        static JsonExtension()
        {
            settings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            settings.Converters.Add(new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-dd"
            });
        }

        // TODO:
        // Object to Json 需要提供压缩可选项，默认为压缩
        public static string ToJson(this object source)
            => source == null ? null : JsonConvert.SerializeObject(source, Formatting.None, settings);

        public static string Json(this object source)
            => source == null ? null : JsonConvert.SerializeObject(source, Formatting.None, settings);

        public static T FromJson<T>(this string source)
            => source == null ? default(T) : JsonConvert.DeserializeObject<T>(source);

        public static T Object<T>(this string source)
            => source == null ? default(T) : JsonConvert.DeserializeObject<T>(source);
      
        /// <summary>
        /// 把对象转换为JSON字符串
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>JSON字符串</returns>
        public static string ToJSON(this object o)
        {
            if (o == null)
            {
                return null;
            }
            return JsonConvert.SerializeObject(o);
        }

        /// <summary>
        /// 把Json文本转为实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T FromJSON<T>(this string input)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(input);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static T ChangeType<T>(this object source)
      => source == null ? default(T) :  JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
    }

    public static class JsonHelper<T>
    {
        private static readonly JavaScriptSerializer Serializer = new JavaScriptSerializer();
        public static string ConversionJson(T objectList)
        {
            return Serializer.Serialize(objectList);
        }

        public static List<T> ConversionObjectList<T>(string jsonStr)
        {
            List<T> objs = Serializer.Deserialize<List<T>>(jsonStr);
            return objs;
        }

        public static T ConversionObj(string jsonStr)
        {
            return Serializer.Deserialize<T>(jsonStr);
        }
    }
}
