using AutoFixture;

namespace CleanArchitecture.Tests
{
    public abstract class BaseUnitTest
    {
        protected IFixture fixture;

        protected BaseUnitTest()
        {
            this.fixture = new Fixture();
            this.fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
    }
}
