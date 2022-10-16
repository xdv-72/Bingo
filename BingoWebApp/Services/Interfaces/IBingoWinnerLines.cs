namespace BingoWebApp.Services.Interfaces
{
    public interface IBingoWinnerLines
    {
        IEnumerable<IList<IBingoCellBase>> ActiveLines { get; }
        bool CheckWinnerLines(IBingoGamePad gamePad);
    }
}
