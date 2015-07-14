using Bardock.R2O.Tests.Fixtures.Customizations;
using System;

namespace Bardock.R2O.Tests.Fixtures.Attributes
{
    public class DefaultDataAttribute : Bardock.UnitTesting.AutoFixture.Xunit2.Fixtures.Attributes.DefaultDataAttribute
    {
        public DefaultDataAttribute()
            : base(new DefaultCustomization())
        {
            Init();
        }

        public DefaultDataAttribute(params Type[] customizationTypes)
            : base(new DefaultCustomization(), customizationTypes)
        {
            Init();
        }

        private void Init()
        {
        }
    }
}