namespace Geronimus.Text.Tests;

[TestClass]
public class EmptyCharacterSeqMethodTests
{
    [TestMethod]
    [DataRow( -1 )]
    [DataRow( 0 )]
    [DataRow( 1 )]
    public void CharacterAt_AnyIndexValueThrows( int indexValue )
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => { CharacterSeq.Empty.CharacterAt( indexValue ); }
        );
    }

    [TestMethod]
    public void Enumerator_CanEnumerateButThereAreNoMembers()
    {
        int counter = 0;

        foreach ( string character in CharacterSeq.Empty )
        {
            counter += 1;
        }
        Assert.AreEqual<int>( 0, counter );
    }

    [TestMethod]
    public void Equals_IsOnlyEqualToItself()
    {
        Assert.AreNotEqual( CharacterSeq.Empty, string.Empty );
        Assert.AreEqual( CharacterSeq.Empty, CharacterSeq.Empty );
    }

    [TestMethod]
    [DataRow( true )]
    [DataRow( false )]
    public void Filter_ReturnsEmptyCharacterSeqNoMatterWhat( bool boolVal )
    {
        Assert.AreEqual<ICharacterSeq>(
            CharacterSeq.Empty,
            CharacterSeq.Empty.Filter( character => { return boolVal; } )
        );
    }

    [TestMethod]
#nullable disable
    [DataRow( null )]
#nullable restore
    [DataRow( "" )]
    [DataRow( "x" )]
    public void Map_ReturnsEmptyCharacterSeqNoMatterWhat( string val )
    {
        Assert.AreEqual<ICharacterSeq>(
            CharacterSeq.Empty,
            CharacterSeq.Empty.Map( ( string ch ) => { return val; } )
        );
    }

    [TestMethod]
    public void ToString_ReturnsEmptyString()
    {
        Assert.AreEqual(
            string.Empty,
            CharacterSeq.OfText( string.Empty ).ToString(),
            false
        );
    }
}
