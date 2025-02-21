
namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UpdateLacksInclude() : MsBuildProjectFileAnalyzer(Rule.AvoidLicenseUrl)
{
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var existing = new HashSet<string>();

        foreach (var action in context.File.Walk().OfType<BuildAction>())
        {
            foreach (var include in action.Include) { existing.Add(include); }

            // We are in the project file to analyze
            if (action.Project == context.File)
            {
                foreach (var update in action.Update.Where(x => !existing.Contains(x)))
                {
                    context.ReportDiagnostic(Descriptor, action, update);
                }
                foreach (var exclude in action.Exclude.Where(x => !existing.Contains(x)))
                {
                    context.ReportDiagnostic(Descriptor, action, exclude);
                }
                foreach (var remove in action.Remove.Where(x => !existing.Contains(x)))
                {
                    context.ReportDiagnostic(Descriptor, action, remove);
                }
            }
        }
    }
}
