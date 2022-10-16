using BingoWebApp.Services.Interfaces;

namespace BingoWebApp.Services
{
    public class BingoCell : BingoCellBase, IBingoCellBase, IBingoCell
    {
        private bool _isActive;
        public bool IsActive { get => _isActive; set => _isActive = value; }
        public BingoCell(int row, int col, int value) : base(row, col, value) { }        
    }
}
