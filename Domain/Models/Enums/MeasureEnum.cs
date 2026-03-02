namespace Domain.Models.Enums;

public enum Measure
{
    Gram = 0,
    Hectogram = 1,
    Kilogram = 2,

    Milliliter = 10,
    Deciliter = 11,
    Liter = 12,

    Cup = 20,
    Teaspoon = 21,
    Tablespoon = 22,

    Ounce = 30,
    Pound = 31,
    
    Piece = 40,  // for items like "1 medium onion"
    Slice = 41,
    Clove = 42
}

public static class MeasureExtensions
{
    public static string ToSwedishTranslation(this Measure measure)
    {
        return measure switch
        {
            Measure.Gram => "Gram",
            Measure.Hectogram => "Hektogram",
            Measure.Kilogram => "Kilogram",
            Measure.Milliliter => "Milliliter",
            Measure.Deciliter => "Deciliter",
            Measure.Liter => "Liter",
            Measure.Cup => "Kopp",
            Measure.Teaspoon => "Tesked",
            Measure.Tablespoon => "Matsked",
            Measure.Ounce => "Uns",
            Measure.Pound => "Pund",
            Measure.Piece => "Stycke",
            Measure.Slice => "Skiva",
            Measure.Clove => "Klyfta",
            _ => "Okänt mått"
        };
    }
}