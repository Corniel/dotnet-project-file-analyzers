﻿namespace DotNetProjectFile.MsBuild;

public sealed class PackageLicenseFile(XElement element, Node parent, Project project)
    : Node<string>(element, parent, project) { }
