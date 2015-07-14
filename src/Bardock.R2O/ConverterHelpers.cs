using System.Collections.Generic;
using System.Linq;

namespace Bardock.R2O
{
    internal static class ConverterHelpers
    {
        #region Helpers

        internal static Dictionary<string, object> ConvertRow(
            IEnumerable<string> columns,
            IEnumerable<object> row)
        {
            var result = new Dictionary<string, object>();
            for (var i = 0; i < columns.Count(); i++)
            {
                var column = columns.ElementAt(i);
                var element = row.ElementAt(i);
                ConvertElement(column, element, result);
            }
            return result;
        }

        internal static Dictionary<string, object> ConvertElement(
            string column,
            object value,
            Dictionary<string, object> result)
        {
            var containerObjectIndex = column.IndexOf('.');
            if (containerObjectIndex != -1)
            {
                var objName = column.Substring(0, containerObjectIndex);
                if (!result.ContainsKey(objName))
                    result.Add(objName, new Dictionary<string, object>());

                ConvertElement(
                    column.Substring(containerObjectIndex + 1),
                    value,
                    (Dictionary<string, object>)result[objName]);
            }
            else
            {
                if (result.ContainsKey(column))
                    throw new ConverterRepeatedColumnException(column);
                result[column] = value;
            }
            return result;
        }

        internal static Dictionary<object, List<Dictionary<string, object>>> ConvertDynamicRows(IEnumerable<string> columns, IEnumerable<IEnumerable<object>> matrix)
        {
            var convertionResults = new Dictionary<object, List<Dictionary<string, object>>>();
            foreach (var row in matrix)
            {
                var convertedRow = ConvertRow(columns, row);
                var convertionKey = convertedRow.Values.First();
                convertedRow = convertedRow.Skip(1).ToDictionary(x => x.Key, x => x.Value);

                if (!convertionResults.ContainsKey(convertionKey))
                    convertionResults.Add(convertionKey, new List<Dictionary<string, object>>());

                convertionResults[convertionKey].Add(convertedRow);
            }
            return convertionResults;
        }

        internal static Dictionary<string, object> ConvertDynamicObjectFields(object id, List<Dictionary<string, object>> fields)
        {
            var convertedObject = new Dictionary<string, object>();

            fields.Insert(0, new Dictionary<string, object>() { { "label", "id" }, { "value", id } });
            foreach (var field in fields)
            {
                ConvertDynamicObjectField(field["label"].ToString(), field["value"], convertedObject);
            }
            return convertedObject;
        }

        internal static Dictionary<string, object> ConvertDynamicObjectField(string label, object value, Dictionary<string, object> convertedObject)
        {
            var objectIndex = label.IndexOf('.');
            if (objectIndex != -1)
            {
                var objectName = label.Substring(0, objectIndex);
                if (!convertedObject.ContainsKey(objectName))
                    convertedObject.Add(objectName, new Dictionary<string, object>());

                ConvertDynamicObjectField(
                    label.Substring(objectIndex + 1),
                    value,
                    (Dictionary<string, object>)convertedObject[objectName]);
            }
            else
            {
                if (convertedObject.ContainsKey(label))
                    throw new ConverterRepeatedColumnException(label);
                convertedObject[label] = value;
            }
            return convertedObject;
        }

        #endregion Helpers
    }
}