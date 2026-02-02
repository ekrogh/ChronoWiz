namespace ChronoWiz.Shared.Ics;

public sealed class SaveIcsRequest
{
	public string Summary { get; init; } = "Summary";
	public string Description { get; init; } = "Description";
	public string Location { get; init; } = "Location";
}
