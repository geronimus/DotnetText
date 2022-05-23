namespace Geronimus.Text.Tests;

[TestClass]
public class NameKey_MethodTests
{
    [TestMethod]
    public void Append_GivenNullChildName_ThrowsArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(
    #nullable disable
            () => { new NameKey( "input/bad" ).Append( null ); }
    #nullable restore
        );
    }

    [TestMethod]
    [DataRow( "" )]
    [DataRow( "  \u00a0\ufeff  " )]
    [DataRow( "my\r\nbad" )]
    public void Append_GivenBadChildName_ThrowsArgumentException(
        string badArg
    )
    {
        Assert.ThrowsException<ArgumentException>(
            () => { new NameKey( "input/bad" ).Append( badArg ); }
        );
    }

    [TestMethod]
    public void Append_GivenGoodChildName_ReturnsNewChildNameKey()
    {
        NameKey parent = new NameKey( "user/hues" );
        NameKey azure = parent.Append( "Azure" );

        Assert.AreEqual( "user/hues/Azure", azure.TextValue );
        Assert.AreEqual<NameKey>( parent, azure.Context );
    }

    [TestMethod]
    public void Concat_GivenNullSuffix_ThrowsArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(
            () => {
                new NameKey( "every", "good", "boy", "deserves" )
            #nullable disable
                    .Concat( null );
            #nullable restore
            }
        );
    }

    [TestMethod]
    public void Concat_GivenValidSuffix_ReturnsNewCompositeNameKey()
    {
        NameKey prefix = new NameKey( "user/lists/street type" );
        NameKey suffix = new NameKey( "items/Avenue" );
        NameKey example = prefix.Concat( suffix );

        Assert.AreEqual(
            "user/lists/street type/items/Avenue",
            example.TextValue
        );
    }

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
