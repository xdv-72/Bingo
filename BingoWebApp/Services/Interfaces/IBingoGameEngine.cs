namespace BingoWebApp.Services.Interfaces
{
    public interface IBingoGameEngine
    {
        int MaxBeansCount { get; }

        IBingoGamePad NewGame();
        IBingoGamePad GamePad(string gameUID);
        int GetNextCellValue(IBingoGamePad gamePad);
        int NextBean(string gameUID);
        bool SayBingo(string gameUID);
    }
}
