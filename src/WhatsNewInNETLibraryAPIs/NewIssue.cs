using System.Diagnostics.CodeAnalysis;

namespace WhatsNewInNETLibraryAPIs;

public sealed class NewIssue
{
	[SetsRequiredMembers]
	public NewIssue(IssueLevel level, TimeProvider timeProvider)
	{
		ArgumentNullException.ThrowIfNull(timeProvider);
		(this.Level, this.Created, this.TimeProvider) = 
			(level, timeProvider.GetUtcNow(), timeProvider);
	}

	public PriorityLevel GetPriority() =>
		this.Level switch
		{
			IssueLevel.Feature => PriorityLevel.None,
			IssueLevel.Bug => this.TimeProvider.GetUtcNow().Subtract(this.Created).TotalHours < 24 ?
				PriorityLevel.Concering : PriorityLevel.Immediate,
			_ => PriorityLevel.Immediate,
		};

	private TimeProvider TimeProvider { get; init; }
	public required DateTimeOffset Created { get; init; }
	public required IssueLevel Level { get; init; }
}