namespace Acme.Core.Tests.Fixtures;

public class BaseApplicationFixture : BaseFixture
{
    public BaseApplicationFixture()
    {
        ServiceCollection.AddCoreServices(Configuration);
    }
}