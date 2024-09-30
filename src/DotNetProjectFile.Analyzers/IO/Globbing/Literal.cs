namespace DotNetProjectFile.IO.Globbing;

public class Literal(string text) : Segement
{
    public string Text { get; } = text;

    public override int MinLength => Text.Length;

    public override int MaxLength => Text.Length;

    public override string ToString() => Text;
}
