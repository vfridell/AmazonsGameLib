using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonsGameLib
{
    public class BoardHistories
    {
        public Board Board { get; set; }

        public BoardHistories(Board board)
        {
            Board = board;
        }

        public IEnumerable<Board> GetPreviousBoards()
        {
            throw new NotImplementedException();
        }

        
    }
}
