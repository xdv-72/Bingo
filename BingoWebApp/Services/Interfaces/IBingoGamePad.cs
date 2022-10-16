namespace BingoWebApp.Services.Interfaces
{
    public interface IBingoGamePad
    {
        IBingoGameEngine Engine { get; }
        string GamePadUID { get; }
        int GamePadSize { get; }
        bool IsGameOver { get; }
        bool IsGameWon { get; }

        List<IBingoCell> gamePadItems { get; }

        List<int> BeansSequence { get; }
        
        void NewGamePad();
        IBingoGamePad GamePad(string gameUID);

        void ClearInactiveLinesOnWin();
        void ClearActiveLines();
    }
}
