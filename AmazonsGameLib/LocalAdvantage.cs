using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonsGameLib
{
    /// <summary>
    /// Represents the relative local advantage between player 1 and player 2 for a point on the grid
    /// based on the minimum number of queen and king moves it would take for each player to reach
    /// the point it refers to. This class is meant to be used as values in a Dictionary keyed on Point
    /// </summary>
    public class LocalAdvantage
    {
        /// <summary>
        /// The player whos turn it is
        /// </summary>
        public Owner PlayerToMove { get; set; }
        /// <summary>
        /// The minimum number of queen moves for player 1 to reach this point
        /// </summary>
        public double Player1QueenDistance { get; set; }
        /// <summary>
        /// The minimum number of queen moves for player 2 to reach this point
        /// </summary>
        public double Player2QueenDistance { get; set; }
        /// <summary>
        /// The minimum number of king moves for player 1 to reach this point
        /// </summary>
        public double Player1KingDistance { get; set; }
        /// <summary>
        /// The minimum number of king moves for player 2 to reach this point
        /// </summary>
        public double Player2KingDistance { get; set; }
        /// <summary>
        /// A constant advantage value to be used when minimum distances are equal between players
        /// </summary>
        public double K { get; set; }  = .2d;

        /// <summary>
        /// Local advantage value for queen moves. 1: player one is closer, -1: player 2 is closer, 
        /// if the distance is the same it will be K or -K depending on whose turn it is
        /// </summary>
        public double DeltaQueen
        {
            get
            {
                if (Player1QueenDistance == Player2QueenDistance) return PlayerToMove == Owner.Player1 ? K : -K;
                else if (Player1QueenDistance > Player2QueenDistance) return 1;
                else return -1;
            }
        }

        /// <summary>
        /// Local advantage value for king moves. 1: player one is closer, -1: player 2 is closer, 
        /// if the distance is the same it will be K or -K depending on whose turn it is
        /// </summary>
        public double DeltaKing
        {
            get
            {
                if (Player1KingDistance == Player2KingDistance) return PlayerToMove == Owner.Player1 ? K : -K;
                else if (Player1KingDistance > Player2KingDistance) return 1;
                else return -1;
            }
        }
    }
}
