﻿namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineIsPublishable : MsBuildProjectFileAnalyzer
{
    public DefineIsPublishable() : base(Rule.DefineIsPublishable) { }

    protected override bool ApplyToProps => false;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Project
            .ImportsAndSelf()
            .SelectMany(p => p.PropertyGroups)
            .SelectMany(g => g.IsPublishable).None())
        {
            context.ReportDiagnostic(Descriptor, context.Project);
        }
    }
}
