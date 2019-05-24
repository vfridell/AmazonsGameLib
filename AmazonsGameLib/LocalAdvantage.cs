using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonsGameLib
{
    public class LocalAdvantage
    {
        public Owner PlayerToMove { get; set; }
        public double Player1QueenDistance { get; set; }
        public double Player2QueenDistance { get; set; }
        public double Player1KingDistance { get; set; }
        public double Player2KingDistance { get; set; }
        public double K { get; set; }  = .2d;

        public double DeltaQueen
        {
            get
            {
                if (Player1QueenDistance == Player2QueenDistance) return PlayerToMove == Owner.Player1 ? K : -K;
                else if (Player1QueenDistance > Player2QueenDistance) return 1;
                else return -1;
            }
        }

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
