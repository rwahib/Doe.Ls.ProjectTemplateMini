namespace System
{
    public static class EntityExtension
    {
        public static bool IsReferenced(this object obj)
        {
            var propList = obj.GetType().GetProperties();

            foreach (var propertyInfo in propList)
            {
                var val = propertyInfo.GetValue(obj);

                if (val == null || val.GetType().GetProperty("Count") == null) continue;

                var countValue = (int)val.GetType().GetProperty("Count").GetValue(val);

                if (countValue > 0) return true;
            }

            return false;
        }
    }
}
