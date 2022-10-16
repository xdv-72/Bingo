using BingoWebApp.Services.Interfaces;
using System.Linq;

namespace BingoWebApp.Services
{
    public class BingoGamePad : IBingoGamePad, IBingoWinnerLines
    {
        private string _gamePadId = string.Empty;
        public string GamePadUID { get => _gamePadId; private set => _gamePadId = value; }

        private int _gamePadSize;
        public int GamePadSize
        { 
            get => _gamePadSize; 
            private set => _gamePadSize = value;
        }

        public bool IsGameOver 
        {
            get
            {
                return ActiveLines.Count() == 0;
            }
        }

        private int maxActiveLines;
        public bool IsGameWon
        {
            get
            {
                return (ActiveLines.Count() < maxActiveLines);
            }
        }

        private IBingoGameEngine _engine;
        public IBingoGameEngine Engine => _engine;

        private List<IBingoCell> _cellPad = new List<IBingoCell>();
        public List<IBingoCell> gamePadItems { get => _cellPad; }

        private List<int> beansSequence = new List<int>();
        public List<int> BeansSequence { get => beansSequence; }

        public List<List<IBingoCellBase>> _activeLines = new List<List<IBingoCellBase>>();
        public IEnumerable<IList<IBingoCellBase>> ActiveLines
        { 
            get => _activeLines.Select(x => x.ToList());
        }

        public void NewGamePad()
        {
            _cellPad.Clear();
            BeansSequence.Clear();
            for (int i = 1; i <= GamePadSize; i++)
            {
                for (int j = 1; j <= GamePadSize; j++)
                {
                    _cellPad.Add(new BingoCell(i, j, Engine.GetNextCellValue(this)));
                }
            }            
        }

        public IBingoGamePad GamePad(string gameUID)
        {
            return Engine.GamePad(gameUID);
        }

        public bool CheckWinnerLines(IBingoGamePad gamePad)
        {
            bool result = false;
            if (gamePad != null)
            {
                IBingoWinnerLines winLines = (IBingoWinnerLines)gamePad;

                result = winLines.ActiveLines
                    .Any(l => l
                        .All(i => gamePad.gamePadItems
                            .Count(_ => _.Row == i.Row && _.Col == i.Col && _.IsActive) >= 1));
                if (result)
                {
                    gamePad.ClearInactiveLinesOnWin();
                }
            }            
            return result;
        }

        public void ClearInactiveLinesOnWin()
        {
            _activeLines.RemoveAll(l => l
                .Any(i => gamePadItems
                    .Count(_ => _.Row == i.Row && _.Col == i.Col && !_.IsActive) != 0));
        }

        public void ClearActiveLines()
        {
            _activeLines.Clear();
        }

        private void InitializeWinnerLines()
        {
            List<IBingoCellBase> line;
            List<List<IBingoCellBase>> columns;
            List<IBingoCellBase> diagonal1;
            List<IBingoCellBase> diagonal2;

            columns = new List<List<IBingoCellBase>>();
            diagonal1 = new List<IBingoCellBase>();
            diagonal2 = new List<IBingoCellBase>();
            for (int i = 1; i <= GamePadSize; i++)
            {
                line = new List<IBingoCellBase>();
                columns.Add(new List<IBingoCellBase>());

                for (int j = 1; j <= GamePadSize; j++)
                {
                    line.Add(new BingoCellBase(i, j, 0));
                    columns[i-1].Add(new BingoCellBase(j, i, 0));
                    if (i == j)
                    {
                        diagonal1.Add(new BingoCellBase(i, j, 0));
                        diagonal2.Add(new BingoCellBase(GamePadSize - i + 1, j, 0));
                    }
                }

                _activeLines.Add(line);
            }
            _activeLines.AddRange(columns);
            _activeLines.Add(diagonal1);
            _activeLines.Add(diagonal2);
            maxActiveLines = _activeLines.Count;
        }

        public BingoGamePad(IBingoGameEngine engine, int gamePadSize)
        {
            _engine = engine;
            GamePadUID = Guid.NewGuid().ToString();
            GamePadSize = gamePadSize;
            InitializeWinnerLines();
        }
    }
}
