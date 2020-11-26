using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.BlobServices
{
    internal class Guard
    {
        public static void NotNull(object obj, string argName)
        {
            if (obj == null)
                throw new ArgumentNullException(argName);
        }
        public static void PropertyNotNull(object obj, string propertyPath)
        {
            if (obj == null)
                throw new ArgumentNullException(propertyPath);
        }
        public static void NotNullOrEmpty(string value, string argName)
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(argName);
        }
        public static void PropertyNotNullOrEmpty(String value, String propertyPath)
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(propertyPath);
        }
        public static void IsTrue<T>(Boolean value) where T : Exception, new()
        {
            if (!value)
                throw new T();
        }
        public static void IsTrue<T>(Boolean value, T error) where T : Exception
        {
            if (!value)
                throw error;
        }
    }
}
