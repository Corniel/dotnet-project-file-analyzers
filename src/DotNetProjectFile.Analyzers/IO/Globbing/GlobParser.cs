using DotNetProjectFile.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.IO.Globbing;

public static class GlobParser
{
    public static Segement TryParse(string str)
    {
        var span = new SourceSpan(SourceText.From(str));

        return TryParse(span);
    }

    private static Segement TryParse(SourceSpan span)
    {
        var group = new List<Segement>();

        while (!span.IsEmpty)
        {
            if (span.StartsWith('?') is { } any)
            {
                group.Add(Segement.AnyChar);
                span = span.TrimLeft(any.Length);
            }
            else if (span.Matches(c => c == '*') is { } wildcards)
            {
                group.Add(wildcards.Length == 1 ? Segement.Wildcard : Segement.RecursiveWildcard);
                span = span.TrimLeft(wildcards.Length);
            }
            else if (span.Option(out var option) is { } sp)
            {
                group.Add(option!);
                span = span.TrimLeft(sp.Length);
            }
            else if (span.Sequence() is { } sequence)
            {
                var text = span.SourceText.ToString(sequence);
                group.Add(text[0] == '!' ? new NotSequence(text[1..]) : new Sequence(text));
                span = span.TrimLeft(sequence.Length + 2);
            }
            else if (span.Literal() is { } literal)
            {
                group.Add(new Literal(span.SourceText.ToString(literal)));
                span = span.TrimLeft(literal.Length);
            }
            else
            {
                group.Add(new Unparsable(span.Text));
                span = span.TrimLeft(span.Length);
            }
        }
        return group.Count == 1
            ? group[0]
            : Segement.Group(group);
    }

    private static TextSpan? Sequence(this SourceSpan span)
    {
        if (span.StartsWith('[') is { })
        {
            span++;

            if (span.Matches(c => c != ']') is { } match &&
                span.TrimLeft(match.Length).StartsWith(']') is { })
            {
                return match;
            }
            else
            {
                throw new InvalidPattern("] Missing.");
            }
        }
        else
        {
            return null;
        }
    }

    private static TextSpan? Option(this SourceSpan span, out Option? option)
    {
        var sp = span;
        option = null;

        if (sp.StartsWith('{') is { })
        {
            sp++;

            var options = new List<Segement>();

            while (sp.Length != 0)
            {
                if (sp.StartsWith(',') is { })
                {
                    // unexpected comma.
                    if (options.Count == 0) { return null; }
                    else { sp++; }
                }

                // closing.

                else if (sp.StartsWith('}') is { })
                {
                    if (options.Count == 0)
                    {
                        return null;
                    }
                    option = new Option(options);
                    return new(span.Start, sp.Start - span.Start + 1);
                }
                // comma is missing.
                else if (options.Count != 0)
                {
                    return null;
                }

                if (sp.StartsWith('{') is { } && sp.Option(out var nested) is { } nest)
                {
                    options.Add(nested!);
                    sp = sp.TrimLeft(nest.Length);
                }
                else if (sp.Matches(c => c != ',' && c != '}') is { } next)
                {
                    var sub = sp.Trim(next);
                    options.Add(TryParse(sub));
                    sp = sp.TrimLeft(next.Length);
                }
                else
                {
                    return null;
                }
            }
        }
        return null;
    }

    private static TextSpan? Literal(this SourceSpan span)
        => span.Matches(c => c != '?' && c != '*' && c != '[' && c != '{' && c != ',');
}
