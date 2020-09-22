using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikeMgrAPI.Helpers
{
    public static class ModelBinder
    {
        public static T Bind<T>()
        {
            //Create an Instance of the Type passed
            var result = Activator.CreateInstance<T>();

            //Retrieve data from request
            var data = HttpContext.Current.Request.Params;

            //Loop Through the properties of the type
            var aType = typeof(T);
            var properties = aType.GetProperties();

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(HttpPostedFile))
                {
                    var filekey = HttpContext.Current.Request.Files.AllKeys.FirstOrDefault(k => string.Equals(k, property.Name, StringComparison.OrdinalIgnoreCase));
                    if (filekey == null) continue;
                    var file = HttpContext.Current.Request.Files.Get(filekey);
                    property.SetValue(result, file);
                    continue;
                }

                var key = data.AllKeys.FirstOrDefault(k => string.Equals(k, property.Name, StringComparison.OrdinalIgnoreCase));
                if (key == null) continue;
                var valueString = data.Get(key);
                
                if (property.PropertyType.IsAssignableFrom(typeof(DateTime)))
                {
                    DateTime value = default;
                    if (DateTime.TryParse(valueString, out value))
                    {
                        property.SetValue(result, value);
                    }
                }
                else if (property.PropertyType.IsAssignableFrom(typeof(int)))
                {
                    int value = default;
                    if (int.TryParse(valueString, out value))
                    {
                        property.SetValue(result, value);
                    }
                }
                else if (property.PropertyType.IsAssignableFrom(typeof(decimal)))
                {
                    decimal value = default;
                    if (decimal.TryParse(valueString, out value))
                    {
                        property.SetValue(result, value);
                    }
                }
                else if (property.PropertyType.IsAssignableFrom(typeof(string)))
                {
                    property.SetValue(result, valueString);
                }
            }

            return result;
        }
    }
}