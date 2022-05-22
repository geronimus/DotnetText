namespace Geronimus.Text.Tests;

[TestClass]
public class NameKey_MethodTests
{
    [TestMethod]
    public void Equals_IsBasedOnAKeysTextValue()
    {
        NameKey example1 = new NameKey( "user", "datatypes", "url" );
        NameKey example2 = new NameKey( "user/datatypes/url" );

        Assert.AreEqual<NameKey>( example2, example1 );
    }

    [TestMethod]
    public void Equals_NameValuesGetNormalizedForComparison()
    {
        NameKey oneCharCedilla = new NameKey( "langues/fran\u00e7ais" );
        NameKey combiningCedilla = new NameKey( "langues/franc\u0327ais" );

        Assert.AreEqual<NameKey>( oneCharCedilla, combiningCedilla );
        Assert.AreEqual( oneCharCedilla.TextValue, combiningCedilla.TextValue );
    }

    [TestMethod]
    public void ToString_HasAFormThatShowsItsValue()
    {
        Assert.AreEqual(
            "NameKey { user/datatypes/url }",
            new NameKey( "user", "datatypes", "url" ).ToString()
        );
    }
}
