using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Bardock.R2O.Tests.Fixtures.Customizations
{
    public class DefaultCustomization : CompositeCustomization
    {
        public DefaultCustomization()
            : base(
                new AutoMoqCustomization())
        { }
    }
}