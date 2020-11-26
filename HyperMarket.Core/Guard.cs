using System;
namespace HyperMarket {
    public static class Guard {
        public static void NotNull(Object obj, String argName) {
            if (obj == null)
                throw ErrorHelper.ArgNull(argName);
        }
        public static void PropertyNotNull(Object obj, String propertyPath) {
            if (obj == null)
                throw ErrorHelper.ArgNull(String.Format("Property path {0} cannot be null", propertyPath));
        }
        public static void NotNullOrEmpty(String value, String argName) {
            if (String.IsNullOrWhiteSpace(value))
                throw ErrorHelper.ArgNull(argName);
        }
        public static void PropertyNotNullOrEmpty(String value, String propertyPath) {
            if (String.IsNullOrWhiteSpace(value))
                throw ErrorHelper.ArgNull(String.Format("Property path {0} cannot be null or empty", propertyPath));
        }
        public static void IsTrue<T>(bool value) where T : Exception, new() {
            if (!value)
                throw new T();
        }
        public static void IsTrue<T>(bool value, T error) where T : Exception {
            if (!value)
                throw error;
        }
    }
}