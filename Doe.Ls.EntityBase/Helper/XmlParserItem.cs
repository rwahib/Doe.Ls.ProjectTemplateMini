using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Doe.Ls.EntityBase.Helper
{
    public class XmlParserItem
    {
        [XmlIgnore]
        public XmlParserClass Parent { get; set; }
        public string PropertyName { get; set; }

        public string PropertyPath
        {
            get
            {
                return Path();
            }
        }

        
        public PropertyType PropertyType { get; set; }

        public string PropertyExpression
        {
            get
            {
                switch (PropertyType)
                {
                    case PropertyType.None:
                    case PropertyType.Key:
                    case PropertyType.Name:
                        return
                            $"(ConfigurationManager.GetSection(@\"{PropertyPath}\") as NameValueCollection)[\"{PropertyName}\"].Trim()";

                    case PropertyType.AppSettings: return
                        $"ConfigurationManager.AppSettings[@\"{PropertyName}\"].Trim()";
                    case PropertyType.ConnectionString: return
                        $"ConfigurationManager.ConnectionStrings[@\"{PropertyName}\"].ConnectionString.Trim()";
                    case PropertyType.HttpRuntime: return
                        $"(ConfigurationManager.GetSection(@\"system.web/httpRuntime\") as HttpRuntimeSection).{(PropertyName.ToLower().Contains("max") ? "MaxRequestLength" : "ExecutionTimeout")}";

                }
                
                return "Coming soon :)";
            }
        }

        public override string ToString()
        {
            return $"{PropertyName}-{PropertyExpression}";
        }
        private string Path()
        {
            var pathExpression = new List<String>();
            GetPath(this.Parent, pathExpression);
            var sb = new StringBuilder();
            pathExpression.Reverse();
            foreach (var val in pathExpression)
            {
                sb.Append(val);
                if (val != pathExpression.Last()) sb.Append("/");

            }
            return sb.ToString();
        }

        private void GetPath(XmlParserClass @class, List<String> exp)
        {
            exp.Add(@class.ClassName);
            if (@class.Parent != null) GetPath(@class.Parent, exp);
        }
    }
}