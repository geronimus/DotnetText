namespace Geronimus.Text;

public class NameKey : IEquatable<NameKey>
{ 
    // Fields:
    private NameKey? _contextNameKey = null;
    private readonly List<string> _names;
    private const string _separator = "/";
    private readonly string _textValue;

    // Constructor:
    public NameKey( params string[] names )
    {
        _names = validate( names );
        _textValue = string.Join( _separator, _names );
    }

    // Properties:
    public NameKey Context
    {
        get {
            if ( _names.Count == 1 )
            {
                throw new InvalidOperationException(
                    "A root NameKey does not have a Context property."
                );
            }
            else if ( _contextNameKey == null )
            {
                _contextNameKey = new NameKey(
                    _names.GetRange( 0, _names.Count - 1 ).ToArray()
                );
            }

            return _contextNameKey;
        }
    }

    public bool HasContext => _names.Count > 1;

    public bool IsRoot => _names.Count == 1;

    public string LeafName => _names[ _names.Count - 1 ];

    public List<string> NameSeq => new List<string>( _names.ToArray() );
    
    public string TextValue => _textValue;

    // Methods:
    public override bool Equals( object? obj ) => Equals( obj as NameKey );

    public bool Equals( NameKey? that )
    {
        if ( that == null )
            return false;
        else
            return _textValue.Equals( that.TextValue );
    }

    public override int GetHashCode() => _textValue.GetHashCode();

    public override string ToString() =>
        typeof( NameKey ).Name + " { " + _textValue + " }";

    private List<string> validate( string[] names )
    {
        if ( names == null )
            throw new ArgumentNullException( nameof( names ) );
            
        return cleanAndValidate( names );

        List<string> cleanAndValidate( string[] names )
        {
            List<string> namesList = resolveNamesList( names );

            if ( namesList.Count < 1 )
            {
                throw new ArgumentException(
                    "You must provide at least one name."
                );
            }

            List<string> result = new();

            for ( int item = 0; item < namesList.Count; item++ )
            {
                string itemRef = $"{ nameof( namesList ) }[ { item } ]";
                
                // Note that there is no need to normalize the name as a
                // separate step, because both Characters.CloseShave and any
                // CharacterSeq method have the side-effect of normalizing the
                // value.
                string cleanName = CharacterSeq.OfText(
                    Characters.CloseShave(
                        Characters.NormalizeLineEndings( namesList[ item ] )
                    )
                ).Filter(
                    ( string ch ) => !isUnprintableControlChar( ch )
                ).Text;

                if ( cleanName.Length < 1 )
                {
                    throw new ArgumentException(
                        "Names must have at least one non-space character.",
                        itemRef
                    );
                }
                else if (
                    cleanName.IndexOf(
                        Characters.LineEndings.LineFeed
                    ) > -1
                )
                {
                    throw new ArgumentException(
                        "Names must not span multiple lines.",
                        itemRef
                    );
                }
                else
                {
                    result.Add( cleanName );
                }
            }

            return result;

            bool isUnprintableControlChar( string elem )
            {
                if ( elem.Length > 0 )
                {
                    char charValue = elem[ 0 ];

                    return ( charValue < 9 ) ||
                        ( charValue >= 14 && charValue < 32 ) ||
                        ( charValue >= 127 && charValue < 160 );
                }
                else
                {
                    return false;
                }
            }

            List<string> resolveNamesList( string[] names )
            {
                List<string> result = new();

                for ( int item = 0; item < names.Length; item++ )
                {
                    if ( names[ item ] == null )
                    {
                        throw new ArgumentNullException(
                            $"{ nameof( names ) }[ { item } ]"
                        );
                    }
                    else
                    {
                        string[] splitNames = names[ item ].Split(
                            _separator,
                            StringSplitOptions.RemoveEmptyEntries
                        );

                        foreach ( string splitNameItem in splitNames )
                        {
                            result.Add( splitNameItem );
                        }
                    }
                }

                return result;
            }
        }
    }
}
