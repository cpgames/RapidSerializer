using System.Collections;
using System.IO;
using System.Text;

namespace cpGames.RapidSerializer
{
    public partial class RapidSerializer
    {
        #region Methods
        public static void SerializeToFile(object item, string filePath, SerializationMaskType mask = SerializationMaskType.Everything)
        {
            var dataStr = SerializeToString(item, mask);
            using (var file = new StreamWriter(filePath))
            {
                file.Write(dataStr);
            }
        }

        public static T DeserializeFromFile<T>(string filePath)
        {
            string dataStr;
            using (var file = new StreamReader(filePath))
            {
                dataStr = file.ReadToEnd();
            }
            var data = DeserializeFromString<T>(dataStr);
            return Deserialize<T>(data);
        }

        public static string SerializeToString(object item, SerializationMaskType mask = SerializationMaskType.Everything)
        {
            var data = Serialize(item, mask);
            var dataStr = new StringBuilder();
            foreach (var key in data.Keys)
            {
                ObjectToString(dataStr, key, data[key], 0);
            }
            return dataStr.ToString();
        }

        private static void ObjectToString(StringBuilder dataStr, object key, object data, int nestingLevel)
        {
            if (data == null)
            {
                return;
            }

            var indent = new string('\t', nestingLevel);
            var lineStr = string.Format("{0}{1}:{2}:", indent, key, data.GetType());

            switch (data)
            {
                case IDictionary dictionary:
                {
                    dataStr.AppendLine(lineStr);
                    foreach (var dKey in dictionary.Keys)
                    {
                        ObjectToString(dataStr, dKey, dictionary[dKey], nestingLevel + 1);
                    }
                    break;
                }
                case IList list:
                {
                    dataStr.AppendLine(lineStr);
                    foreach (var val in list)
                    {
                        ObjectToString(dataStr, string.Empty, val, nestingLevel + 1);
                    }
                    break;
                }
                default:
                    dataStr.AppendLine(string.Format("{0}{1}", lineStr, data));
                    break;
            }
        }

        private static T DeserializeFromString<T>(string dataStr)
        {
            var p = new TextParser(dataStr);
            var data = p.StringToDictionary();
            return Deserialize<T>(data);
        }
        #endregion
    }
}