namespace Rules.MS_Build.NuGet_packages.Define_package_info;

public class Reports_on_missing
{
    [Test]
    public void information_provided()
       => new DefinePackageInfo()
       .ForProject("NoPackageInfo.cs")
       .HasIssues(
           Issue.WRN("Proj0201", "Define the <Version> or <VersionPrefix> node explicitly or define the <IsPackable> node with value 'false'."),
           Issue.WRN("Proj0202", "Define the <Description> or <PackageDescription> node explicitly or define the <IsPackable> node with value 'false'."),
           Issue.WRN("Proj0203", "Define the <Authors> node explicitly or define the <IsPackable> node with value 'false'."),
           Issue.WRN("Proj0204", "Define the <PackageTags> node explicitly or define the <IsPackable> node with value 'false'."),
           Issue.WRN("Proj0205", "Define the <RepositoryUrl> node explicitly or define the <IsPackable> node with value 'false'."),
           Issue.WRN("Proj0206", "Define the <PackageProjectUrl> node explicitly or define the <IsPackable> node with value 'false'."),
           Issue.WRN("Proj0207", "Define the <Copyright> node explicitly or define the <IsPackable> node with value 'false'."),
           Issue.WRN("Proj0208", "Define the <PackageReleaseNotes> node explicitly or define the <IsPackable> node with value 'false'."),
           Issue.WRN("Proj0209", "Define the <PackageReadmeFile> node explicitly or define the <IsPackable> node with value 'false'."),
           Issue.WRN("Proj0210", "Define the <PackageLicenseExpression> or <PackageLicenseFile> node explicitly or define the <IsPackable> node with value 'false'."),
           Issue.WRN("Proj0212", "Define the <PackageIcon> node explicitly or define the <IsPackable> node with value 'false'."),
           Issue.WRN("Proj0213", "Define the <PackageIconUrl> node explicitly or define the <IsPackable> node with value 'false'."),
           Issue.WRN("Proj0214", "Define the <PackageId> node explicitly or define the <IsPackable> node with value 'false'."),
           Issue.WRN("Proj0216", "Define the <ProductName> node explicitly or define the <IsPackable> node with value 'false'."),
           Issue.WRN("Proj0217", "Define the <PackageRequireLicenseAcceptance> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void version()
       => new DefinePackageInfo()
       .ForProject("NoVersion.cs")
       .HasIssue(
           Issue.WRN("Proj0201", "Define the <Version> or <VersionPrefix> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void description()
       => new DefinePackageInfo()
       .ForProject("NoDescription.cs")
       .HasIssue(
           Issue.WRN("Proj0202", "Define the <Description> or <PackageDescription> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void authors()
       => new DefinePackageInfo()
       .ForProject("NoAuthors.cs")
       .HasIssue(
           Issue.WRN("Proj0203", "Define the <Authors> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void tags()
       => new DefinePackageInfo()
       .ForProject("NoTags.cs")
       .HasIssue(
           Issue.WRN("Proj0204", "Define the <PackageTags> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void repository_url()
       => new DefinePackageInfo()
       .ForProject("NoRepositoryUrl.cs")
       .HasIssue(
           Issue.WRN("Proj0205", "Define the <RepositoryUrl> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void url()
       => new DefinePackageInfo()
       .ForProject("NoUrl.cs")
       .HasIssue(
           Issue.WRN("Proj0206", "Define the <PackageProjectUrl> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void copyright()
       => new DefinePackageInfo()
       .ForProject("NoCopyright.cs")
       .HasIssue(
           Issue.WRN("Proj0207", "Define the <Copyright> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void release_notes()
       => new DefinePackageInfo()
       .ForProject("NoReleaseNotes.cs")
       .HasIssue(
           Issue.WRN("Proj0208", "Define the <PackageReleaseNotes> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void readme()
       => new DefinePackageInfo()
       .ForProject("NoReadme.cs")
       .HasIssue(
           Issue.WRN("Proj0209", "Define the <PackageReadmeFile> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void license()
       => new DefinePackageInfo()
       .ForProject("NoLicense.cs")
       .HasIssue(
           Issue.WRN("Proj0210", "Define the <PackageLicenseExpression> or <PackageLicenseFile> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void icon()
       => new DefinePackageInfo()
       .ForProject("NoIcon.cs")
       .HasIssue(
           Issue.WRN("Proj0212", "Define the <PackageIcon> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void icon_url()
       => new DefinePackageInfo()
       .ForProject("NoIconUrl.cs")
       .HasIssue(
           Issue.WRN("Proj0213", "Define the <PackageIconUrl> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void product_name()
      => new DefinePackageInfo()
      .ForProject("NoProductName.cs")
      .HasIssue(
          Issue.WRN("Proj0216", "Define the <ProductName> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void missing_require_package_license_acceptance()
      => new DefinePackageInfo()
        .ForProject(@"NoPackageRequireLicenseAcceptance.cs")
        .HasIssue(Issue.WRN("Proj0217", "Define the <PackageRequireLicenseAcceptance> node explicitly or define the <IsPackable> node with value 'false'."));
}

public class Guards
{
    [Test]
    public void Test_project()
        => new DefinePackageInfo()
       .ForProject("TestProject.cs")
       .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    [TestCase("PackageDescription.cs")]
    [TestCase("WithLicenseFile.cs")]
    [TestCase("VersionPrefix.cs")]
    [TestCase("VersionPrefixAndSuffix.cs")]
    [TestCase("VersionPrefixAndSuffixAndVersion.cs")]
    [TestCase("PackageRequireLicenseAcceptanceDisabled.cs")]
    public void Projects_without_issues(string project)
         => new DefinePackageInfo()
        .ForProject(project)
        .HasNoIssues();
}
