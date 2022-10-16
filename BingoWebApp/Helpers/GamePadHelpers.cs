using BingoWebApp.Services.Interfaces;

namespace BingoWebApp.Helpers
{
    public static class GamePadHelpers
    {
        public static string GetGamePadItemStyleExtension(int row, int col)
        {

            return $"width: 100%; height: 60px;" +
                $"font-weight: bold; " +                
                $"border-width: 1px; position: relative; cursor.pointer;";
        }

        public static string GetCellStyleBackgroundColor(int row, int col, IBingoGamePad gamePad)
        {
            if (gamePad != null && gamePad.gamePadItems.Count(_ => _.Row == row && _.Col == col && _.IsActive) > 0)
            {
                return "background-color: #fdc278b3;";
            }
            else
            {
                return "background-color: white;";
            }
        }

        public static string GetCellStyleWinnerBackgroundColor()
        {
            return "background-color: red; color: white;";
        }

        public static bool IsWinnerLine(int row, int col, IBingoGamePad gamePad)
        {
            bool result = gamePad.gamePadItems
                .Any(x => x.Row == row && x.Col == col &&
                    ((IBingoWinnerLines)gamePad).ActiveLines
                    .Any(l => l
                        .Any(i => i.Row == x.Row && i.Col == x.Col)) == true);
            return result;
        }
    }
}
