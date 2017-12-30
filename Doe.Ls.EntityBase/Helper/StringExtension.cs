using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace System
{
    public static class StringExtension
    {
        const string invalidCharacters = ":;.’\\-~!@#$%^&=+-*/()[]{}\"\n\r\t\v\f\r ";
        public static string ToValue(this IEnumerable<string> input, char delimiter = ',')
        {
            var sb = new StringBuilder();
            foreach (var s in input)
            {
                sb.Append(s);
                sb.Append(delimiter);
                sb.Append(" ");
            }
            return sb.ToString().Trim().TrimEnd(delimiter);
        }

        public static int ExtractNumber(this string input)
        {
            var result = Regex.Replace(input, @"[^\d]", "");
            int resultNumber;

            return int.TryParse(result, out resultNumber) ? resultNumber : int.MinValue;

        }

        public static bool IsUri(this string input)
        {
            Uri test = null;
            return Uri.TryCreate(input, UriKind.Absolute, out test);

        }

        public static string Wordify(this string value)
        {
            if (value == null)
                return string.Empty;
            var fixedStardPatterns = new[] { "DoE", "HR" };
            foreach (var fixedStardPattern in fixedStardPatterns)
            {
                value = value.Trim();
                if (value.Equals(fixedStardPattern))
                {
                    return fixedStardPattern;
                }
                if (value.StartsWith(fixedStardPattern))
                {

                    return $"{fixedStardPattern} " + Wordify(value.Remove(0, fixedStardPattern.Length)).ToLower();
                }
            }


            var formatted = Regex.Replace(value, "([A-Z][a-z]?)", " $1").Trim();
            var lower = formatted.Replace("_", " ").Replace("-", " ").ToLower();
            var firstLetter = formatted.Substring(0, 1);
            var rest = lower.Substring(1, formatted.Length - 1);

            return firstLetter + rest;


        }
        public static string StripHtml(this string value)
        {
            if (value == null)
                return string.Empty;
            var noHtml = Regex.Replace(value, @"<[^>]+>|&nbsp;", "").Trim();
            var noHtmlNormalised = Regex.Replace(noHtml, @"\s{2,}", " ");
            return noHtmlNormalised;
        }
        public static string CamelCase(this string value)
        {
            if (value != null)
            {
                var sb = new StringBuilder();
                sb.Append(value[0].ToString().ToLower());
                sb.Append(value.Substring(1, value.Length - 1));
                return sb.ToString();
            }
            return string.Empty;
        }

        public static string ToTitleCase(this string value)
        {
            return value != null ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Wordify(value)) : string.Empty;
        }

        public static string PaskalCase(this string value)
        {
            if (value != null)
            {
                var wordiFied = Wordify(value);
                wordiFied = wordiFied.Replace("-", string.Empty).Replace("_", string.Empty);
                var sb = new StringBuilder();
                sb.Append(wordiFied[0].ToString().ToUpper());
                sb.Append(wordiFied.Substring(1, wordiFied.Length - 1));
            }
            return string.Empty;
        }

        public static string WordifyOld(this string value)
        {
            if (value == null)
                return string.Empty;
            value = value.Trim();
            if (value.Equals("DoE"))
            {
                return "DoE";
            }
            else if (value.StartsWith("DoE"))
            {

                return $"DoE " + WordifyOld(value.Remove(0, 3));
            }
            else
            {
                return value != null ? Regex.Replace(value, "([A-Z][a-z]?)", " $1").Trim() : string.Empty;
            }
        }

        public static string Minimise(this string value, int max = 24, string more = "...")
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;
            if (value.Length > max) return value.Substring(0, max) + more;
            return value;
        }

        public static string NewLineToBr(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            return value.Replace("\r\n", "</br>").Replace("\n\r", "</br>").Replace("\n", "</br>").Replace("\r", "</br>");
        }

        public static string CleanFilename(this string fileName)
        {
            const string invalidCharactersForFileNames = "/\\~!@#$%^&*()[]=+}{:\" +";
            foreach (char invalidCharacter in invalidCharactersForFileNames)
            {
                fileName = fileName.Replace(invalidCharacter, '_');
            }
            return fileName;

        }
        public static string CleanFromControlCharacters(this string fileName)
        {
            const string invalidCharacters = "’\"\n\r\t\v\f\r";
            foreach (char invalidCharacter in invalidCharacters)
            {
                fileName = fileName.Replace(invalidCharacter, '_');
            }
            return fileName;

        }

        public static string CleanPropertyName(this string variable)
        {
            variable = variable.StripHtml().Wordify();

            foreach (char invalidCharacter in invalidCharacters)
            {
                variable = variable.Replace(invalidCharacter, ' ');
            }
            if (variable.Length > 0)
                if ("123456789".Contains(variable[0].ToString())) // if first character is number remove it
                {
                    variable = variable.Substring(1);
                }

            TextInfo info = Thread.CurrentThread.CurrentCulture.TextInfo;
            variable = info.ToTitleCase(variable);
            string[] parts = variable.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
            string result = String.Join(String.Empty, parts);
            return result;

        }
        public static string CleanVariable(this string variable)
        {
            foreach (char invalidCharacter in invalidCharacters)
            {
                variable = variable.Replace(invalidCharacter, ' ');
            }
            if (variable.Length > 0)
                if ("123456789".Contains(variable[0].ToString())) // if first character is number remove it
                {
                    variable = variable.Substring(1);
                }

            string[] parts = variable.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
            string result = string.Join(String.Empty, parts);
            return result;

        }

        public static string CleanEntityName(this string entityName)
        {
            foreach (char invalidCharacter in invalidCharacters)
            {
                entityName = entityName.Replace(invalidCharacter.ToString(), "");
            }
            return entityName;

        }
        public static bool IsOn(this string formField)
        {
            return !string.IsNullOrWhiteSpace(formField) && formField.Equals("on", StringComparison.InvariantCultureIgnoreCase);
        }

        public static string FileSizeWithUnits(this long size)
        {
            if (size / (1024 * 1024 * 1024) > 0)
            {
                var dob = ((double)size) / ((double)1024 * 1024 * 1024);
                var fraction = Math.Round(dob, 2);
                return $"{fraction}GB";
            }

            if (size / (1024 * 1024) > 0)
            {
                var dob = ((double)size) / ((double)1024 * 1024);
                var fraction = Math.Round(dob, 2);

                return $"{fraction} MB";
            }

            if (size / (1024) > 0)
            {
                var dob = ((double)size) / ((double)1024);
                var fraction = Math.Round(dob, 2);
                return $"{fraction} KB";
            }

            return $"{size} b";


        }
        public static string FileSizeWithUnits(this int size)
        {

            return FileSizeWithUnits((long)size);


        }

        public static bool IsNullOrWhiteSpaceOrNa(this string val)
        {

            return string.IsNullOrWhiteSpace(val) || val.Equals("NA") || val.Equals("N/A");


        }
        public static bool IsNullOrWhiteSpaceOrPleaseProvide(this string val)
        {

            return string.IsNullOrWhiteSpace(val) || val.Equals("[Please provide]");


        }

        public static string InitFirstWordOnly(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            var words = value.Split(' ');
            var sb = new StringBuilder();
            sb.Append(words[0].Substring(0, 1).ToUpper());
            sb.Append(words[0].Substring(1).ToLower());
            for (var i = 1; i < words.Length; i++)
            {
                sb.Append(" " + words[i].ToLower());
            }
            return sb.ToString();
        }

        public static string GetChecked(this bool check)
        {
            return check ? "checked" : string.Empty;
        }

        public static string GetChecked(this bool? check)
        {
            return check.HasValue ? GetChecked(check.Value) : string.Empty;
        }
        /// <summary>
        /// Check if any of the elements has any value 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool HasAnyValue(this IEnumerable<string> input)
        {
            if (input == null) return false;
            foreach (var s in input)
            {
                if (!string.IsNullOrWhiteSpace(s)) return true;
            }
            return false;
        }

        public static List<int> CastToIntegerList(this IEnumerable<string> list)
        {
            if (list == null) return null;
            return list.Select(item => item.ToInteger()).ToList();
        }
        /// <summary>
        /// this method will parse the multiple values that come in any ajax params
        /// </summary>
        public static List<string> ToParamList(this IEnumerable<string> list)
        {
            if (list == null) return null;
            var tmp = new List<string>();

            foreach (var param in list)
            {
                foreach (var p in param.Split(','))
                {
                    if(!string.IsNullOrWhiteSpace(p)&&!tmp.Contains(p))tmp.Add(p);
                }
            }

            return tmp;
        }
        
        }
}
