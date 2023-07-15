﻿namespace DotNetProjectFile.MsBuild;

public sealed class PackageProjectUrl : Node
{
    public PackageProjectUrl(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public string? Value => Convert<string?>(Element.Value);
}
