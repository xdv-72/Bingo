namespace BingoWebApp.Services.Interfaces
{
    public interface IBingoCell: IBingoCellBase
    {
        bool IsActive { get; set; }
    }
}
