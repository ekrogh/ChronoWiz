namespace ChronoWiz.Shared.Ics;

public sealed class IcsCoordinator
{
	public static IcsCoordinator Default { get; } = new();

	public OpenIcsRequest? LastOpenRequest { get; private set; }
	public SaveIcsRequest? LastSaveRequest { get; private set; }

	public void RequestOpen(OpenIcsRequest req) => LastOpenRequest = req;

	public void RequestSave(SaveIcsRequest req) => LastSaveRequest = req;
}
