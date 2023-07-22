﻿namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class OrderPackageReferencesAlphabetically : MsBuildProjectFileAnalyzer
{
    public OrderPackageReferencesAlphabetically() : base(Rule.OrderPackageReferencesAlphabetically) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var references in context.Project
            .ImportsAndSelf()
            .SelectMany(p => p.ItemGroups)
            .Select(g => g.PackageReferences))
        {
            AnalyzeGroup(context, references);
        }
    }

    private void AnalyzeGroup(ProjectFileAnalysisContext context, Nodes<PackageReference> references)
        => references.CheckAlphabeticalOrder(r => r.IncludeOrUpdate, (expected, found) =>
        {
            context.ReportDiagnostic(Descriptor, expected, expected.IncludeOrUpdate, found.IncludeOrUpdate);
        });
}
