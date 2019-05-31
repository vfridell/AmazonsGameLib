namespace AmazonsGameLib
{
    public interface IAnalysisResult
    {
        double player1Advantage { get; set; }
        GameResult gameResult { get; }
    }
}