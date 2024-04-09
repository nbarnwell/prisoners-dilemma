namespace PrisonersDilemma.Runner;

public record GameResult
{
    public Decision MyDecision { get; }
    public Decision TheirDecision { get; }

    public GameResult(Decision myDecision, Decision theirDecision)
    {
        MyDecision = myDecision;
        TheirDecision = theirDecision;
    }
}