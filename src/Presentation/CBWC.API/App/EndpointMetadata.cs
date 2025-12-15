namespace CBWC.API.App;

internal record RetiredAtMetadata(
    DateOnly RetiredAt,
    DateOnly? SunsetAt,
    string? LinkDescription);

internal record ChangedAtMetadata(string Name, string ChangedAt);
