using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace NetCorePerformanceTest
{
    /// <summary>
    /// 衍生函式
    /// </summary>
    public static class ExtensionMethods
    {
        public static T DeepCloneExpressionTree<T>(this T a)
        {
            if (a == null)
                return default(T);
            return (T)DeepCopyByExpressionTrees.DeepCopyByExpressionTree<T>(a);
        }

        public static T DeepCloneBinarySerializer<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }


        /// <summary>
        /// 深度复制元件
        /// 元件必须能够序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <returns></returns>
        public static T DeepCloneJSON<T>(this T a)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(a);
            return (T)Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
    }
}
