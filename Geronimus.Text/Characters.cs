namespace Geronimus.Text;
public static class Characters
{
    public static ISet<string> Spaces = new HashSet<string>(
        Blanks.Set.Union<string>( LineEndings.Set )
    );

    public static string CloseShave( string text )
    {
        return CloseShaveStart( CloseShaveEnd ( text ) );
    }

    public static string CloseShaveEnd( string text )
    {
        if ( string.IsNullOrEmpty( text ) )
            return string.Empty;

        ICharacterSeq seq = CharacterSeq.OfText( text );
        bool foundStart = false;
        string shavedText = "";

        for ( int item = ( seq.Length - 1 ); item > -1 ; item-- )
        {
            if ( !Spaces.Contains( seq[ item ] ) )
            {
                if ( !foundStart )
                    foundStart = true;

                shavedText = seq[ item ] + shavedText;
            }
            else
            {
                if ( foundStart )
                    shavedText = seq[ item ] + shavedText;
            }
        }

        return shavedText;
    }

    public static string CloseShaveStart( string text )
    {
        if ( string.IsNullOrEmpty( text ) )
            return string.Empty;

        ICharacterSeq seq = CharacterSeq.OfText( text );
        bool foundStart = false;
        string shavedText = "";

        for ( int item = 0; item < seq.Length; item++ )
        {
            if ( !Spaces.Contains( seq[ item ] ) )
            {
                if ( !foundStart )
                    foundStart = true;

                shavedText += seq[ item ];
            }
            else
            {
                if ( foundStart )
                    shavedText += seq[ item ];
            }
        }

        return shavedText;
    }

    public static string NormalizeLineEndings( string text )
    {
        if ( text == null )
            return string.Empty;

        List<string> toReplace = new List<string>(
            new string[]
            {
                LineEndings.Windows,
                LineEndings.CarriageReturn,     // Pre-X macOS-style
                LineEndings.LineSeparator,
                LineEndings.ParagraphSeparator
            }
        );
        string result = text;

        toReplace.ForEach(
            lineEnd =>
            {
                result = result.Replace( lineEnd, LineEndings.LineFeed );
            }
        );

        return result;
    }

    public static string WindowsLineEndings( string text )
    {
        if ( text == null )
            return string.Empty;
        else
            return NormalizeLineEndings( text ).Replace(
                LineEndings.Unix,
                LineEndings.Windows
            );
    }

    public static class Blanks
    {
        public const string EmQuad = "\u2001";
        public const string EmSpace = "\u2003";
        public const string EnQuad = "\u2000";
        public const string EnSpace = "\u2002";
        public const string FigureSpace = "\u2007";
        public const string HairSpace = "\u200a";
        public const string IdeographicSpace = "\u3000";
        public const string MediumMathematicalSpace = "\u202f";
        public const string MidSpace = "\u2005";
        public const string NarrowNoBreakSpace = "\u202f";
        public const string NonBreakingSpace = "\u00a0";
        public const string PunctuationSpace = "\u2008";
        public const string SixPerEmSpace = "\u2006";
        public const string Space = " ";
        public const string Tab = "\t";
        public const string ThickSpace = "\u2004";
        public const string ThinSpace = "\u2009";
        // According to Unicode, White_Space=no for ZeroWidthNoBreakSpace, but
        // we're leaving it here because of the ECMAScript Standard.
        // (https://262.ecma-international.org/12.0/#prod-WhiteSpace)
        public const string ZeroWidthNoBreakSpace = "\ufeff";
        public static readonly ISet<string> Set = new HashSet<string>(
            new string[]
            {
                EmQuad,
                EmSpace,
                EnQuad,
                EnSpace,
                FigureSpace,
                HairSpace,
                IdeographicSpace,
                MediumMathematicalSpace,
                MidSpace,
                NarrowNoBreakSpace,
                NonBreakingSpace,
                PunctuationSpace,
                SixPerEmSpace,
                Space,
                Tab,
                ThickSpace,
                ThinSpace,
                ZeroWidthNoBreakSpace
            }
        );
    }

    public static class LineEndings
    {
        public const string CarriageReturn = "\r";
        public const string FormFeed = "\u000c";
        public const string LineFeed = "\n";
        public const string LineSeparator = "\u2028";
        public const string NextLine = "";
        public const string ParagraphSeparator = "\u2029";
        public const string VerticalTab = "\u000b";
        public const string Unix = LineFeed;
        public const string Windows = CarriageReturn + LineFeed;
        public static readonly ISet<string> Set = new HashSet<string>(
            new string[]
            {
                CarriageReturn,
                FormFeed,
                LineFeed,
                LineSeparator,
                ParagraphSeparator,
                VerticalTab,
                Windows
            }
        );
    }
}
