using Product.Domain;

namespace Product.Domain.Tests;

public class IProductRepositoryTests
{
    [Test]
    public void Product_repository_port_is_an_interface()
    {
        Assert.That(typeof(IProductRepository).IsInterface, Is.True);
    }
}
