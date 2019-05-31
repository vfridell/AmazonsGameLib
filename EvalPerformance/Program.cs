using AmazonsGameLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvalPerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Begin(null, null, 10);

            foreach (Move move in game.CurrentMoves)
            {
                var analysisGraph = new AnalysisGraph();
                analysisGraph.BuildAnalysis(game.CurrentBoard.PieceGrid, Owner.Player1);
            }
        }
    }
}
