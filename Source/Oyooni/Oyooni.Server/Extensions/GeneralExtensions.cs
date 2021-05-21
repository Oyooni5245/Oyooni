using Oyooni.Server.Constants;
using System.IO;
using System.Linq;
using System.Text;

namespace Oyooni.Server.Extensions
{
    /// <summary>
    /// General-purpose extensions
    /// </summary>
    public static class GeneralExtensions
    {
        /// <summary>
        /// Converts a string to utf8 bytes
        /// </summary>
        public static byte[] ToUTF8Bytes(this string value) => Encoding.UTF8.GetBytes(value);

        /// <summary>
        /// Trims an object by trimming all string properties or returns a trimmed object if it was a string
        /// </summary>
        public static T Trim<T>(this T obj) where T : class
        {
            // Get object type
            var objType = typeof(T);

            // If it was string
            if (objType == typeof(string))
            {
                // Trim and return it
                return obj.ToString().Trim() as T;
            }

            // Get readable and writable and string properties
            var propertiesOfTypeString = objType.GetProperties()
                .Where(p => p.PropertyType == typeof(string) && p.CanWrite && p.CanRead);

            // Loop over all properties
            foreach (var property in propertiesOfTypeString)
            {
                // Get the propery value
                var propertyValue = (string)property.GetValue(obj);

                // If it is not null, set the trimmed version
                if (!(propertyValue is null))
                    property.SetValue(obj, propertyValue.Trim());
            }

            // Return the object
            return obj;
        }

        /// <summary>
        /// Returns if the string value is null or empty of whitespace
        /// </summary>
        public static bool IsNullOrEmptyOrWhiteSpaceSafe(this string value)
            => value is null || value == string.Empty || value.All(c => c == ' ');

        public static bool IsImageFileName(this string path)
            => SupportedFileExtensions.Images.Contains(Path.GetExtension(path));
    }
}
