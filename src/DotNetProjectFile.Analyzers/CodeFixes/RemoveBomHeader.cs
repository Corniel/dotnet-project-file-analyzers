using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Text;
using System.Threading.Tasks;

namespace DotNetProjectFile.CodeFixes;

public sealed class RemoveBomHeader : CodeFixProvider
{
    private static readonly Encoding UTF8_no_BOM = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

    public sealed override ImmutableArray<string> FixableDiagnosticIds => [Rule.OnlyUseUTF8WithoutBom.Id];

    public override Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        if (context.Diagnostics.FirstOrDefault() is { } diagnostic)
        {

            context.RegisterCodeFix(
                CodeAction.Create("Remove UTF-8 BOM header", t => UpdateDocument(context.Document, t)),
                diagnostic);
        }

        return Task.CompletedTask;
    }

    private static async Task<Document> UpdateDocument(Document document, CancellationToken token)
    {
        var text = await document.GetTextAsync(token)!;

        return document.WithText(text);
    }

    public sealed override FixAllProvider? GetFixAllProvider() => null;
}
