namespace CSESoftware.Selenium;

public static class XunitExtensions
{
    public static IEnumerable<object[]> AsObjectArray<T>(this List<T> objects)
    {
        return objects.Select(o => new object[] { o });
    }
}
