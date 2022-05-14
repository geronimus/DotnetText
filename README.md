# Geronimus.Text Namespace

⚠️ _Under Construction_ ⚠️

Contains utility functions for working with text values.

# Characters Class

## Definition

Contains constants and methods useful for processes involving manipulation of characters within text values.

```c#
public static class Characters
```

## Properties

| | |
| --- | --- |
| `public const char CarriageReturn` | The Unicode code point 13 / Hex `000d`.  |
| `public const char LineFeed` | The Unicode code point 10 / Hex `000a`. |
| `public const char LineSeparator` | The Unicode code point 8232 / Hex `2028`. |
| `public const char ParagraphSeparator` | The Unicode code point 8233 / Hex `2029`. |
| `public static readonly ISet<char> LineEndings` | A set containing the four line ending characters referenced above. |

## Methods

### NormalizeLineEndings

#### Definition

Replaces all line ending character sequences in a text value with the standard Unix line feed character. (Unicode 10 / Hex `000a`.)

```c#
public static string NormalizeLineEndings( string text )
```

### WindowsLineEndings

#### Definition

Replaces all line ending character sequences in a text value with the standard Microsoft Windows sequence of carriage return (Unicode 13 / Hex `000d`) + line feed (Unicode 10 / Hex `000a`).

```c#
public static string WindowsLineEndings( string text )
```
