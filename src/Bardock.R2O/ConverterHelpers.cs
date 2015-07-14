using System;
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
                try
                {
                    ConvertElement(column, element, result);
                }
                catch (ConverterInvalidColumnNameException)
                {
                    throw new ConverterInvalidColumnNameException(column);
                }
            }
            return result;
        }

        internal static Dictionary<string, object> ConvertElement(
            string fieldFullName,
            object value,
            Dictionary<string, object> @object)
        {
            var containerFieldIndex = fieldFullName.IndexOf('.');
            if (containerFieldIndex != -1)
            {
                var fieldName = fieldFullName.Substring(0, containerFieldIndex);
                if (String.IsNullOrEmpty(fieldName))
                    throw new ConverterInvalidColumnNameException();

                if (!@object.ContainsKey(fieldName))
                    @object.Add(fieldName, new Dictionary<string, object>());

                ConvertElement(
                    fieldFullName.Substring(containerFieldIndex + 1),
                    value,
                    (Dictionary<string, object>)@object[fieldName]);
            }
            else
            {
                if (@object.ContainsKey(fieldFullName))
                    throw new ConverterRepeatedColumnException(fieldFullName);
                @object[fieldFullName] = value;
            }
            return @object;
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
            var @object = new Dictionary<string, object>();

            fields.Insert(0, new Dictionary<string, object>() { { "fieldFullName", "id" }, { "value", id } });
            foreach (var field in fields)
            {
                ConvertDynamicObjectField(field["name"].ToString(), field["value"], @object);
            }
            return @object;
        }

        internal static Dictionary<string, object> ConvertDynamicObjectField(string fieldFullName, object value, Dictionary<string, object> @object)
        {
            var containerFieldIndex = fieldFullName.IndexOf('.');
            if (containerFieldIndex != -1)
            {
                var fieldName = fieldFullName.Substring(0, containerFieldIndex);
                if (!@object.ContainsKey(fieldName))
                    @object.Add(fieldName, new Dictionary<string, object>());

                ConvertDynamicObjectField(
                    fieldFullName.Substring(containerFieldIndex + 1),
                    value,
                    (Dictionary<string, object>)@object[fieldName]);
            }
            else
            {
                if (@object.ContainsKey(fieldFullName))
                    throw new ConverterRepeatedColumnException(fieldFullName);
                @object[fieldFullName] = value;
            }
            return @object;
        }

        #endregion Helpers
    }
}