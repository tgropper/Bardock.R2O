using System.Collections.Generic;

namespace Bardock.R2O
{
    public class Converter : IConverter
    {
        /// <summary>
        /// Converts the specified matrix into a Dictionary, that can be easily serialized into json or another type of object
        /// </summary>
        /// <param name="columns">Matrix columns names, in the same order as the columns appear in the matrix</param>
        /// <param name="matrix">Matrix in bidimensional form</param>
        /// <returns>Converted object</returns>
        public IEnumerable<Dictionary<string, object>> Convert(IEnumerable<string> columns, IEnumerable<IEnumerable<object>> matrix)
        {
            var results = new List<Dictionary<string, object>>();
            foreach (var row in matrix)
            {
                var r = ConverterHelpers.ConvertRow(columns, row);
                results.Add(r);
            }
            return results;
        }

        /// <summary>
        /// Converts the specified matrix into a Dictionary, that can be easily serialized into json or another type of object
        /// </summary>
        /// <param name="columns">Matrix columns names, it must have this three columns: id, label and value</param>
        /// <param name="matrix">Matrix in bidimensional form</param>
        /// <returns>Converted object</returns>
        public IEnumerable<Dictionary<string, object>> ConvertDynamic(IEnumerable<string> columns, IEnumerable<IEnumerable<object>> matrix)
        {
            var results = new List<Dictionary<string, object>>();

            var convertionResults = ConverterHelpers.ConvertDynamicRows(columns, matrix);
            foreach (var convertedNewObject in convertionResults)
            {
                var dynamicObject = ConverterHelpers.ConvertDynamicObjectFields(convertedNewObject.Key, convertedNewObject.Value);
                results.Add(dynamicObject);
            }
            return results;
        }
    }
}