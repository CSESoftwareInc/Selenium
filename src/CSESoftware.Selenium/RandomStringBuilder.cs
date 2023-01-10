namespace CSESoftware.Selenium;

public class RandomStringBuilder
{
    public RandomStringBuilder()
    {
        RandomNumberGenerator = new Random(Guid.NewGuid().GetHashCode());
    }

    private Random RandomNumberGenerator { get; }
    private const string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
    private const string Numbers = "0123456789";
    private const string SpecialCharacters = "0123456789";
    private string CustomCharacters = "";

    private int Length { get; set; }
    private bool UseUppercaseLetters { get; set; }
    private bool UseLowercaseLetters { get; set; }
    private bool UseNumbers { get; set; }
    private bool UseSpecialCharacters { get; set; }
    private bool UseCustomCharacters => !string.IsNullOrEmpty(CustomCharacters);

    public RandomStringBuilder WithLength(int length)
    {
        Length = length;
        return this;
    }

    public RandomStringBuilder WithUppercaseLetters()
    {
        UseUppercaseLetters = true;
        return this;
    }

    public RandomStringBuilder WithLowercaseLetters()
    {
        UseLowercaseLetters = true;
        return this;
    }

    public RandomStringBuilder WithNumbers()
    {
        UseNumbers = true;
        return this;
    }

    public RandomStringBuilder WithSpecialCharacters()
    {
        UseSpecialCharacters = true;
        return this;
    }

    public RandomStringBuilder WithCustomCharacters(string customCharacters)
    {
        CustomCharacters = customCharacters;
        return this;
    }

    public string Generate()
    {
        if (Length <= 0) return "";

        var availableCharacters = GetAvailableCharacters();

        if (!availableCharacters.Any()) return "";

        return GenerateRandomString(availableCharacters);
    }

    private string GenerateRandomString(string availableCharacters)
    {
        var randomCharacters = Enumerable.Range(0, Length)
            .Select(x => GetNextCharacter(availableCharacters));

        return string.Join("", randomCharacters);
    }

    private string GetAvailableCharacters()
    {
        var availableCharacters = "";
        if (UseUppercaseLetters) availableCharacters += UppercaseLetters;
        if (UseLowercaseLetters) availableCharacters += LowercaseLetters;
        if (UseNumbers) availableCharacters += Numbers;
        if (UseSpecialCharacters) availableCharacters += SpecialCharacters;
        if (UseCustomCharacters) availableCharacters += CustomCharacters;

        return availableCharacters;
    }

    private char GetNextCharacter(string availableCharacters)
    {
        return availableCharacters[RandomNumberGenerator.Next(0, availableCharacters.Length)];
    }
}
