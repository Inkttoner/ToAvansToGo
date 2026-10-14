using ToAvansToGo.Domain.Enums;

namespace ToAvansToGo.Web.Helpers;

public static class CityExtensions
{
    // Toont de stadsnaam zoals je die op de site wilt zien ("Denbosch" wordt "Den Bosch")
    public static string ToDisplayName(this City city) =>
        city == City.Denbosch ? "Den Bosch" : city.ToString();
}
