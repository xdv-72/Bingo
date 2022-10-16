using BingoWebApp.Models;
using BingoWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.JSInterop.Implementation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;

namespace BingoWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBingoGameEngine _gameEngine;

        public HomeController(ILogger<HomeController> logger, IBingoGameEngine gameEngine)
        {
            _logger = logger;
            _gameEngine = gameEngine;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetGamePadItems([FromQuery]string gamePadUID)
        {
            if (!string.IsNullOrEmpty(gamePadUID))
            {
                return  new OkObjectResult(_gameEngine.GamePad(gamePadUID).gamePadItems
                    .Select(_ => new { _.Row, _.Col, _.IsActive }).ToList());
            }
            else
            {
                return Error();
            }
        }

        [HttpPost]
        public IActionResult NewGame()
        {
            IBingoGamePad gamePad = _gameEngine.NewGame();
            ViewData["gamePadUID"] = gamePad.GamePadUID;
            return View("~/Views/Home/Index.cshtml", gamePad);
        }

        [HttpPost]
        public IActionResult RefreshGamePad(string gamePadUID)
        {
            if (!string.IsNullOrEmpty(gamePadUID))
            {
                IBingoGamePad gamePad = _gameEngine.GamePad(gamePadUID);
                if (gamePad != null)
                {
                    return View("~/Views/Home/Index.cshtml", gamePad);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult OnCellClick(int row, int col, string? gameuid)
        {
            if (row >= 0 && col >= 0 && !string.IsNullOrEmpty(gameuid))
            {
                IBingoGamePad gamePad = _gameEngine.GamePad(gameuid);
                if (gamePad != null)
                {
                    ViewData["gamePadUID"] = gamePad.GamePadUID;
                    IBingoCell? cell = gamePad.gamePadItems
                        .Where(_ => _.Row == row && _.Col == col && 
                                gamePad.BeansSequence
                                    .Where(v => v == _.Value).Count() > 0
                        )
                        .FirstOrDefault();
                    if (cell != null && !cell.IsActive)
                    {
                        cell.IsActive = true;
                        if (((IBingoWinnerLines)gamePad).CheckWinnerLines(gamePad))
                            return Ok();
                        else
                            return Accepted();
                    }
                    else
                        return NoContent();
                }    
            }
            return NotFound();
        }

        [HttpPost]
        [Route("/Home/NextStep/{gameuid}")]
        public JsonResult NextStep(string gameuid)
        {
            IBingoGamePad gamePad = _gameEngine.GamePad(gameuid);
            if (gamePad != null)
            {
                if (gamePad.IsGameOver)
                {
                    return Json( new BingoStepResult(HttpStatusCode.NotFound, 0));
                }
                else
                {
                    int newValue = _gameEngine.NextBean(gameuid);
                    if (newValue > 0)
                    {
                        gamePad.gamePadItems.ToList().ForEach(_ => _.IsActive = _.Value == newValue ? true : _.IsActive);
                        gamePad.BeansSequence.Add(newValue);
                        if (((IBingoWinnerLines)gamePad).CheckWinnerLines(gamePad))
                            return Json(new BingoStepResult(HttpStatusCode.ResetContent, newValue));
                        
                        return Json( new BingoStepResult(HttpStatusCode.OK, newValue));
                    }
                    else
                    {
                        gamePad.ClearActiveLines();
                        return Json(new BingoStepResult(HttpStatusCode.NotFound, 0));
                    }
                }
            }
            return Json(new BingoStepResult(HttpStatusCode.Accepted, 0 ));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}