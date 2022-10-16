using BingoWebApp.Services.Interfaces;
using System.Security.Cryptography;

namespace BingoWebApp.Services
{
    public static class BingoValuesGenerator
    {
        public static Random randomGenerator = new Random();
        public static int GetNextRandomValue(int maxValue)
        {
            return randomGenerator.Next(maxValue - 1) + 1;

        }
    }
}
