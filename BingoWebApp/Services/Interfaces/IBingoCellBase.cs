namespace BingoWebApp.Services.Interfaces
{
    public interface IBingoCellBase
    {
        int Value { get; }
        int Row { get; }
        int Col { get; }
    }
}