using Product.Domain;

namespace Product.Domain.Tests;

public class AssetLanguageSelectorTests
{
    [Test]
    public void Omitted_language_uses_product_content_language()
    {
        var assets = new[]
        {
            new Asset { Id = Guid.NewGuid(), LanguageCode = "en" },
            new Asset { Id = Guid.NewGuid(), LanguageCode = "es" },
            new Asset { Id = Guid.NewGuid(), LanguageCode = null }
        };

        var selected = AssetLanguageSelector.Select(assets, "en", requestedLanguage: null);

        Assert.That(selected.Select(a => a.LanguageCode), Is.EquivalentTo(new string?[] { "en", null }));
    }

    [Test]
    public void Requested_language_selects_matching_assets()
    {
        var assets = new[]
        {
            new Asset { Id = Guid.NewGuid(), LanguageCode = "en" },
            new Asset { Id = Guid.NewGuid(), LanguageCode = "es" }
        };

        var selected = AssetLanguageSelector.Select(assets, "en", "es");

        Assert.That(selected, Has.Count.EqualTo(1));
        Assert.That(selected[0].LanguageCode, Is.EqualTo("es"));
    }
}
