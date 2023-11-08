using System.Diagnostics.CodeAnalysis;

namespace WhatsNewInNETLibraryAPIs;

public sealed class OldIssue
{
	[SetsRequiredMembers]
	public OldIssue(IssueLevel level) =>
		(this.Level, this.Created) = (level, DateTime.UtcNow);

	public PriorityLevel GetPriority() => 
		this.Level switch
		{
			IssueLevel.Feature => PriorityLevel.None,
			IssueLevel.Bug => DateTime.UtcNow.Subtract(this.Created).TotalHours < 24 ?
				PriorityLevel.Concering : PriorityLevel.Immediate,
			_ => PriorityLevel.Immediate,
		};

	public required DateTime Created { get; init; }
	public required IssueLevel Level { get; init; }
}