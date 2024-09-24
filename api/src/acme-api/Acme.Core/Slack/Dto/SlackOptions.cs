namespace Acme.Core.Slack.Dto;

public record SlackOptions
{
    public required bool Enabled { get; init; }
}
