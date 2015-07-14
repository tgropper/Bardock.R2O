using Bardock.UnitTesting.AutoFixture.Xunit2.Attributes;

namespace Bardock.R2O.Tests.Fixtures.Attributes
{
    internal class InlineDefaultDataAttribute : InlineAutoDataAndCustomizationsAttribute
    {
        public InlineDefaultDataAttribute(params object[] valuesAndCustomizationTypes)
            : base(new DefaultDataAttribute(), valuesAndCustomizationTypes)
        {
        }
    }
}