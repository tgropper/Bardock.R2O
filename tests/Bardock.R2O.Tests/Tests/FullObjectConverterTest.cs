using Bardock.R2O.Tests.Assertions;
using Bardock.R2O.Tests.Fixtures.Attributes;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Bardock.R2O.Tests.Tests
{
    public class FullObjectConverterTest
    {
        [Theory]
        [DefaultData]
        public void Convert_SimpleObject_ShouldSucced(FullObjectConverter sut)
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

            var config = new FullObjectConfiguration
            {
                Matrix = matrix,
                Columns = columns
            };

            //Exercise
            var actual = sut.Convert(config);

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
        public void Convert_SimpleObjectWithArrayOfSimpleObject_ShouldSucced(FullObjectConverter sut)
        {
            //Setup
            var columns = new List<string>() {
                "id"
            };
            var matrix = new List<List<object>>()
            {
                new List<object>() { 1 },
                new List<object>() { 2 }
            };
            var arrayColumns = new List<string>() {
                "parentId",
                "title"
            };
            var arrayMatrix = new List<List<object>>()
            {
                new List<object>() { 1, "titleValue11" },
                new List<object>() { 1, "titleValue12" },
                new List<object>() { 2, "titleValue21" }
            };
            var arrayConfig = new List<FullObjectArrayConfiguration>()
            {
                new FullObjectArrayConfiguration
                {
                    Columns = arrayColumns,
                    Matrix = arrayMatrix,
                    FieldName = "notes",
                    ParentIdColumn = "parentId"
                }
            };

            var config = new FullObjectConfiguration
            {
                Matrix = matrix,
                Columns = columns,
                ArraysConfiguration = arrayConfig
            };

            //Exercise
            var actual = sut.Convert(config);

            //Verify
            actual.Should().NotBeEmpty()
                .And.HaveSameCount(matrix)
                .And.DeepContain(new Dictionary<string, object>()
                {
                    { "id", 1 },
                    { "notes", new List<Dictionary<string, object>>()
                        {
                            new Dictionary<string, object>()
                            {
                                { "title", "titleValue11" }
                            },
                            new Dictionary<string, object>()
                            {
                                { "title", "titleValue12" }
                            }
                        }
                    }
                })
                .And.DeepContain(new Dictionary<string, object>()
                {
                    { "id", 2 },
                    { "notes", new List<Dictionary<string, object>>()
                        {
                            new Dictionary<string, object>()
                            {
                                { "title", "titleValue21" }
                            }
                        }
                    }
                });
        }

        [Theory]
        [DefaultData]
        public void Convert_SimpleObjectWithArrayOfNestedObject_ShouldSucced(FullObjectConverter sut)
        {
            //Setup
            var columns = new List<string>() {
                "id"
            };
            var matrix = new List<List<object>>()
            {
                new List<object>() { 1 },
                new List<object>() { 2 }
            };
            var arrayColumns = new List<string>() {
                "parentId",
                "nested.nested2.field"
            };
            var arrayMatrix = new List<List<object>>()
            {
                new List<object>() { 1, "value11" },
                new List<object>() { 1, "value12" },
                new List<object>() { 2, "value21" }
            };
            var arrayConfig = new List<FullObjectArrayConfiguration>()
            {
                new FullObjectArrayConfiguration
                {
                    Columns = arrayColumns,
                    Matrix = arrayMatrix,
                    FieldName = "array",
                    ParentIdColumn = "parentId"
                }
            };

            var config = new FullObjectConfiguration
            {
                Matrix = matrix,
                Columns = columns,
                ArraysConfiguration = arrayConfig
            };

            //Exercise
            var actual = sut.Convert(config);

            //Verify
            actual.Should().NotBeEmpty()
                .And.HaveSameCount(matrix)
                .And.DeepContain(new Dictionary<string, object>()
                {
                    { "id", 1 },
                    { "array", new List<Dictionary<string, object>>()
                        {
                            new Dictionary<string, object>()
                            {
                                { "nested", new Dictionary<string, object>()
                                    {
                                        { "nested2", new Dictionary<string, object>()
                                            {
                                                { "field", "value11" }
                                            }
                                        }
                                    }
                                }
                            },
                            new Dictionary<string, object>()
                            {
                                { "nested", new Dictionary<string, object>()
                                    {
                                        { "nested2", new Dictionary<string, object>()
                                            {
                                                { "field", "value12" }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                })
                .And.DeepContain(new Dictionary<string, object>()
                {
                    { "id", 2 },
                    { "array", new List<Dictionary<string, object>>()
                        {
                            new Dictionary<string, object>()
                            {
                                { "nested", new Dictionary<string, object>()
                                    {
                                        { "nested2", new Dictionary<string, object>()
                                            {
                                                { "field", "value21" }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
        }

        [Theory]
        [DefaultData]
        public void Convert_NestedObjectWithArrayOfSimpleObject_ShouldSucced(FullObjectConverter sut)
        {
            //Setup
            var columns = new List<string>() {
                "id",
                "address.state.name"
            };
            var matrix = new List<List<object>>()
            {
                new List<object>() { 1, "Florida" },
                new List<object>() { 2, "NY" }
            };
            var arrayColumns = new List<string>() {
                "stateId",
                "name"
            };
            var arrayMatrix = new List<List<object>>()
            {
                new List<object>() { 1, "Miami" },
                new List<object>() { 1, "Orlando" },
                new List<object>() { 2, "NYC" },
                new List<object>() { 2, "Buffalo" }
            };
            var arrayConfig = new List<FullObjectArrayConfiguration>()
            {
                new FullObjectArrayConfiguration
                {
                    Columns = arrayColumns,
                    Matrix = arrayMatrix,
                    FieldName = "address.state.cities",
                    ParentIdColumn = "stateId"
                }
            };

            var config = new FullObjectConfiguration
            {
                Matrix = matrix,
                Columns = columns,
                ArraysConfiguration = arrayConfig
            };

            //Exercise
            var actual = sut.Convert(config);

            //Verify
            actual.Should().NotBeEmpty()
                .And.HaveSameCount(matrix)
                .And.DeepContain(new Dictionary<string, object>()
                {
                    { "id", 1 },
                    { "address", new Dictionary<string, object>()
                        {
                            { "state", new Dictionary<string, object>()
                                {
                                    { "name", "Florida" },
                                    { "cities", new List<Dictionary<string, object>>()
                                        {
                                            new Dictionary<string, object>()
                                            {
                                                { "name", "Miami" }
                                            },
                                            new Dictionary<string, object>()
                                            {
                                                { "name", "Orlando" }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                })
                .And.DeepContain(new Dictionary<string, object>()
                {
                    { "id", 2 },
                    { "address", new Dictionary<string, object>()
                        {
                            { "state", new Dictionary<string, object>()
                                {
                                    { "name", "NY" },
                                    { "cities", new List<Dictionary<string, object>>()
                                        {
                                            new Dictionary<string, object>()
                                            {
                                                { "name", "NYC" }
                                            },
                                            new Dictionary<string, object>()
                                            {
                                                { "name", "Buffalo" }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
        }

        [Theory]
        [DefaultData]
        public void Convert_NestedObjectWithArrayOfNestedObject_ShouldSucced(FullObjectConverter sut)
        {
            //Setup
            var columns = new List<string>() {
                "id",
                "address.state.name"
            };
            var matrix = new List<List<object>>()
            {
                new List<object>() { 1, "Florida" },
                new List<object>() { 2, "NY" }
            };
            var arrayColumns = new List<string>() {
                "stateId",
                "name",
                "mayor.name",
                "mayor.age"
            };
            var arrayMatrix = new List<List<object>>()
            {
                new List<object>() { 1, "Miami", "Tomás Regalado", 50 },
                new List<object>() { 1, "Orlando", "Buddy Dyer", 62 },
                new List<object>() { 2, "NYC", "Bill de Blasio", 70 },
                new List<object>() { 2, "Buffalo", "Byron Brown", 46 }
            };
            var arrayConfig = new List<FullObjectArrayConfiguration>()
            {
                new FullObjectArrayConfiguration
                {
                    Columns = arrayColumns,
                    Matrix = arrayMatrix,
                    FieldName = "address.state.cities",
                    ParentIdColumn = "stateId"
                }
            };

            var config = new FullObjectConfiguration
            {
                Matrix = matrix,
                Columns = columns,
                ArraysConfiguration = arrayConfig
            };

            //Exercise
            var actual = sut.Convert(config);

            //Verify
            actual.Should().NotBeEmpty()
                .And.HaveSameCount(matrix)
                .And.DeepContain(new Dictionary<string, object>()
                {
                    { "id", 1 },
                    { "address", new Dictionary<string, object>()
                        {
                            { "state", new Dictionary<string, object>()
                                {
                                    { "name", "Florida" },
                                    { "cities", new List<Dictionary<string, object>>()
                                        {
                                            new Dictionary<string, object>()
                                            {
                                                { "name", "Miami" },
                                                { "mayor", new Dictionary<string, object>()
                                                    {
                                                        { "name", "Tomás Regalado" },
                                                        { "age", 50 }
                                                    }
                                                }
                                            },
                                            new Dictionary<string, object>()
                                            {
                                                { "name", "Orlando" },
                                                { "mayor", new Dictionary<string, object>()
                                                    {
                                                        { "name", "Buddy Dyer" },
                                                        { "age", 62 }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                })
                .And.DeepContain(new Dictionary<string, object>()
                {
                    { "id", 2 },
                    { "address", new Dictionary<string, object>()
                        {
                            { "state", new Dictionary<string, object>()
                                {
                                    { "name", "NY" },
                                    { "cities", new List<Dictionary<string, object>>()
                                        {
                                            new Dictionary<string, object>()
                                            {
                                                { "name", "NYC" },
                                                { "mayor", new Dictionary<string, object>()
                                                    {
                                                        { "name", "Bill de Blasio" },
                                                        { "age", 70 }
                                                    }
                                                }
                                            },
                                            new Dictionary<string, object>()
                                            {
                                                { "name", "Buffalo" },
                                                { "mayor", new Dictionary<string, object>()
                                                    {
                                                        { "name", "Byron Brown" },
                                                        { "age", 46 }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
        }

        [Theory]
        [DefaultData]
        public void Convert_SimpleObjectWithArrayOfSimpleObjectWithinArrayOfSimpleObject_ShouldSucced(FullObjectConverter sut)
        {
            //Setup
            var columns = new List<string>() {
                "id"
            };
            var matrix = new List<List<object>>()
            {
                new List<object>() { 1 },
                new List<object>() { 2 }
            };
            var arrayColumns = new List<string>() {
                "parentId",
                "id"
            };
            var arrayMatrix = new List<List<object>>()
            {
                new List<object>() { 1, 5 },
                new List<object>() { 1, 6 },
                new List<object>() { 2, 7 }
            };
            var arrayInceptionColumns = new List<string>() {
                "parentId",
                "arrayKeyComparer",
                "val"
            };
            var arrayInceptionMatrix = new List<List<object>>()
            {
                new List<object>() { 1, 5, "val1" },
                new List<object>() { 1, 5, "val2" },
                new List<object>() { 1, 6, "val3" },
                new List<object>() { 2, 7, "val4" }
            };
            var arrayConfig = new List<FullObjectArrayConfiguration>()
            {
                new FullObjectArrayConfiguration
                {
                    Columns = arrayColumns,
                    Matrix = arrayMatrix,
                    FieldName = "array",
                    ParentIdColumn = "parentId"
                },
                new FullObjectArrayConfiguration
                {
                    Columns = arrayInceptionColumns,
                    Matrix = arrayInceptionMatrix,
                    FieldName = "array.arrayInception",
                    ParentIdColumn = "parentId",
                    InsertIntoArrayComparerKey = "arrayKeyComparer"
                }
            };

            var config = new FullObjectConfiguration
            {
                Matrix = matrix,
                Columns = columns,
                ArraysConfiguration = arrayConfig
            };

            //Exercise
            var actual = sut.Convert(config);

            //Verify
            actual.Should().NotBeEmpty()
                .And.HaveSameCount(matrix)
                .And.DeepContain(new Dictionary<string, object>()
                {
                    { "id", 1 },
                    { "array", new List<Dictionary<string, object>>()
                        {
                            new Dictionary<string, object>()
                            {
                                { "id", 5 },
                                { "arrayInception", new List<Dictionary<string, object>>()
                                    {
                                        new Dictionary<string, object>()
                                        {
                                            { "val", "val1" }
                                        },
                                        new Dictionary<string, object>()
                                        {
                                            { "val", "val2" }
                                        }
                                    }
                                }
                            },
                            new Dictionary<string, object>()
                            {
                                { "id", 6 },
                                { "arrayInception", new List<Dictionary<string, object>>()
                                    {
                                        new Dictionary<string, object>()
                                        {
                                            { "val", "val3" }
                                        }
                                    }
                                }
                            }
                        }
                    }
                })
                .And.DeepContain(new Dictionary<string, object>()
                {
                    { "id", 2 },
                    { "array", new List<Dictionary<string, object>>()
                        {
                            new Dictionary<string, object>()
                            {
                                { "id", 7 },
                                { "arrayInception", new List<Dictionary<string, object>>()
                                    {
                                        new Dictionary<string, object>()
                                        {
                                            { "val", "val4" }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
        }

        [Theory]
        [DefaultData]
        public void Convert_SimpleObjectWithArrayOfNestedObjectWithinArrayOfSimpleObject_ShouldSucced(FullObjectConverter sut)
        {
            //Setup
            var columns = new List<string>() {
                "id"
            };
            var matrix = new List<List<object>>()
            {
                new List<object>() { 1 },
                new List<object>() { 2 }
            };
            var arrayColumns = new List<string>() {
                "parentId",
                "id"
            };
            var arrayMatrix = new List<List<object>>()
            {
                new List<object>() { 1, 5 },
                new List<object>() { 1, 6 },
                new List<object>() { 2, 7 }
            };
            var arrayInceptionColumns = new List<string>() {
                "parentId",
                "arrayKeyComparer",
                "container.val"
            };
            var arrayInceptionMatrix = new List<List<object>>()
            {
                new List<object>() { 1, 5, "val1" },
                new List<object>() { 1, 5, "val2" },
                new List<object>() { 1, 6, "val3" },
                new List<object>() { 2, 7, "val4" }
            };
            var arrayConfig = new List<FullObjectArrayConfiguration>()
            {
                new FullObjectArrayConfiguration
                {
                    Columns = arrayColumns,
                    Matrix = arrayMatrix,
                    FieldName = "array",
                    ParentIdColumn = "parentId"
                },
                new FullObjectArrayConfiguration
                {
                    Columns = arrayInceptionColumns,
                    Matrix = arrayInceptionMatrix,
                    FieldName = "array.arrayInception",
                    ParentIdColumn = "parentId",
                    InsertIntoArrayComparerKey = "arrayKeyComparer"
                }
            };

            var config = new FullObjectConfiguration
            {
                Matrix = matrix,
                Columns = columns,
                ArraysConfiguration = arrayConfig
            };

            //Exercise
            var actual = sut.Convert(config);

            //Verify
            actual.Should().NotBeEmpty()
                .And.HaveSameCount(matrix)
                .And.DeepContain(new Dictionary<string, object>()
                {
                    { "id", 1 },
                    { "array", new List<Dictionary<string, object>>()
                        {
                            new Dictionary<string, object>()
                            {
                                { "id", 5 },
                                { "arrayInception", new List<Dictionary<string, object>>()
                                    {
                                        new Dictionary<string, object>()
                                        {
                                            { "container", new Dictionary<string, object>()
                                                {
                                                    { "val", "val1" }
                                                }
                                            }
                                        },
                                        new Dictionary<string, object>()
                                        {
                                            { "container", new Dictionary<string, object>()
                                                {
                                                    { "val", "val2" }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            new Dictionary<string, object>()
                            {
                                { "id", 6 },
                                { "arrayInception", new List<Dictionary<string, object>>()
                                    {
                                        new Dictionary<string, object>()
                                        {
                                            { "container", new Dictionary<string, object>()
                                                {
                                                    { "val", "val3" }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                })
                .And.DeepContain(new Dictionary<string, object>()
                {
                    { "id", 2 },
                    { "array", new List<Dictionary<string, object>>()
                        {
                            new Dictionary<string, object>()
                            {
                                { "id", 7 },
                                { "arrayInception", new List<Dictionary<string, object>>()
                                    {
                                        new Dictionary<string, object>()
                                        {
                                            { "container", new Dictionary<string, object>()
                                                {
                                                    { "val", "val4" }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
        }
    }
}