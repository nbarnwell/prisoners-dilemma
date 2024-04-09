namespace PrisonersDilemma.Runner;

internal class Score
{
    public readonly int PlayerScore;
    public readonly int OpponentScore;

    public Score(int playerScore, int opponentScore)
    {
        PlayerScore   = playerScore;
        OpponentScore = opponentScore;
    }

    public static Score From(Decision playerDecision, Decision opponentDecision)
    {
        switch (playerDecision)
        {
            case Decision.Cooperate when opponentDecision == Decision.Cooperate:
                return new Score(3, 3);
            case Decision.Cooperate when opponentDecision == Decision.Defect:
                return new Score(5, 0);
            case Decision.Defect when opponentDecision == Decision.Cooperate:
                return new Score(0, 5);
            case Decision.Defect when opponentDecision == Decision.Defect:
                return new Score(1, 1);
            default:
                throw new ArgumentOutOfRangeException(nameof(playerDecision), playerDecision, null);
        }
    }
}