using BingoWebApp.Services.Interfaces;

namespace BingoWebApp.Services
{
    public class BingoGameEngine : IBingoGameEngine
    {
        private readonly int MaxGameCellValue = 52;
        private readonly int MaxGameBeansValue = 25;

        private readonly int GamePadSize = 5;

        public int MaxBeansCount => MaxGameBeansValue;

        private Mutex _engineLocker = new Mutex();

        private Dictionary<string, IBingoGamePad> gamePads = new Dictionary<string, IBingoGamePad>();
        
        public BingoGameEngine()
        {
            ;
        }

        public int GetNextCellValue(IBingoGamePad gamePad)
        {
            _engineLocker.WaitOne();
            try
            {
                int result = BingoValuesGenerator.GetNextRandomValue(MaxGameCellValue);
                while (gamePad.gamePadItems.Count(_ => _.Value == result) > 0)
                {
                    result = BingoValuesGenerator.GetNextRandomValue(MaxGameCellValue);
                }
                return result;
            }
            finally
            {
                _engineLocker.ReleaseMutex();
            }

        }

        public IBingoGamePad NewGame()
        {
            _engineLocker.WaitOne();
            try
            {
                BingoGamePad gamePad = new BingoGamePad(this, GamePadSize);
                gamePad.NewGamePad();

                gamePads.Remove(gamePad.GamePadUID);
                gamePads.Add(gamePad.GamePadUID, gamePad);

                return gamePad;
            }
            finally
            {
                _engineLocker.ReleaseMutex();
            }
        }

        public IBingoGamePad GamePad(string gameUID)
        {
            return gamePads[gameUID];
        }

        public int NextBean(string gameUID)
        {
            _engineLocker.WaitOne();
            try
            {
                int result = 0;
                if (gamePads.ContainsKey(gameUID))
                {
                    IBingoGamePad? gamePad = gamePads.GetValueOrDefault(gameUID);
                    if (gamePad != null)
                    {
                        if (gamePad.BeansSequence.Count < MaxBeansCount)
                        {
                            result = BingoValuesGenerator.GetNextRandomValue(MaxGameCellValue);
                            while (gamePad.BeansSequence.Count(_ => _ == result) > 0)
                            {
                                result = BingoValuesGenerator.GetNextRandomValue(MaxGameCellValue);
                            }
                        }
                    }

                    // Only for debug tests...
                    //Random rnd = new Random();
                    //result = rnd.Next(5) + 1;
                    //result = gamePad.gamePadItems.Where(_ => _.Row == 5 - result + 1 && _.Col == result)
                    //    .Select(_ => _.Value).FirstOrDefault();
                }
                return result;
            }
            finally {
                _engineLocker.ReleaseMutex();
            }
        }

        public bool SayBingo(string gameUID)
        {
            _engineLocker.WaitOne();
            try
            {
                bool result = false;
                if (gamePads.ContainsKey(gameUID))
                {
                    IBingoGamePad? gamePad = gamePads.GetValueOrDefault(gameUID);
                    if (gamePad != null)
                    {
                        result = ((IBingoWinnerLines)gamePad).CheckWinnerLines(gamePad);
                    }
                }
                return result;
            }
            finally
            {
                _engineLocker.ReleaseMutex();
            }

        }
    }
}
