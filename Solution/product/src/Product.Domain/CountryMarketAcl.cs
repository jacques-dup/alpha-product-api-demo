namespace Product.Domain;

/// <summary>
/// Maps a caller country (or already-canonical market) code to <c>market.code</c>.
/// Not a table — dossier section 2.7.
/// </summary>
public static class CountryMarketAcl
{
    private static readonly Dictionary<string, string> Map = new(StringComparer.OrdinalIgnoreCase)
    {
        ["za"] = "za",
        ["gb"] = "gb",
        ["ke"] = "ssa",
        ["co"] = "lat",
        ["ssa"] = "ssa",
        ["lat"] = "lat"
    };

    public static bool TryResolve(string? countryOrMarket, out string marketCode)
    {
        marketCode = "";
        if (string.IsNullOrWhiteSpace(countryOrMarket))
        {
            return false;
        }

        return Map.TryGetValue(countryOrMarket.Trim(), out marketCode!);
    }
}
