namespace Geronimus.Text.Tests;

[TestClass]
public class ValuedCharacterSeqPropertyTests
{
    [TestMethod]
    public void Length_ReturnsTheNumberOfLogicalCharacters()
    {
        Assert.AreEqual<int>( 1, CharacterSeq.OfText( "x" ).Length );
        Assert.AreEqual<int>( 3, CharacterSeq.OfText( "123" ).Length );
        Assert.AreEqual<int>(
            7,
            // 1 000 € - Mille euros en format français.
            // (Attention les &nbsp; !)
            CharacterSeq.OfText( "1\u00a0000\u00a0\u20ac" ).Length
        );
    }

    [TestMethod]
    [DataRow( "x" )]
    [DataRow( "\u2116" )]
    public void Head_ReturnsTheFirstFullCharacter( string exampleText )
    {
        Assert.AreEqual(
            exampleText,
            CharacterSeq.OfText( exampleText ).Head,
            false
        );
    }

    [TestMethod]
    public void Tail_ReturnsACharacterSeqOfTheRemainingCharacters()
    {
        Assert.AreEqual<ICharacterSeq>(
            CharacterSeq.Empty,
            CharacterSeq.OfText( "x" ).Tail
        );
        Assert.AreEqual<ICharacterSeq>(
            CharacterSeq.OfText( "23" ),
            CharacterSeq.OfText( "123" ).Tail
        );
        Assert.AreEqual<ICharacterSeq>(
            CharacterSeq.OfText( "\u00a01\u00a0000" ),
            // € 1 000 - One thousand euros with the almost Irish format.
            // (I've kept the French &nbsp; separators.)
            CharacterSeq.OfText( "\u20ac\u00a01\u00a0000" ).Tail
        );
    }
}