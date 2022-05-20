# Geronimus.Text Namespace

⚠️ _Under Construction_ ⚠️

A library of utility functions to help you work with some of the unpredictable text values you might encounter in the wild.

---

# Contents

- Interfaces
    - [ICharacterSeq](#ICharacterSeq-Interface)
        - Instance Properties
            - Head
            - IsEmpty
            - Length
            - Tail
        - Instance Methods
            - *indexer*
            - CharacterAt
            - Equals
            - Filter
            - GetEnumerator
            - GetHashCode
            - Map
            - ToString
- Classes
    - [Characters](#Characters-Class)
        - Static Fields
            - Blanks
            - LineEndings
        - Static Methods
            - CloseShave
            - CloseShaveEnd
            - CloseShaveStart
            - NormalizeLineEndings
            - WindowsLineEndings
    - [CharacterSeq](#CharacterSeq-Class)
        - Static Properties
            - Empty
        - Static Methods
            - OfText

---

# ICharacterSeq Interface

Character Sequence

## Definition

An object to help you iterate over the characters that make up a text or `string` value.

By characters, we mean logical characters, rather than `char` values. Multi-byte characters (such as characters from Asian scripts, dead languages, and emoji) are treated as one single atomic unit. (That's why we represent individual characters as `string` values and not `char`s.)

To get an instance of an `ICharacterSeq`, use the static class `CharacterSeq`'s `OfText` method.

An `ICharacterSeq` value's `Length` is the number of characters you might expect to find when you look at its graphical representation, rather than the number of `char` values it contains.

`ICharacterSeq` values of the same text compare as equal with the `Equals` method.

And when you create an `ICharacterSeq` of a string, the sequence will store only a normalized representation of the text, with combining diacritics fused into a single logical character. (See `String.Normalize()` in the .NET documentation.)

You can create an `ICharacterSeq` of the empty string. That is a special case, and it is the only  `ICharacterSeq` whose `IsEmpty` property will be set to `true`.

You can use the `Head` and `Tail` properties to iterate over the sequence of characters in a recursive functional style, in the way you might treat a linked list. You will know you have reached the end of the sequence when the current sequence (eg, the `Tail` of the previous iteration's sequence) has `IsEmpty` set to `true`. 

```c#
public interface ICharacterSeq : IEnumerable<string>
```

## Examples

```c#
string message = "👁️❤️🐑";
message.Length // => 4
message[ 0 ] // => '\ud83d' (Unprintable)

ICharacterSeq msgSeq = CharacterSeq.OfText( message );
msgSeq.Length // => 3
msgSeq[ 1 ] // => "❤️"
msgSeq.Head // => "👁️"
msgSeq.Tail // => "❤️🐑"
msgSeq.Filter( ( string ch ) => ch == "❤️" ); // => "❤️"
msgSeq.Map(
    ( string ch ) =>
    {
        if ( ch == "👁️" )
            return "I ";
        else if ( ch == "🐑" )
            return " you";
    }
); // => "I ❤️ you"
```

## Instance Properties

### `Head` *string*

Returns the first character in the sequence.

The `Head` and `Tail` properties are designed to help you iterate over the text in a recursive functional style, if that is your preference.

###  `IsEmpty` *bool*

If this property is true, it means the sequence contains no characters. Calling `Head` or `ToString()` on an empty sequence will return the empty `string`.

Calling `Tail` on an empty sequence will return the same empty sequence.

### `Length` *int*

Returns the number of logical characters in the sequence.

Remember that this is different from `string.Length`, which returns the number of `char` values contained in the `string`.

Because .NET uses the UTF-16 to represent text internally, some characters can be made up of more than one `char` value. And that fact is actually the primary motivation for providing this interface as a way of working with logical characters, rather than `char`s, which may only represent a partial character.

### `Tail` *ICharacterSeq*

Returns an `ICharacterSeq` containing all characters in the original sequence, except for the first.

The `Head` and `Tail` properties are designed to help you iterate over the text in a recursive functional style, if that is your preference.

If the `Tail` property of a sequence has its `IsEmpty` property set to `true`, that means you have reached the end of the sequence and there are no more characters.

If you call `Tail` on an already empty sequence, you get back the same empty sequence.

## Instance Methods

### *indexer*

You can use bracket notation to retrieve individual characters. This works in the same way as the `CharacterAt` method.

#### Example
```c#
ICharacterSeq msgSeq = CharacterSeq.OfText( "👁️❤️🐑" );

msgSeq[ 1 ]; // => "❤️"
```

### CharacterAt

```c#
public string CharacterAt( int index );
```

Retrieves the logical character at the specified index.

This will be a whole character, not one part of a UTF-16 surrogate pair.

The index is zero-based, but it refers to whole logical characters, rather than `char` values, which may only be part of a unicode surrogate pair.

#### Example
```c#
string message = "👁️❤️🐑";
message.Length; // => 4
message[ 0 ]; // => '\ud83d'

ICharacterSeq msgSeq = CharacterSeq.OfText( message );
msgSeq.Length; // => 3
msgSeq[ 0 ]; // => "👁️"
```

### Equals

```c#
public override bool Equals( object? obj );
```

Determines whether the other is an `ICharacterSeq` containing the same value as the current `ICharacterSeq`.

`ICharacterSeq` values containing equivalent `string` values should compare as equal.

However, please note that an `ICharacterSeq` stores a normalized internal representation of its text value. (See `string.Normalize()` in the .NET documentation.)

That means that if you create two sequences containing the same logical characters, but one where they were created using whole characters, and another where they were creating using combinators, then the two sequences will compare as equal. (Even though the equivalent `string`s will not.)

### Filter

```c#
public ICharacterSeq Filter( Func<string, bool> condition );
```

Alows you to define a function that takes a single `string` as an argument and returns a `bool` value.

`Filter` will call the function against each logical character that it contains, and return a new `ICharacterSeq` containing only those characters for which your function returned `true`.

#### Example
```c#
ICharacterSeq example = CharacterSeq.OfText( "The Lady\nFrom Shanghai" );

example.Filter(
    ( string item ) => item != "\n"
).ToString() // => "The LadyFrom Shanghai"
```

### GetEnumerator

```c#
public IEnumerator<string> GetEnumerator();
```

Allows you to iterate over the characters of an `ICharacterSeq` with a `foreach` statement.

### GetHashCode

```c#
public override int GetHashCode();
```

The hashcode produced will be the same as the hashcode of the `string` that the `ICharacterSeq` contains.

### Map

```c#
public ICharacterSeq Map( Func<string, string> transformation );
```

Allows you to provide a transformation function, which will be called on each character in sequence.

This function must return a `string`.

The results of calling the function on each of the sequence's characters will be combined into one text value, which will be returned as another `ICharacterSeq`.

#### Example
```c#
ICharacterSeq msgSeq = CharacterSeq.OfText( "👁️❤️🐑" );

msgSeq.Map(
    ( string ch ) =>
    {
        if ( ch == "👁️" )
            return "I ";
        else if ( ch == "🐑" )
            return " you";
    }
); // => "I ❤️ you"
```

### ToString

```c#
public override string ToString();
```

Returns the text value that this `ICharacterSeq` represents as a `string`.

Usually this is the same text value from which the sequence was originally made, but remember that it might not match exactly because any composite characters will have been normalized. (See `string.Normalize()` in the .NET documentation.)

---

# Characters Class

## Definition

Contains constants and methods useful for manipulating the characters within text values.

A text value is a `string`, but it is also a sequence of characters.

C# must use multiple `char` values to represent certain Unicode characters, such as many Asian scripts and also, of course, emoji.

But when we manipulate characters, we don't want to have to deal with that complexity. It would be better if we could treat each character as a single atomic unit.

In order to make that possible, this library represents characters as `string`s, rather than as `char`s.

```c#
public static class Characters
```

## Fields

### `Blanks` *Static Class*

Contains constants representing all of the spacing modifier characters that are not line breaks.

| Field | Definition |
| --- | --- |
| `EmQuad` | A space the width of one em. Equivalent to `EmSpace`, which is usually preferred over use of this character. |
| `EmSpace` | A space the width of one em. |
| `EnQuad` | A space the width of one en. Equivalent to `EnSpace`, which is usually preferred over use of this character. |
| `EnSpace` | A space the width of one en. |
| `FigureSpace` | A space equivalent to the width of a digit, in fonts that use monospaced characters to represent digits. |
| `HairSpace` | Even thinner than a `ThinSpace`. |
| `IdeographicSpace` | A space as wide as a Chinese, Japanese, or Korean (CJK) character cell. |
| `MediumMathematicalSpace` | A space that is four eighteenths of an em wide that is often used in mathematical formulae. |
| `MidSpace` | A space one fourth of an em wide, slightly narrower than a `ThickSpace`. |
| `NarrowNoBreakSpace` | A space about the width of a `ThinSpace`. However, no line breaks are permitted to replace this character. |
| `NonBreakingSpace` | An ordinary `Space`. However, no line breaks are permitted to replace this character. |
| `PunctuationSpace` | A space as wide as the narrow punctuation characters in a font, such as the comma or the full stop. |
| `SixPerEmSpace` | A space one sixth of an em wide. |
| `Space` | The usual space used between words on a latin script keyboard. |
| `Tab` | A horizontal space whose width can be determined by the user. |
| `ThickSpace` | A space one third of an em wide. |
| `ThinSpace` | A space one fifth to one sixth of an em wide, recommended for use as a thousands separator. |
| `ZeroWidthNoBreakSpace` | According to Unicode, this is not technically a spacing character, but we are leaving it in the list because the ECMAScript standard classifies it as one. |
| `Set` | *ISet\<string\>* The set of all of the spacing modifier characters listed above. |

### `LineEndings` *Static Class*

Contains constants representing line breaks and line terminators.

| Field | Definition |
| --- | --- |
| `CarriageReturn` | The signal to return the carriage of a typewriter or printer to its starting horizontal position. It is used as the first character of the line break sequence preferred by Windows. It was also the line break character used by MacOS versions before the first in the OS X series. |
| `FormFeed` | A signal for printers to skip to the end of the form or page. |
| `LineFeed` | A signal for printers to skip to the next line. It is used as the preferred line break character for Unix and Linux-derived systems, and it is the second character in the Windows preferred line break sequence. |
| `LineSeparator` | Indicates that the line has ended. |
| `NextLine` | A signal for printers to skip to the next line. |
| `ParagraphSeparator` | Indicates that the paragraph has ended. |
| `VerticalTab` | A vertical space whose width can be determined by the user. |
| `Unix` | The line ending used by Unix and Linux-derived systems. The same as the `LineFeed`. |
| `Windows` | The line ending preferred by Windows systems. It consists of a `CarriageReturn` followed by a `LineFeed`. Sometimes referred to as `\r\n` or `CrLf`. |
| `Set` | *ISet\<string\>* The set of all of the line ending characters listed above. |

`Spaces` *ISet\<String\>*

The super-set of all spacing modifiers, which is the union of all constants defined in the `Blanks` field and all constants defined in the `LineEndings` field.

## Methods

## CloseShave

```c#
public static string CloseShave( string text )
```

### Definition

This function is similar to `string.Trim()`, but the range of characters it removes is broader.

This function removes all characters included in the `Spaces` set from both the beginning and end of a text value.

The spacing characters "within" the text are left intact, but we guarantee that neither the beginning nor the end of the text value will be a spacing character.

## CloseShaveEnd

```c#
public static string CloseShaveEnd( string text )
```

### Definition

This function is similar to `string.TrimRight()`, but the range of characters it removes is broader.

This function removes all characters included in the `Spaces` set from the end of a text value, meaning its final character will not be a spacing character.

## CloseShaveStart

```c#
public static string CloseShaveStart( string text )
```

### Definition

This function is similar to `string.TrimLeft()`, but the range of characters it removes is broader.

This function removes all characters included in the `Spaces` set from the beginning of a text value, meaning its initial character will not be a spacing character.

## NormalizeLineEndings

```c#
public static string NormalizeLineEndings( string text )
```

### Definition

Replaces all line ending character sequences in a text value with the standard Unix line feed character. (Unicode 10 / Hex `000a`.)

## WindowsLineEndings

```c#
public static string WindowsLineEndings( string text )
```

### Definition

Replaces all line ending character sequences in a text value with the standard Microsoft Windows sequence of carriage return (Unicode 13 / Hex `000d`) + line feed (Unicode 10 / Hex `000a`).

---

# CharacterSeq Class

## Definition

```c#
public static class CharacterSeq
```

Provides static methods to create `ICharacterSeq` objects.

## Static Properties

### `Empty` *ICharacterSeq*

```c#
public static ICharacterSeq Empty
```

Returns the empty character sequence, containing no characters.

## Static Methods

## OfText

```c#
public static ICharacterSeq OfText( string text )
```

Constructs a character sequence from the provided text argument.

(Remember that any `string` values containing composite characters will be normalized.)

---
