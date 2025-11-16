rebuild:
	dotnet clean && dotnet build && dotnet run --framework net9.0-maccatalyst

run:
	dotnet run --framework net9.0-maccatalyst