// Import necessary namespaces
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MVA_poe.Data
{
    // Define a static class to extend enum functionality
    public static class EnumExtend
    {
        // Extension method to get the description attribute of an enum value
        public static string GetString(this Enum value)
        {
            // Get the field information for the enum value
            FieldInfo field = value.GetType().GetField(value.ToString());

            // Get the DescriptionAttribute of the field, if it exists
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();

            // Return the description if it exists, otherwise return the enum value as a string
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____---____---____---____---__\\
