﻿namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefinePackageReferenceAssetsAsAttributes() : MsBuildProjectFileAnalyzer(Rule.DefinePackageReferenceAssetsAsAttributes)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var reference in context.Project.ItemGroups
            .SelectMany(g => g.PackageReferences)
            .Where(HasAssetsElement))
        {
            context.ReportDiagnostic(Description, reference, reference.Include);
        }
    }

    private static bool HasAssetsElement(PackageReference reference)
        => reference.Element.Elements()
        .Any(e => e.Name.LocalName.Contains("assets", StringComparison.OrdinalIgnoreCase));
}
