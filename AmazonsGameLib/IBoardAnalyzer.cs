namespace AmazonsGameLib
{
    public interface IBoardAnalyzer
    {
        /// <summary>
        /// Given a board, return an IAnalysisResult. This process should probably be thread safe 
        /// to allow for parallelization.
        /// </summary>
        /// <param name="board">The board to analyze</param>
        /// <returns>Something that implements IAnalysisResult</returns>
        IAnalysisResult Analyze(Board board);
    }
}