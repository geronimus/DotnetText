using System.Collections;
using System.Globalization;

namespace Geronimus.Text;

public interface ICharacterSeq : IEnumerable<string>
{
    public string this[ int index ] { get; }
    public string Head { get; }
    public bool IsEmpty { get; }
    public int Length { get; }
    public ICharacterSeq Tail { get; }
    public string Text { get; }

    public string CharacterAt( int index );

    public ICharacterSeq Filter( Func<string, bool> condition );

    public ICharacterSeq Map( Func<string, string> transformation );
}

public static class CharacterSeq
{
    public static ICharacterSeq Empty = new EmptyCharacterSeq();

    public static ICharacterSeq OfText( string text )
    {
        if ( text == null || text.Equals( string.Empty ) )
            return Empty;
        else
            return new ValuedCharacterSeq( text );
    }

    public class CharacterSeqEnumerator : IEnumerator<string>
    {
        private int _currentIndex = -1;
        private ICharacterSeq _seq;

        public CharacterSeqEnumerator( ICharacterSeq seq )
        {
            _seq = seq;
        }

        public string Current
        {
            get
            {
                if ( 
                    _seq.IsEmpty ||
                        _currentIndex < 0 ||
                        _currentIndex >= _seq.Length
                )
                {
                    throw new InvalidOperationException(
                        $"Attempted to access item { _currentIndex } of a " +
                            $"CharacterSeq containing { _seq.Length } elements."
                    );
                }
                else
                {
                    return _seq[ _currentIndex ];
                }
            }
        }

        private object current { get { return this.Current; } }

        object IEnumerator.Current { get { return current; } }

        public void Dispose()
        {
            Dispose( disposing: true );
            GC.SuppressFinalize( this );
        }

        protected virtual void Dispose( bool disposing ) {}

        public bool MoveNext()
        {
            if ( _seq.IsEmpty || _currentIndex >= ( _seq.Length - 1 ) )
            {
                return false;
            }
            else
            {
                _currentIndex += 1;
                return true;
            }
        }

        public void Reset()
        {
            _currentIndex = -1;
        }

        ~CharacterSeqEnumerator()
        {
            Dispose( disposing: false );
        }
    }

    private sealed class EmptyCharacterSeq : ICharacterSeq, IEquatable<ICharacterSeq>
    {
        public string Head => string.Empty;
        public bool IsEmpty => true;
        public int Length => 0;
        public ICharacterSeq Tail => this;
        public string Text => "";

        public string this[ int index ] => CharacterAt( index );

        public string CharacterAt( int index )
        {
            throw new ArgumentOutOfRangeException(
                "This is an empty Character Sequence. " +
                    "It contains no characters."
            );
        }

        public override bool Equals( object? obj ) =>
            obj is ICharacterSeq that && this.Equals( that );

        public bool Equals( ICharacterSeq? that ) =>
            that != null && string.Empty.Equals( that.ToString() );

        public ICharacterSeq Filter( Func<string, bool> condition )
        {
            return CharacterSeq.Empty;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return new CharacterSeqEnumerator( this );
        }

        private IEnumerator getEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return getEnumerator();
        }

        public override int GetHashCode() => string.Empty.GetHashCode();

        public ICharacterSeq Map( Func<string, string> transformation )
        {
            return CharacterSeq.Empty;
        }

        public override string? ToString()
        {
            return string.Empty;
        }
    }

    private sealed class ValuedCharacterSeq : ICharacterSeq, IEquatable<ICharacterSeq>
    {
        private List<CharacterPosition> _characters = new();
        private string _head = string.Empty;
        private string _text;

        public ValuedCharacterSeq( string text )
        {
            _text = text.Normalize();

            TextElementEnumerator cursor =
                StringInfo.GetTextElementEnumerator( _text );
            
            cursor.Reset();
            while ( cursor.MoveNext() )
            {
                string textElem = cursor.GetTextElement();

                _characters.Add(
                    new CharacterPosition(
                        cursor.ElementIndex,
                        textElem.Length
                    )
                );

                if ( _characters.Count == 1 )
                    _head = textElem;
            }
        }

        public string this[ int index ] => CharacterAt( index );
        public string Head => _head;
        public bool IsEmpty => false;
        public int Length => _characters.Count;
        public ICharacterSeq Tail
        {
            get {
                if ( _characters.Count > 1 )
                {
                    return CharacterSeq.OfText(
                        _text.Substring( _characters[ 1 ].start )
                    );
                }
                else
                {
                    return CharacterSeq.Empty;
                }
            }
        }
        public string Text
        {
            get { return _text; }
        }

        public string CharacterAt( int index )
        {
            int seqMax = _characters.Count - 1;

            if ( index < 0 || index > seqMax )
                throw new ArgumentOutOfRangeException(
                    $"For a Character Sequence with Length { seqMax + 1 }, " +
                        $"the index value be between 0 and { seqMax } " +
                        "inclusive."
                );

            return _text.Substring(
                _characters[ index ].start,
                _characters[ index ].length
            );
        }

        public override bool Equals( object? obj ) =>
            obj is ICharacterSeq that && this.Equals( that );

        public bool Equals( ICharacterSeq? that ) =>
            that != null && this.ToString().Equals( that.ToString() );

        public ICharacterSeq Filter( Func<string, bool> condition )
        {
            if ( condition == null )
                return this;

            string newText = "";

            foreach ( string character in this )
            {
                if ( condition( character ) )
                    newText += character;
            }

            return CharacterSeq.OfText( newText );
        }

        public IEnumerator<string> GetEnumerator()
        {
            return new CharacterSeqEnumerator( this );
        }

        private IEnumerator getEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return getEnumerator();
        }

        public override int GetHashCode() => _text.GetHashCode();

        public ICharacterSeq Map( Func<string, string> transformation )
        {
            string newText = "";

            foreach( string character in this )
            {
                newText += transformation( character );
            }

            return CharacterSeq.OfText( newText );
        }

        public override string ToString()
        {
            return _text;
        }

        private record CharacterPosition( int start, int length );
    }
}
