using System;
using System.Collections.Generic;
using System.Reflection;

namespace SimpleFollow.Helpers
{
    public class ToStringReflector
    {
        public static string GetObjectString(object obj)
        {
            if (obj == null)
                return "";
            string output = "";
            Type t = obj.GetType();
            foreach (var property in t.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                output += property.Name + "=" + property.GetValue(obj, null) + " ";
            }
            foreach (var field in t.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                output += field.Name + "=" + field.GetValue(obj) + " ";
            }
            return output;
        }
    }
}
