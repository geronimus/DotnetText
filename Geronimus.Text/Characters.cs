namespace Geronimus.Text;
public static class Characters
{
    public const char CarriageReturn = '\r';
    public const char LineFeed = '\n';
    public const char LineSeparator = '\u2028';
    public const char ParagraphSeparator = '\u2029';

    public static readonly ISet<char> LineEndings = new HashSet<char>(
        new char[]
        {
            CarriageReturn,
            LineFeed,
            LineSeparator,
            ParagraphSeparator
        }
    );

    public static string NormalizeLineEndings( string text )
    {
        if ( text == null )
            return string.Empty;

        List<string> toReplace = new List<string>(
            new string[]
            {
                $"{ CarriageReturn }{ LineFeed }", // Windows-style
                $"{ CarriageReturn }",             // Pre-X macOS-style
                $"{ LineSeparator }",
                $"{ ParagraphSeparator }"
            }
        );
        string result = text;

        toReplace.ForEach(
            lineEnd =>
            {
                result = result.Replace( lineEnd, LineFeed.ToString() );
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
                $"{ LineFeed }",
                $"{ CarriageReturn }{ LineFeed }"
            );
    }
}
