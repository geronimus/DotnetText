namespace Geronimus.Text.Tests;

[TestClass]
public class CharacterSeqBuilderTests
{
    [TestMethod]
    public void Empty_AlwaysReturnsTheSameEmptyInstance()
    {
        ICharacterSeq one = CharacterSeq.Empty;
        ICharacterSeq two = CharacterSeq.Empty;

        Assert.IsTrue( one.IsEmpty );
        Assert.IsTrue( two.IsEmpty );
        Assert.IsTrue( one == two );
    }

    [TestMethod]
    public void Empty_ReturnsTheSameInstanceAsEmptyStringCharacterSeq()
    {
        Assert.IsTrue(
            CharacterSeq.Empty == CharacterSeq.OfText( "" )
        );
    }

    [TestMethod]
    public void OfText_GivenNull_ReturnsAnEmptySeq()
    {
    #nullable disable
        Assert.IsTrue( CharacterSeq.OfText( null ).IsEmpty );
    #nullable restore
    }

    [TestMethod]
    public void OfText_GivenAnEmptyString_ReturnsEmptySeq()
    {
        Assert.IsTrue( CharacterSeq.OfText( string.Empty ).IsEmpty );
    }

    [TestMethod]
    public void OfText_GivenANonEmptyString_ReturnsANonEmptySeq()
    {
        Assert.IsFalse( CharacterSeq.OfText( "x" ).IsEmpty );
    }
}