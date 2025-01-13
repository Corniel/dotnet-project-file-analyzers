using Grammr;
using Grammr.Text;
using Token = Grammr.Token;

namespace Grammr_Parsers_specs;

public class Parses : Grammar
{
    private static readonly Token A = ch('A');
    private static readonly Token B = ch('B');
    private static readonly Token C = ch('C');

    public static readonly Parser ABC = A & B & C;
    public static readonly Parser Letter = A | B | C;
    public static readonly Parser As = A.Star;
    public static readonly Parser Bs = B.Repeat(2);

    [Test]
    public void sequences()
    {
        var results = ABC.Parse(TokenStream.From("ABCDE"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new []{ new { Text = "A", Kind = "A" }, new { Text = "B", Kind = "B" }, new { Text = "C", Kind = "C" } },
                Success = true,
                Remaining = new { Text = "DE" },
            }]);
    }

    [Test]
    public void switches()
    {
        var results = Letter.Parse(TokenStream.From("ABCDE"), new());

        results.Should().BeEquivalentTo([
        new
        {
            Stream = new []{ new { Text = "A", Kind = "A" } },
            Success = true,
            Remaining = new { Text = "BCDE" },
        }]);
    }

    public class Repeates
    {
        [Test]
        public void multiple()
        {
            var results = As.Parse(TokenStream.From("AAABCDE"), new());

            object[] expected = [
                new
                {
                    Stream = new[]{ new { Text = "A", Kind = "A" }, new { Text = "A", Kind = "A" }, new { Text = "A", Kind = "A" } },
                    Success = true,
                    Remaining = new { Text = "BCDE" },
                },
                new
                {
                    Stream = new[]{ new { Text = "A", Kind = "A" }, new { Text = "A", Kind = "A" } },
                    Success = true,
                    Remaining = new { Text = "ABCDE" },
                },
                new
                {
                    Stream = new[]{ new { Text = "A", Kind = "A" } },
                    Success = true,
                    Remaining = new { Text = "AABCDE" },
                },
                new
                {
                    Stream = Array.Empty<object>(),
                    Success = true,
                    Remaining = new { Text = "AAABCDE" },
                }];

            results.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void not_enough()
        {
            var results = Bs.Parse(TokenStream.From("BCDE"), new());

            results.Outcome.Should().BeEquivalentTo(
            new
            {
                Stream = new[] { new { Text = "B", Kind = "B" } },
                Success = false,
                Remaining = new { Text = "CDE" },
                Message = "Expected B."
            });
        }
    }
}
