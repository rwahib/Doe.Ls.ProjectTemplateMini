using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace System
{
    public static class CustomConverter
    {
        public static decimal ConvertTimeToHoursDecimal(this string time)
        {
            var timeFormat = time.Split(':');

            if (timeFormat.Count() < 2) return 0;

            var hours = Convert.ToInt32(timeFormat[0]);
            var minutes = (double)Convert.ToInt32(timeFormat[1]) / 60;

            return Convert.ToDecimal(hours + minutes);
        }



        /// <summary/>
        /// Converting hh:mm format to decimal
        /// <remarks/> 


        public static decimal ConvertTimeToDecimal(this string time)
        {
            var timeFormat = time.Split(':');

            if (timeFormat.Count() != 2) return 0;

            var hours = Convert.ToInt32(timeFormat[0]);
            var minutes = (double)Convert.ToInt32(timeFormat[1]) / 60;

            return Convert.ToDecimal(hours + minutes);
        }

        /// <summary>
        /// Converting decimal to hh:mm format        
        /// </summary>
        public static string ConvertDecimalToTime(this decimal dHours)
        {
            var secs = dHours * 60 * 60;
            var hours = Math.Floor(secs / 3600);
            var minutes = Math.Ceiling((secs - (hours * 3600)) / 60);

            return $"{hours}:{minutes.ToString("00")}";
        }

        public static string ToUniversalDateFormat(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        public static string ToUniversalDateFormat(this DateTime? dt)
        {
            if (!dt.HasValue) return string.Empty;
            return ToUniversalDateFormat(dt.Value);
        }

        public static string ToEasyDateFormat(this DateTime dt)
        {
            return dt.ToString("dd-MMM-yyyy");
        }

        public static string ToEasyDateTimeFormat(this DateTime dt)
        {
            return dt.ToString("dd-MMM-yyyy HH:mm:ss");
        }

        public static string ToEasyTimeFormat(this DateTime dt)
        {
            return dt.ToString("HH:mm:ss");
        }

        public static string ToEasyDateFormat(this DateTime? dt)
        {
            return dt.HasValue ? ToEasyDateFormat(dt.Value) : string.Empty;
        }

        public static string ToEasyDateTimeFormat(this DateTime? dt)
        {
            return dt.HasValue ? ToEasyDateTimeFormat(dt.Value) : string.Empty;
        }

        public static string ToEasyTimeFormat(this DateTime? dt)
        {
            return dt.HasValue ? ToEasyTimeFormat(dt.Value) : string.Empty;
        }

        public static string ToUniversalDateTimeFormat(this DateTime dt)
        {

            return dt.ToString("yyyy-MM-dd HH:mm");
        }

        public static string ToICalLocalDateTimeFormat(this DateTime dt)
        {
            return dt.ToString("yyyyMMddTHHmmss");
        }

        public static string ToDisplayTimeFormat(this TimeSpan ts)
        {

            return (DateTime.Today.Add(ts)).ToString("HH:mm");
        }

        public static DateTime ToTimeSpan(this string ts)
        {
            DateTime startTime;
            DateTime.TryParseExact(ts, "HH:mm",
                CultureInfo.CurrentCulture, DateTimeStyles.None, out startTime);
            return startTime;
        }

        public static bool ToBoolean(this string booleanString)
        {
            if (string.IsNullOrWhiteSpace(booleanString)) return false;
            return booleanString.StartsWith("t", StringComparison.InvariantCultureIgnoreCase);
        }

        public static int ToInteger(this string intString, int defaultValue = int.MinValue)
        {
            int x;
            if (int.TryParse(intString, out x))
            {
                return x;
            }
            return defaultValue;
        }
        public static decimal ToDecimal(this string decimalString, decimal defaultValue = decimal.MinValue)
        {
            decimal x;
            if (decimal.TryParse(decimalString, out x))
            {
                return x;
            }
            return defaultValue;
        }
        public static string ToMoneyFormat(this decimal rate)
        {

            return $"{rate:C}";
        }
        public static string ToUniversalDateTimeFormat(this DateTime? dt)
        {
            if (!dt.HasValue) return string.Empty;
            return ToUniversalDateTimeFormat(dt.Value);
        }

        public static void UpdateProperties(object source, object destination, bool ignoreNullValues, string[] execludeNames = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (destination == null) throw new ArgumentNullException(nameof(destination));

            foreach (var pInfo in destination.GetType().GetProperties())
            {
                if (pInfo.GetSetMethod() != null && !pInfo.GetSetMethod().IsStatic)
                {
                    if (pInfo.PropertyType.Name.ToLower().Contains("icollection") || (execludeNames != null && execludeNames.Contains(pInfo.Name))) continue;

                    if (ignoreNullValues)
                    {
                        var destinationValue = pInfo.GetValue(destination, null);

                        if (destinationValue != null)
                        {
                            pInfo.SetValue(source, pInfo.GetValue(destination, null), null);
                        }
                    }
                    else
                    {
                        pInfo.SetValue(source, pInfo.GetValue(destination, null), null);
                    }
                }
            }
        }

        public static TTarget To<TSource, TTarget>(this TSource sourceObject, TTarget targetObject, bool ignoreNullValues = false) where TTarget : class, new()
        {
            if (sourceObject == null || targetObject == null) return null;

            foreach (var targetProperty in typeof(TTarget).GetProperties())
            {
                var name = targetProperty.Name;

                var sourceProperty = typeof(TSource).GetProperties().FirstOrDefault(p => p.Name == name);
                if (sourceProperty == null) continue;


                var sourcePropertyValue = sourceProperty.GetValue(sourceObject, BindingFlags.Default, null, null, null);
                if (!ignoreNullValues && sourcePropertyValue == null) continue;

                if (targetProperty.PropertyType.Name.ToLower().Contains("string"))
                {
                    targetProperty.SetValue(targetObject, sourcePropertyValue.ToString(), BindingFlags.Default, null, null, null);
                }
                else
                {
                    targetProperty.SetValue(targetObject, sourcePropertyValue, BindingFlags.Default, null, null, null);

                }

            }

            return targetObject;
        }

        public static TTarget JsonDeserialise<TTarget>(this string json, bool escapeCharactersBeforePars = false)
        {
            if (escapeCharactersBeforePars)
            {
                json = EscapeStringValueForJson(json);
            }
            var result = JsonConvert.DeserializeObject<TTarget>(json);
            return result;
        }

        public static string JsonSerialise<TTarget>(this TTarget @object, Formatting formatting = Formatting.Indented)
        {
            var result = JsonConvert.SerializeObject(@object, formatting);
            return result;
        }


        public static string GetEnumDescription(Enum value)
        {
            var attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }

        private static string EscapeStringValueForJson(string value)
        {
            const char BACK_SLASH = '\\';
            const char SLASH = '/';
            const char DBL_QUOTE = '"';

            var output = new StringBuilder(value.Length);
            foreach (char c in value)
            {
                switch (c)
                {
                    case SLASH:
                        output.AppendFormat("{0}{1}", BACK_SLASH, SLASH);
                        break;

                    case BACK_SLASH:
                        output.AppendFormat("{0}{0}", BACK_SLASH);
                        break;

                    case DBL_QUOTE:
                        output.AppendFormat("{0}{1}", BACK_SLASH, DBL_QUOTE);
                        break;

                    default:
                        output.Append(c);
                        break;
                }
            }

            return output.ToString();
        }
    }
}
