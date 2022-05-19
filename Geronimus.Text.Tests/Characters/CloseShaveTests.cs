namespace Geronimus.Text.Tests;

[TestClass]
public class CloseShaveTests
{
    [TestMethod]
#nullable disable
    [DataRow( null )]
#nullable restore
    [DataRow( "" )]
    [DataRow( "\r\n" )]
    [DataRow( "\t \u00a0\u202f\u2009\u200a\ufeff" )]
    public void GivenEmptyValues_ReturnsTheEmptyString( string basicallyEmpty )
    {
        Assert.AreEqual(
            string.Empty,
            Characters.CloseShave( basicallyEmpty )
        );
    }

    [TestMethod]
    public void LeavesTextsWithoutSurroundingSpaceAlone()
    {
        Assert.AreEqual(
            "That Puma from Creature Comforts",
            Characters.CloseShave( "That Puma from Creature Comforts" )
        );
    }

    [TestMethod]
    public void RemovesOuterSpacesButNotInnerOnes()
    {
        Assert.AreEqual(
            "Groucho Marx",
            Characters.CloseShave( " \u00a0\u2004Groucho Marx\u2004\u00a0 " )
        );
    }

    [TestMethod]
    public void RemovesOuterButNotInnerLineBreaks()
    {
        Assert.AreEqual(
            "Page One\r\n\u000cPageTwo",
            Characters.CloseShave( "\u2029\r\nPage One\r\n\u000cPageTwo\r\n" )
        );
    }
}