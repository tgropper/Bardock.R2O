using Bardock.R2O.Tests.Assertions;
using Bardock.R2O.Tests.Fixtures.Attributes;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Bardock.R2O.Tests.Tests
{
    public class ConverterTest
    {
        [Theory]
        [DefaultData]
        public void Convert_SimpleObject_ShouldSucced(Converter sut)
        {
            //Setup
            var columns = new List<string>() {
                "id",
                "name"
            };
            var matrix = new List<List<object>>()
            {
                new List<object>() { 1, "name1" },
                new List<object>() { 2, "name2" },
                new List<object>() { 3, "name3" }
            };

            //Exercise
            var actual = sut.Convert(columns, matrix);

            //Verify
            actual.Should().NotBeEmpty()
                .And.HaveSameCount(matrix)
                .And.DeepContain(new Dictionary<string, object>() { { "id", 1 }, { "name", "name1" } })
                .And.DeepContain(new Dictionary<string, object>() { { "id", 2 }, { "name", "name2" } })
                .And.DeepContain(new Dictionary<string, object>() { { "id", 3 }, { "name", "name3" } });
        }

        [Theory]
        [DefaultData]
        public void Convert_NestedObject_ShouldSucced(Converter sut)
        {
            //Setup
            var columns = new List<string>() {
                "id",
                "city.name",
                "city.country.isoCode"
            };
            var matrix = new List<List<object>>()
            {
                new List<object>() { 1, "cityName1", "countryIsoCode1" },
                new List<object>() { 2, "cityName2", "countryIsoCode2" }
            };

            //Exercise
            var actual = sut.Convert(columns, matrix);

            //Verify
            actual.Should().NotBeEmpty()
                .And.HaveSameCount(matrix)
                .And.DeepContain(new Dictionary<string, object>()
                {
                    { "id", 1 },
                    { "city", new Dictionary<string, object>()
                        {
                            { "name", "cityName1" },
                            { "country", new Dictionary<string, object>()
                                {
                                    { "isoCode", "countryIsoCode1" }
                                }
                           }
                        }
                    }
                })
                .And.DeepContain(new Dictionary<string, object>()
                {
                    { "id", 2 },
                    { "city", new Dictionary<string, object>()
                        {
                            { "name", "cityName2" },
                            { "country", new Dictionary<string, object>()
                                {
                                    { "isoCode", "countryIsoCode2" }
                                }
                            }
                        }
                    }
                });
        }

        [Theory]
        [DefaultData]
        public void Convert_SimpleObjectWithRepeatedColumns_ShouldThrowRepeatedColumnException(Converter sut)
        {
            var repeatedColumnName = "id";
            //Setup
            var columns = new List<string>() {
                repeatedColumnName,
                repeatedColumnName
            };
            var matrix = new List<List<object>>()
            {
                new List<object>() { 1, "name1" }
            };

            //Exercise
            Action exercise = () => sut.Convert(columns, matrix);

            //Verify
            exercise.ShouldThrow<ConverterRepeatedColumnException>()
                .Where(x => x.Message.Contains(repeatedColumnName));
        }

        [Theory]
        [DefaultData]
        public void Convert_SimpleObjectWithRepeatedColumns_ShouldThrowInvalidColumnException(Converter sut)
        {
            var invalidColumnName = "address..zipCode";
            //Setup
            var columns = new List<string>() {
                "id",
                invalidColumnName
            };
            var matrix = new List<List<object>>()
            {
                new List<object>() { 1, 2605 }
            };

            //Exercise
            Action exercise = () => sut.Convert(columns, matrix);

            //Verify
            exercise.ShouldThrow<ConverterInvalidColumnNameException>()
                .Where(x => x.Message.Contains(invalidColumnName));
        }

        [Theory]
        [DefaultData]
        public void Convert_SimpleDynamicObject_ShouldSucced(Converter sut)
        {
            //Setup
            var columns = new List<string>() {
                "id",
                "name",
                "value"
            };
            var matrix = new List<List<object>>()
            {
                new List<object>() { 1, "firstName", "firstName" },
                new List<object>() { 1, "lastName", "lastName" },
                new List<object>() { 2, "hasPets", true }
            };

            //Exercise
            var actual = sut.ConvertDynamic(columns, matrix);

            //Verify
            actual.Should().NotBeEmpty()
                .And.HaveCount(2)
                .And.DeepContain(new Dictionary<string, object>() { { "id", 1 }, { "firstName", "firstName" }, { "lastName", "lastName" } })
                .And.DeepContain(new Dictionary<string, object>() { { "id", 2 }, { "hasPets", true } });
        }

        [Theory]
        [DefaultData]
        public void Convert_NestedDynamicObject_ShouldSucced(Converter sut)
        {
            //Setup
            var columns = new List<string>() {
                "id",
                "name",
                "value"
            };
            var matrix = new List<List<object>>()
            {
                new List<object>() { 1, "city.name", "cityName" },
                new List<object>() { 1, "city.country.isoCode", "countryIsoCode" },
                new List<object>() { 2, "school.name", "schoolName" }
            };

            //Exercise
            var actual = sut.ConvertDynamic(columns, matrix);

            //Verify
            actual.Should().HaveCount(2)
                .And.DeepContain(new Dictionary<string, object>()
                {
                    { "id", 1},
                    { "city", new Dictionary<string, object>()
                        {
                            { "name", "cityName" },
                            { "country", new Dictionary<string, object>()
                                {
                                    { "isoCode", "countryIsoCode" }
                                }
                           }
                        }
                    }
                })
                .And.DeepContain(new Dictionary<string, object>()
                {
                    { "id", 2},
                    { "school", new Dictionary<string, object>()
                        {
                            { "name", "schoolName" }
                        }
                    }
                });
        }

        [Theory]
        [DefaultData]
        public void Convert_DynamicObjectWithRepeatedColumns_ShouldThrowRepeatedColumnException(Converter sut)
        {
            var repeatedLabelName = "firstName";
            //Setup
            var columns = new List<string>() {
                "id",
                "name",
                "value"
            };
            var matrix = new List<List<object>>()
            {
                new List<object>() { 1, repeatedLabelName, "firstName1" },
                new List<object>() { 1, repeatedLabelName, "firstName2" }
            };

            //Exercise
            Action exercise = () => sut.ConvertDynamic(columns, matrix);

            //Verify
            exercise.ShouldThrow<ConverterRepeatedColumnException>()
                .Where(x => x.Message.Contains(repeatedLabelName));
        }
    }
}