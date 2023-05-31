using eVoucherGames.Models.TicTacToe.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVoucherGames.Models.TicTacToe
{
    public class GamePiece
    {
        public PieceStyle Style;

        public GamePiece()
        {
            Style = PieceStyle.Blank;
        }
    }
}
