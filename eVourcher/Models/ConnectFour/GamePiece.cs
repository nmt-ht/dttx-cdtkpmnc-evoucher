using eVoucherGames.Models.ConnectFour.Enums;

namespace eVoucherGames.Models.ConnectFour
{
    public class GamePiece
    {
        public PieceColor Color;

        public GamePiece()
        {
            Color = PieceColor.Blank;
        }

        public GamePiece(PieceColor color)
        {
            Color = color;
        }
    }
}
