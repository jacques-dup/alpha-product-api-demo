using Product.Domain;

namespace Product.Domain.Tests;

public class CountryMarketAclTests
{
    [Test]
    public void Kenya_and_Colombia_map_to_region_markets()
    {
        Assert.That(CountryMarketAcl.TryResolve("ke", out var kenya), Is.True);
        Assert.That(kenya, Is.EqualTo("ssa"));
        Assert.That(CountryMarketAcl.TryResolve("co", out var colombia), Is.True);
        Assert.That(colombia, Is.EqualTo("lat"));
    }

    [Test]
    public void Market_codes_pass_through()
    {
        Assert.That(CountryMarketAcl.TryResolve("za", out var za), Is.True);
        Assert.That(za, Is.EqualTo("za"));
        Assert.That(CountryMarketAcl.TryResolve("SSA", out var ssa), Is.True);
        Assert.That(ssa, Is.EqualTo("ssa"));
    }

    [Test]
    public void Unknown_country_does_not_resolve()
    {
        Assert.That(CountryMarketAcl.TryResolve("xx", out _), Is.False);
        Assert.That(CountryMarketAcl.TryResolve(" ", out _), Is.False);
        Assert.That(CountryMarketAcl.TryResolve(null, out _), Is.False);
    }
}
