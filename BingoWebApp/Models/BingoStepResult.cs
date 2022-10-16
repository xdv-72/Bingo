using System.Net;

namespace BingoWebApp.Models
{
    public class BingoStepResult
    {
        public HttpStatusCode Status { get; set; }
        public int Value { get; set; }

        public BingoStepResult(HttpStatusCode status, int value)
        {
            Status = status;
            Value = value;
        }
    }
}
