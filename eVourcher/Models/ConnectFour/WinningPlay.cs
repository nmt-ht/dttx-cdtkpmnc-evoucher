﻿using eVoucherGames.Models.ConnectFour.Enums;
using System.Collections.Generic;

namespace eVoucherGames.Models.ConnectFour
{
    public class WinningPlay
    {
        public List<string> WinningMoves { get; set; }
        public EvaluationDirection WinningDirection { get; set; }
        public PieceColor WinningColor { get; set; }
    }
}
