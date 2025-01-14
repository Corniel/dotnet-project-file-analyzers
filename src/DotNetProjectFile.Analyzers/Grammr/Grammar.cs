using Grammr.Lexers;
using Grammr.Parsers;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Grammr;

/// <summary>Represents a base to build grammar with.</summary>
[Inheritable]
public class Grammar
{
    protected Grammar() { }

    /// <summary>End of file.</summary>
    public static readonly Token eof = new EndOfFile();

    /// <summary>End of Line.</summary>
    public static Token eol([CallerMemberName] string? kind = null) => new EndOfLine(kind);

    /// <summary>Matches if the remaining source starts with the specified character.</summary>
    /// <param name="ch">
    /// The expected character.
    /// </param>
    /// <param name="kind">
    /// The (optional) token kind.
    /// </param>
    /// <returns>
    /// A new grammar.
    /// </returns>
    public static Token ch(char ch, [CallerMemberName] string? kind = null) => new Lexers.Char(ch, kind);

    /// <summary>Matches if the remaining source matches the predicate.</summary>
    /// <param name="predicate">
    /// The predicate to match.
    /// </param>
    /// <param name="kind">
    /// The (optional) token kind.
    /// </param>
    /// <returns>
    /// A new grammar.
    /// </returns>
    [Pure]
    public static Token match(Predicate<char> predicate, [CallerMemberName] string? kind = default) => new Predicate(predicate, kind);

    /// <summary>Matches if the remaining source starts with the specified string.</summary>
    /// <param name="str">
    /// The expected string.
    /// </param>
    /// <param name="kind">
    /// The (optional) token kind.
    /// </param>
    /// <returns>
    /// A new grammar.
    /// </returns>
    public static Token str(string str, [CallerMemberName] string? kind = null) => new Lexers.String(str, kind);

    /// <inheritdoc cref="regex(Regex, string?)"/>
    [Pure]
    public static Token regex([StringSyntax(StringSyntaxAttribute.Regex)] string pattern, [CallerMemberName] string? kind = default)
        => new RegularExpression(pattern, kind);

    /// <summary>Matches if the remaining source starts with the specified pattern.</summary>
    /// <param name="pattern">
    /// The expected pattern.
    /// </param>
    /// <param name="kind">
    /// The (optional) token kind.
    /// </param>
    /// <returns>
    /// A new grammar.
    /// </returns>
    [Pure]
    public static Token regex(Regex pattern, [CallerMemberName] string? kind = default) => new RegularExpression(pattern, kind);

    /// <summary>Creates a typed node.</summary>
    /// <typeparam name="TNode">
    /// The type of the node.
    /// </typeparam>
    /// <param name="parser">
    /// The parser responsible for the content of the node.
    /// </param>
    [Pure]
    public static Typed node<TNode>(Parser parser) where TNode : Syntax.Node => new(parser, typeof(TNode));
}
