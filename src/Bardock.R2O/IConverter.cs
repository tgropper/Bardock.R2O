using System.Collections.Generic;

namespace Bardock.R2O
{
    public interface IConverter
    {
        /// <summary>
        /// Converts the specified matrix into a Dictionary, that can be easily serialized into json or another type of object
        /// </summary>
        /// <param name="columns">Matrix columns names, in the same order as the columns appear in the matrix</param>
        /// <param name="matrix">Matrix in bidimensional form</param>
        /// <returns>Converted object</returns>
        /// <example>
        ///     id     |   name     |   lastName    |   instrument.name |   instrument.type     |   instrument.brand.name
        ///     1      |   "Paul"   |   "McCartney" |   "bass"          |   "string"            |   "Höfner"
        ///     2      |   "John"   |   "Lennon"    |   "guitar"        |   "string"            |   "Gibson"
        ///
        ///
        /// This matrix will generate the object:
        /// [
        ///     {
        ///         id: 1,
        ///         name: "Paul",
        ///         lastName: "McCartney",
        ///         instrument: {
        ///             name: "bass",
        ///             type: "string",
        ///             brand: {
        ///                 "Höfner"
        ///             }
        ///         }
        ///     },
        ///     {
        ///         id: 2,
        ///         name: "John",
        ///         lastName: "Lennon",
        ///         instrument: {
        ///             name: "guitar",
        ///             type: "string",
        ///             brand: {
        ///                 "Gibson"
        ///             }
        ///         }
        ///     }
        /// ]
        /// </example>
        IEnumerable<Dictionary<string, object>> Convert(IEnumerable<string> columns, IEnumerable<IEnumerable<object>> matrix);

        /// <summary>
        /// Converts the specified matrix into a Dictionary, that can be easily serialized into json or another type of object
        /// </summary>
        /// <param name="columns">Matrix columns names, it must have this three columns: id, label, value</param>
        /// <param name="matrix">Matrix in bidimensional form</param>
        /// <returns>Converted object</returns>
        /// <summary>
        /// It represents an object that its fields are built dynamically based on rows and not on columns
        /// </summary>
        /// <example>
        ///     id      |   field   |   value
        ///     1       |   "name"  |   "Ringo Starr"
        ///     1       |   "age"   |   26
        ///     1       |   "email" |   "ringo@starr.com"
        ///
        /// This will build the object:
        /// [
        ///     {
        ///         id: 1,
        ///         name: "Ringo Starr",
        ///         age: 26,
        ///         email: "ringo@starr.com"
        ///     }
        /// ]
        ///
        /// NOTE: first column (id) of matrix is used to group the object properties
        /// NOTE2: this objects can have nested objects inside:
        ///
        ///     id      |   field                           |   value
        ///     1       |   "hasDogs"                       |   true
        ///     1       |   "address.street"                |   "Penny Lane"
        ///     1       |   "address.number"                |   2605
        ///     2       |   "address.zipCode"               |   1992
        ///     2       |   "address.city.name"             |   "Liverpool"
        ///     2       |   "address.city.country.isoCode"  |   "UK"
        ///
        /// So the dynamic objects will be:
        /// [
        ///     {
        ///         id: 1,
        ///         hasDogs: true,
        ///         address: {
        ///             street: "Penny Lane",
        ///             number: 2605
        ///         }
        ///     },
        ///     {
        ///         id: 2,
        ///         address: {
        ///             zipCode: 1992,
        ///             city: {
        ///                 name: "Liverpool",
        ///                 country: {
        ///                     isoCode: "UK"
        ///                 }
        ///             }
        ///         }
        ///     }
        /// ]
        /// </example>
        IEnumerable<Dictionary<string, object>> ConvertDynamic(IEnumerable<string> columns, IEnumerable<IEnumerable<object>> matrix);
    }
}