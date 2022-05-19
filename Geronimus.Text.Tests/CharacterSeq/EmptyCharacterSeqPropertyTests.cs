namespace Geronimus.Text.Tests;

[TestClass]
public class EmptyCharacterSeqPropertyTests
{
    [TestMethod]
    public void Length_ReturnsZero()
    {
        Assert.AreEqual<int>( 0, CharacterSeq.Empty.Length );
    }

    [TestMethod]
    public void Head_ReturnsTheEmptyString()
    {
        Assert.AreEqual(
            string.Empty,
            CharacterSeq.Empty.Head,
            false
        );
    }

    [TestMethod]
    public void Tail_ReturnsTheEmptyCharacterSeq()
    {
        Assert.AreEqual<ICharacterSeq>(
            CharacterSeq.Empty,
            CharacterSeq.Empty.Tail
        );
    }
}