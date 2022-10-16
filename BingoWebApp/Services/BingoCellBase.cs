using BingoWebApp.Services.Interfaces;

namespace BingoWebApp.Services
{
    public class BingoCellBase: IBingoCellBase
    {
        private int _value;
        public int Value { get => _value; protected set => _value = value; }

        private int _row;
        public int Row { get => _row; protected set => _row = value; }

        private int _col;
        public int Col { get => _col; protected set => _col = value; }

        public BingoCellBase(int row, int col, int value)
        {
            Row = row;
            Col = col;
            Value = value;
        }
    }
}
