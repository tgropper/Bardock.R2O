using System;
using System.Linq;
using System.Collections.Generic;

namespace Bardock.R2O
{
    public class FullObjectConverter : IFullObjectConverter
    {
        public IEnumerable<Dictionary<string, object>> Convert(FullObjectConfiguration config)
        {
            var converter = new Converter();
            var results = converter.Convert(config.Columns, config.Matrix);

            var resultsDictionary = results.ToDictionary(x => x.First().Value, x => x);
            foreach (var arrayConfig in config.ArraysConfiguration)
            {
                var arrayElements = converter.Convert(arrayConfig.Columns, arrayConfig.Matrix)
                    .GroupBy(x => x[arrayConfig.ParentIdColumn])
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Where(z => z.Key != arrayConfig.ParentIdColumn).ToDictionary(w => w.Key, w => w.Value)).ToList());
                foreach (var @object in resultsDictionary)
                {
                    if (arrayElements.ContainsKey(@object.Key))
                    { 
                        AddArray(@object.Value, arrayElements[@object.Key], arrayConfig.FieldName, arrayConfig.InsertIntoArrayComparerKey);
                    }
                }
                //ConvertArray(arrayConfig, resultsDictionary);
            }

            return resultsDictionary.Select(x => x.Value).ToList();
        }

        private void AddArray(
            Dictionary<string, object> @object, 
            IEnumerable<Dictionary<string, object>> arrayElements, 
            string fieldName,
            string insertIntoArrayComparerKey = null)
        {
            var newArrayKey = ConverterHelpers.GetLeafName(fieldName);
            Dictionary<string, object> newArrayContainerElement = null;
            if (insertIntoArrayComparerKey != null)
            {
                var existingArrayAttributeName = fieldName.Substring(0, fieldName.Count() - newArrayKey.Count() - 1);
                var existingArrayContainerElement = ConverterHelpers.GetLeafContainerElement(@object, existingArrayAttributeName);
                var existingArrayKey = ConverterHelpers.GetLeafName(existingArrayAttributeName);

                if (!existingArrayContainerElement.ContainsKey(existingArrayKey))
                    throw new Exception("The array property, where you are trying to insert the new array, is not a valid property of the primary object. You have to serialize that array first");

                var existingArray = (List<Dictionary<string, dynamic>>)existingArrayContainerElement[existingArrayKey];

                foreach (var arrayElement in arrayElements)
                {
                    if (!existingArray.Any(x => x.First().Value.ToString() == arrayElement[insertIntoArrayComparerKey].ToString()))
                        throw new Exception("There is no element in the existing array that matches with the key from the serialized array");

                    newArrayContainerElement = existingArray.First(x => x.First().Value.ToString() == arrayElement[insertIntoArrayComparerKey].ToString());
                    var arrayElementWithoutComparerKey = arrayElement.Where(x => x.Key != insertIntoArrayComparerKey).ToDictionary(x => x.Key, x => x.Value);

                    if (!newArrayContainerElement.ContainsKey(newArrayKey))
                        newArrayContainerElement.Add(newArrayKey, new List<Dictionary<string, object>>());

                    ((List<Dictionary<string, object>>)newArrayContainerElement[newArrayKey]).Add(arrayElementWithoutComparerKey);
                }
            }
            else
            {
                newArrayContainerElement = ConverterHelpers.GetLeafContainerElement(@object, fieldName);
                newArrayContainerElement[newArrayKey] = arrayElements;
            }
        }

        //private Dictionary<object, Dictionary<string, object>> ConvertArray(
        //    FullObjectArrayConfiguration config,
        //    Dictionary<object, Dictionary<string, object>> results)
        //{
        //    foreach (var row in config.Matrix)
        //    {
        //        var convertedRow = ConverterHelpers.ConvertRow(config.Columns, row);
        //        if (!results.ContainsKey(convertedRow.Values.First()))
        //            throw new Exception(String.Format("Array element is related with an object with _id {0}, but it doesn't belong to serialized objects list", convertedRow.Values.First()));

        //        var @object = results[convertedRow[config.ParentIdColumn]];
        //        convertedRow = convertedRow.Where(x => x.Key != config.ParentIdColumn).ToDictionary(x => x.Key, x => x.Value);

        //        var newArrayKey = ConverterHelpers.GetLeafName(config.FieldName);
        //        Dictionary<string, object> newArrayContainerElement = null;
        //        if (config.InsertIntoArrayComparerKey != null)
        //        {
        //            var existingArrayAttributeName = config.FieldName.Substring(0, config.FieldName.Count() - newArrayKey.Count() - 1);
        //            var existingArrayContainerElement = ConverterHelpers.GetLeafContainerElement(@object, existingArrayAttributeName);
        //            var existingArrayKey = ConverterHelpers.GetLeafName(existingArrayAttributeName);

        //            if (!existingArrayContainerElement.ContainsKey(existingArrayKey))
        //                throw new Exception("The array property, where you are trying to insert the new array, is not a valid property of the primary object. You have to serialize that array first");

        //            var existingArray = (List<dynamic>)existingArrayContainerElement[existingArrayKey];

        //            if (!existingArray.Any(x => x[config.InsertIntoArrayComparerKey].ToString() == convertedRow[config.InsertIntoArrayComparerKey].ToString()))
        //                throw new Exception("There is no element in the existing array that matches with the key from the serialized array");

        //            newArrayContainerElement = existingArray.First(x => x[config.InsertIntoArrayComparerKey].ToString() == convertedRow[config.InsertIntoArrayComparerKey].ToString());
        //            convertedRow = convertedRow.Where(x => x.Key != config.InsertIntoArrayComparerKey).ToDictionary(x => x.Key, x => x.Value);
        //        }
        //        else
        //        {
        //            newArrayContainerElement = ConverterHelpers.GetLeafContainerElement(@object, config.FieldName);
        //        }

        //        if (!newArrayContainerElement.ContainsKey(newArrayKey))
        //            newArrayContainerElement.Add(newArrayKey, new List<Dictionary<string, object>>());

        //        ((List<Dictionary<string, object>>)newArrayContainerElement[newArrayKey]).Add(convertedRow);
        //    }

        //    return results;
        //}
    }
}