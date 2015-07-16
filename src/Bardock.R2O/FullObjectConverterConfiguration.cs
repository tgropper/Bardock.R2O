using System.Collections.Generic;

namespace Bardock.R2O
{
    public class FullObjectConfiguration : ObjectConfiguration
    {
        public IEnumerable<FullObjectArrayConfiguration> ArraysConfiguration { get; set; }

        public IEnumerable<FullObjectDynamicObjectConfiguration> DynamicObjectsConfiguration { get; set; }

        public FullObjectConfiguration()
        {
            ArraysConfiguration = new List<FullObjectArrayConfiguration>();
            DynamicObjectsConfiguration = new List<FullObjectDynamicObjectConfiguration>();
        }
    }

    public class FullObjectArrayConfiguration : ObjectConfiguration
    {
        public string FieldName { get; set; }

        public string ParentIdColumn { get; set; }

        /// <summary>
        /// If it has value, the array will be inserted within another array, matching the value of the second column with the array element taken by this key
        /// </summary>
        public string InsertIntoArrayComparerKey { get; set; }
    }

    public class FullObjectDynamicObjectConfiguration : ObjectConfiguration
    {
        public string FieldName { get; set; }

        public string ParentIdColumn { get; set; }

        /// <summary>
        /// If it has value, the object will be inserted within an array, matching the value of the second column with the array element taken by this key
        /// </summary>
        public string InsertIntoArrayComparerKey { get; set; }
    }

    public class ObjectConfiguration
    {
        public IEnumerable<IEnumerable<object>> Matrix { get; set; }

        public IEnumerable<string> Columns { get; set; }
    }
}