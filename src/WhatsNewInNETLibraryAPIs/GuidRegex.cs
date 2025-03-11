using System.Text.RegularExpressions;

namespace WhatsNewInNETLibraryAPIs;

public static partial class GuidRegex
{
	// Lifted from: https://www.geeksforgeeks.org/how-to-validate-guid-globally-unique-identifier-using-regular-expression/
	// Note that in C# 13, you can use a partial property, instead of a method.
	[GeneratedRegex("^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", RegexOptions.IgnoreCase)]
	internal static partial Regex Regex { get; }
}