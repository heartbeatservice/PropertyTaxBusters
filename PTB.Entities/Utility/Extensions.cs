using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace PTB.Entities.Utility
{
    public static class Extensions
    {
        public static bool HasColumn(this IDataReader dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        public static List<string> ColumnList(this IDataReader dr)
        {
            var columnList = new List<string>();
            for (int i = 0; i < dr.FieldCount; i++)
            {
                columnList.Add(dr.GetName(i).ToLower());
            }
            return columnList;
        }
        public static string GetAnalyticCss(this string levelColor)
        {
            switch (levelColor)
            {
                case "#CC0404":
                    return "analyticHighRed";
                case "#FC6604":
                    return "analyticHighOrange";
                case "#FCDD04":
                    return "analyticMedimYellow";
                case "#FCFECC":
                    return "analyticLowLightYellow";
                case "#F2F2F2":
                    return "analyticLowGrey";
                default:
                    return "analyticNoColor";
            }
        }

        public static Tuple<string, string> GetLevelLabelAndColor(this int levelValue)
        {
            switch (levelValue)
            {
                case 1:
                    return Tuple.Create("Very Low", "#F2F2F2");
                case 2:
                    return Tuple.Create("Low", "#FCFECC");
                case 3:
                    return Tuple.Create("Moderate", "#FCDD04");
                case 4:
                    return Tuple.Create("High", "#FC6604");
                case 5:
                    return Tuple.Create("Very High", "#CC0404");
            }
            return null;
        }

        public static bool IsAny<T>(this IEnumerable<T> list)
        {
            return list != null && list.Any();
        }

        public static string TryGetElementValue(this XElement parentEl, string elementName, string defaultValue = null)
        {
            var foundEl = parentEl.Element(elementName);
            if (foundEl != null)
            {
                return foundEl.Value;
            }
            return defaultValue;
        }
        public static T TryGetElementValue<T>(this XElement parentEl, string elementName)
        {
            var foundEl = parentEl.Element(elementName);
            if (foundEl != null)
            {
                try
                {
                    return (T)((object)foundEl.Value);
                }
                catch
                {
                    return default(T);
                }
            }
            return default(T);
        }

        public static T TryGetElementValue<T>(this XElement parentEl, string elementName, T defaultValue) where T : class
        {
            var foundEl = parentEl.Element(elementName);
            if (foundEl != null)
            {
                return foundEl.Value as T;
            }
            return defaultValue;
        }


        public static string TryGetElementAttributeValue(this XElement parentEl, string attributeName, string defaultValue = null)
        {
            var attribute = parentEl.Attribute(attributeName);
            return attribute != null ? attribute.Value : defaultValue;
        }

        public static bool ContainsElement(this XElement parentEl, string elementName)
        {
            var foundEl = parentEl.Element(elementName);
            if (foundEl != null)
            {
                return true;
            }
            return false;
        }

        public static DateTime? ToNullableDateTime(this string val)
        {
            DateTime dt;
            if (DateTime.TryParse(val, out dt))
                return dt;

            return null;
        }
        public static int? ToNullableInt(this string val)
        {
            int parsedVal;
            if (int.TryParse(val, out parsedVal))
                return parsedVal;

            return null;
        }
        public static Stream ToStream(this string @this)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(@this);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }


        public static T ParseXML<T>(this string @this) where T : class
        {
            var reader = XmlReader.Create(@this.Trim().ToStream(), new XmlReaderSettings() { ConformanceLevel = ConformanceLevel.Document });
            return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
        }

    }
}
