namespace Product.WebApi.Acceptance.Tests;

public class ScaffoldTests
{
    [Test]
    public void Acceptance_project_is_wired_to_ApplicationRoot()
    {
        Assert.That(typeof(Program).Assembly.GetName().Name, Is.EqualTo("Product.ApplicationRoot"));
    }
}
