namespace Geronimus.Text.Tests;

[TestClass]
public class NameKeyConstructorTests
{
    [TestMethod]
    [DataRow( null )]
#nullable disable
    [DataRow( new string[] { "every", "good", null, "deserves", "favour" } )]
#nullable restore
    public void RejectsNullNames( string[] badArgs )
    {
        Assert.ThrowsException<ArgumentNullException>(
            () => { new NameKey( badArgs ); }
        );
    }

    [TestMethod]
    public void RejectsEmptyNamesArrays()
    {
        Assert.ThrowsException<ArgumentException>(
            () => { new NameKey( new string[] {} ); }
        );
    }

    [TestMethod]
    [DataRow( new string[] { "windows\r\nstyle" } )]
    [DataRow( new string[] { "unx\nstyle" } )]
    [DataRow( new string[] { "user", "My\r\nCollection", "item" } )]
    public void RejectsMultilineNames( string[] badArgs )
    {
        Assert.ThrowsException<ArgumentException>(
            () => { new NameKey( badArgs ); }
        );
    }

    [TestMethod]
    [DataRow( new string[] { "" } )]
    [DataRow( new string[] { "   " } )]
    [DataRow( new string[] { "\u00a0 \u00a0" } )]
    [DataRow( new string[] { "\t" } )]
    [DataRow( new string[] { "\ufeff  \u00a0\t  \ufeff" } )]
    [DataRow( new string[] { "am", "stram", "\u00a0 \u00a0", "colégram" } )]
    public void RejectsBasicallyEmptyNames( string[] badArgs )
    {
        Assert.ThrowsException<ArgumentException>(
            () => { new NameKey( badArgs ); }
        );
    }

    [TestMethod]
    public void SilentlySwallowsUnprintableControlCharacters()
    {
        NameKey example = new NameKey(
            new string[] {
                "\u0098\u009dHumphrey\u001f Bogart\u009c\u0000",
                "\u0098Lauren\u001e Bacall\u000e\u001b\u009c",
                "\u0098\u001a\u009eMartha \u000fVickers\u0015\u009e\u009c",
                "\u0080\u0011Charles\u0082 Waldron\u0094\u0080\u009c"
            }
        );

        CollectionAssert.AreEqual(
            new List<string>(
                new string[] {
                    "Humphrey Bogart",
                    "Lauren Bacall",
                    "Martha Vickers",
                    "Charles Waldron"
                }
            ),
            example.NameSeq
        );
    }

    [TestMethod]
    [DataRow( new string[] { "user", "datatypes/url" } )]
    [DataRow( new string[] { "user/datatypes", "url" } )]
    [DataRow( new string[] { "/user/datatypes/", "/url/" } )]
    [DataRow( new string[] { "/user/", "/datatypes/", "/url/" } )]
    [DataRow( new string[] { "/user/datatypes/url/" } )]
    [DataRow( new string[] { "/user//datatypes//url/" } )]
    [DataRow( new string[] { "user/datatypes/url" } )]
    public void IfANameContainsASlash_ItIsParsedAsTwoNames( string[] args )
    {
        NameKey example = new NameKey( args );

        Assert.AreEqual<int>(
            3,
            example.NameSeq.Count
        );

        CollectionAssert.AreEqual(
            new List<string>(
                new string[] { "user", "datatypes", "url" }
            ),
            example.NameSeq
        );

        Assert.AreEqual( "user/datatypes/url", example.TextValue );
    }
}
