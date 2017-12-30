using System.Configuration;

namespace Doe.Ls.EntityBase.Helper {
    public static class ConfigurationHelper {

        public static T GeConfigSection<T>(this string sectionName) where T : class {

            return ConfigurationManager.GetSection(sectionName) as T;

        }

    }
}
