namespace Geronimus.Text.Tests;

[TestClass]
public class ValuedCharacterSeqMethodTests
{
    [TestMethod]
    public void CharacterAt_GivenIndexLessThanZero_Throws()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => { CharacterSeq.OfText( "x" ).CharacterAt( -1 ); }
        );
    }
    
    [TestMethod]
    [DataRow( "x" )]
    [DataRow( "123" )]
    [DataRow( "10\u2009000\u2004\u20ac" )] // "10 000 €", as it might be
                                           // recorded in France, for example.
    public void CharacterAt_GivenIndexGreaterThanMax_Throws( string val )
    {
        ICharacterSeq example = CharacterSeq.OfText( val );

        Assert.ThrowsException<ArgumentOutOfRangeException>(
            // The max index should be .Length minus 1.
            () => { example.CharacterAt( example.Length ); }
        );
    }

    [TestMethod]
    public void CharacterAt_RetrievesFullCharactersRatherThanBytes()
    {
        Assert.AreEqual(
            "\u20ac",
            CharacterSeq.OfText( "10\u2009000\u2004\u20ac" ).CharacterAt( 7 )
        );
        // This example using combining characters to construct a "c" with a
        // cedilla diacritic, but the result at index 2 should be normalized
        // to the single Unicode character "Latin Small Letter C with cedilla".
        Assert.AreEqual(
            "\u00e7",
            CharacterSeq.OfText( "Fac\u0327ade" ).CharacterAt( 2 )
        );
        // Show me a rose... or leave me alone!
        Assert.AreEqual(
            "\ud83c\udf39",
            CharacterSeq.OfText(
                // Three 2-byte characters.
                "\ud83e\udd78\ud83c\udf39\ud83e\uddf3"
            // We want the middle one.
            ).CharacterAt( 1 )
        );
    }

    [TestMethod]
    public void Enumerator_CanEnumerate()
    {
        List<string> characters = new();

        foreach (
            string character in CharacterSeq.OfText(
                "\ud83e\udd78\ud83c\udf39\ud83e\uddf3"
            )
        )
        {
            characters.Add( character );
        }
        Assert.AreEqual<string>( "\ud83e\udd78", characters[ 0 ] );
        Assert.AreEqual<string>( "\ud83c\udf39", characters[ 1 ] );
        Assert.AreEqual<string>( "\ud83e\uddf3", characters[ 2 ] );
    }

    [TestMethod]
    public void Equals_CharacterSequencesWithTheSameValueAreEqual()
    {
        Assert.AreEqual<ICharacterSeq>(
            CharacterSeq.OfText( "x" ),
            CharacterSeq.OfText( "x" )
        );
        // Test normalization of combining glyphs versus character:
        Assert.AreEqual<ICharacterSeq>(
            CharacterSeq.OfText(
                new String( new char[] { '\u0061', '\u0308' } ) 
            ),
            CharacterSeq.OfText( "\u00e4" )
        );
        // Test equality of combining chars versus the final string value:
        Assert.AreEqual<ICharacterSeq>(
            CharacterSeq.OfText(
                new String( new char[] { '\ud83e', '\udd78' } )
            ),
            CharacterSeq.OfText( "🥸" )
        );
    }
    
    [TestMethod]
    public void Filter_GivenNull_ReturnsTheSameSeq()
    {
        ICharacterSeq example = CharacterSeq.OfText( "By default" );
    # nullable disable
        Assert.AreEqual<ICharacterSeq>( example, example.Filter( null ) );
    # nullable restore
    }

    [TestMethod]
    public void Filter_CanOperateOnMultiByteCharacters()
    {
        // We're going to use a filter to remove the emojis from this text:
        ICharacterSeq before = CharacterSeq.OfText(
            "\ud83e\udd78 > Show me a \ud83c\udf39..."
        );
        ICharacterSeq after = CharacterSeq.OfText( " > Show me a ..." );
        // But we'll use a fairly brutal method that removes all UTF-16 multi-
        // byte characters.
        Func<string,bool> condition = ( string ch ) => ch.Length < 2;

        Assert.AreEqual<ICharacterSeq>( after, before.Filter( condition ) );
    }

    [TestMethod]
    public void Filter_CanReduceToEmpty()
    {
        Assert.AreEqual<ICharacterSeq>(
            CharacterSeq.Empty,
            CharacterSeq.OfText( "Hello, my honey...!" ).Filter(
                ( string ch ) => { return false; }
            )
        );
    }

    [TestMethod]
    public void Map_PerformsASortOfAllPurposeFlatMapCharacterReplacement()
    {
        ICharacterSeq before = CharacterSeq.OfText(
            "\ud83e\udd78 > Show me a \ud83c\udf39..."
        );
        ICharacterSeq after = CharacterSeq.OfText(
            "Groucho > Show me a rose..."
        );
        Func<string, string> mapping = ( string ch ) => {
            if ( ch == "\ud83e\udd78" )
                return "Groucho";
            else if ( ch == "\ud83c\udf39" )
                return "rose";
            else
                return ch;
        };

        Assert.AreEqual<ICharacterSeq>(
            after,
            before.Map( mapping )
        );
    }

    [TestMethod]
    public void Map_GivenALambdaThatReturnsAnEmptyString_CanReduceToEmpty()
    {
        Assert.AreEqual<ICharacterSeq>(
            CharacterSeq.Empty,
            CharacterSeq.OfText( "Hello, my honey...!" ).Map(
                ( string ch ) => { return ""; }
            )
        );
    }

    [TestMethod]
    [DataRow( "x" )]
    [DataRow( "I am the very model of a modern major general!" )]
    [DataRow(
        "«\u00a0Grâce à la pub, nous vendûmes le \u2116 88 pour la somme " +
            "estimée de 1\ufeff000\ufeff000\u00a0€.\u00a0»"
    )]
    public void ToString_ReturnsNormalizedInitalizerValue( string initValue )
    {
        Assert.AreEqual(
            initValue.Normalize(),
            CharacterSeq.OfText( initValue ).ToString(),
            false
        );
    }
}
