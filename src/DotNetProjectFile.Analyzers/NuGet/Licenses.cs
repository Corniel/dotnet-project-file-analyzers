using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Text;

namespace DotNetProjectFile.NuGet;

// Based on https://spdx.org/licenses/

public static class Licenses
{
    public static readonly LicenseExpression Unknown = UnknownLicense.Instance;

    // Note that this list is not and will never be legal advice.
    public static readonly ImmutableArray<LicenseExpression> All =
    [
        // TODO: the remainder of the recognized list.

        Unknown,
        new PermissiveLicense("MIT"),
        new PermissiveLicense("Apache-1.0"),
        new PermissiveLicense("Apache-1.1"),
        new PermissiveLicense("Apache-2.0"),
        new PermissiveLicense("0BSD"),
        new PermissiveLicense("BSD-1-Clause"),
        new PermissiveLicense("BSD-2-Clause"),
        new PermissiveLicense("BSD-3-Clause"),
        new PermissiveLicense("BSD-4-Clause"),
        new PermissiveLicense("Unlicense"),
        new PermissiveLicense("MS-PL"),

        // MPL is technically copy-left, but only on a source-file level, so can be statically and dynamically linked.
        new PermissiveLicense("MPL-1.0"),
        new PermissiveLicense("MPL-1.1"),
        new PermissiveLicense("MPL-2.0"),
        new PermissiveLicense("MPL-2.0-no-copyleft-exception"),

        new CopyLeftLicense("GPL-1.0-only", ["GPL-1.0"]),
        new CopyLeftLicense("GPL-1.0", ["GPL-1.0-only"], Deprecated: true),
        new CopyLeftLicense("GPL-2.0-only", ["GPL-2.0"]),
        new CopyLeftLicense("GPL-2.0", ["GPL-2.0-only"], Deprecated: true),
        new CopyLeftLicense("GPL-3.0-only", ["GPL-3.0"]),
        new CopyLeftLicense("GPL-3.0", ["GPL-3.0-only"], Deprecated: true),

        new CopyLeftLicense("GPL-1.0-or-later", ["GPL-1.0", "GPL-1.0-only", "GPL-1.0+", "GPL-2.0", "GPL-2.0-only", "GPL-2.0+", "GPL-2.0-or-later", "GPL-3.0", "GPL-3.0-only", "GPL-3.0+", "GPL-3.0-or-later"]),
        new CopyLeftLicense("GPL-1.0+", ["GPL-1.0", "GPL-1.0-only", "GPL-1.0-or-later", "GPL-2.0", "GPL-2.0-only", "GPL-2.0+", "GPL-2.0-or-later", "GPL-3.0", "GPL-3.0-only", "GPL-3.0+", "GPL-3.0-or-later"], Deprecated: true),
        new CopyLeftLicense("GPL-2.0-or-later", ["GPL-2.0", "GPL-2.0-only", "GPL-2.0+", "GPL-3.0", "GPL-3.0-only", "GPL-3.0+", "GPL-3.0-or-later"]),
        new CopyLeftLicense("GPL-2.0+", ["GPL-2.0", "GPL-2.0-only", "GPL-2.0-or-later", "GPL-3.0", "GPL-3.0-only", "GPL-3.0+", "GPL-3.0-or-later"], Deprecated: true),
        new CopyLeftLicense("GPL-3.0-or-later", ["GPL-3.0", "GPL-3.0-only", "GPL-3.0+"]),
        new CopyLeftLicense("GPL-3.0+", ["GPL-3.0", "GPL-3.0-only", "GPL-3.0-or-later"], Deprecated: true),

        new CopyLeftLicense("AGPL-1.0-or-later", ["AGPL-1.0", "AGPL-1.0-only", "AGPL-1.0+", "AGPL-3.0", "AGPL-3.0-only", "AGPL-3.0+", "AGPL-3.0-or-later"]),
        new CopyLeftLicense("AGPL-1.0+", ["AGPL-1.0", "AGPL-1.0-only", "AGPL-1.0-or-later", "AGPL-3.0", "AGPL-3.0-only", "AGPL-3.0+", "AGPL-3.0-or-later"], Deprecated: true),
        new CopyLeftLicense("AGPL-3.0-or-later", ["AGPL-3.0", "AGPL-3.0-only", "AGPL-3.0+"]),
        new CopyLeftLicense("AGPL-3.0+", ["AGPL-3.0", "AGPL-3.0-only", "AGPL-3.0-or-later"], Deprecated: true),
    ];

    private static readonly FrozenDictionary<string, LicenseExpression> Lookup
        = All.ToFrozenDictionary(x => x.Expression, x => x);

    public static LicenseExpression Parse(string input)
    {
        // TODO: handle complex expressions: https://spdx.github.io/spdx-spec/v2-draft/using-SPDX-short-identifiers-in-source-files/#e4-representing-multiple-licenses
        
        if (Lookup.TryGetValue(input, out var result))
        {
            return result;
        }
        else
        {
            return Unknown;
        }
    }
}

public abstract record LicenseExpression(bool Deprecated)
{
    public abstract string Expression { get; }

    public abstract bool CompatibleWith(LicenseExpression other);
}

public static class LicenseExpressionExtensions
{
    public static bool CompatibleWith(this LicenseExpression license, string other)
        => license.CompatibleWith(Licenses.Parse(other));
}

public sealed record UnknownLicense : LicenseExpression
{
    public static readonly UnknownLicense Instance = new();

    private UnknownLicense()
        : base(false)
    {
    }

    public override string Expression => string.Empty;

    public override bool CompatibleWith(LicenseExpression other)
        => true;
}

public abstract record SingleLicense(string Identifier, bool Deprecated) : LicenseExpression(Deprecated)
{
    public override string Expression => Identifier;
}

public sealed record PermissiveLicense(string Identifier, bool Deprecated = false) : SingleLicense(Identifier, Deprecated)
{
    public override bool CompatibleWith(LicenseExpression other)
        => true;
}

public sealed record CopyLeftLicense(string Identifier, ImmutableArray<string> Compatibilities, bool Deprecated = false) : SingleLicense(Identifier, Deprecated)
{
    public override bool CompatibleWith(LicenseExpression other)
    {
        if (other.Expression == Expression || Compatibilities.Contains(other.Expression))
        {
            return true;
        }

        return other switch
        {
            // NB: the inversion of the `and` and `or` are intentional.
            AndLicenseExpression e => CompatibleWith(e.Left) || CompatibleWith(e.Right),
            OrLicenseExpression e => CompatibleWith(e.Left) && CompatibleWith(e.Right),
            UnknownLicense => true, // Assume true if information is missing.
            _ => false, // All other cases.
        };
    }
}

public sealed record AndLicenseExpression(LicenseExpression Left, LicenseExpression Right) : LicenseExpression(Left.Deprecated || Right.Deprecated)
{
    public override string Expression => $"({Left} AND {Right})";

    public override bool CompatibleWith(LicenseExpression other)
        => Left.CompatibleWith(other) && Right.CompatibleWith(other);
}

public sealed record OrLicenseExpression(LicenseExpression Left, LicenseExpression Right) : LicenseExpression(Left.Deprecated || Right.Deprecated)
{
    public override string Expression => $"({Left} OR {Right})";

    public override bool CompatibleWith(LicenseExpression other)
        => Left.CompatibleWith(other) || Right.CompatibleWith(other);
}
