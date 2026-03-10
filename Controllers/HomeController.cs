using Microsoft.AspNetCore.Mvc;
using CognitiveOverloadLMS.Models;
using CognitiveOverloadLMS.Services;
using MongoDB.Driver;

namespace CognitiveOverloadLMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMongoCollection<UserSession> _sessions;

        public HomeController(MongoDBService mongoDBService)
        {
            _sessions = mongoDBService.GetCollection<UserSession>("UserSessions");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StartSession([FromBody] string userName)
        {
            try
            {
                var session = new UserSession
                {
                    UserName = userName,
                    StartTime = DateTime.UtcNow,
                    Games = new List<GameResult>()
                };

                await _sessions.InsertOneAsync(session);
                
                return Ok(new { sessionId = session.Id, success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
        public IActionResult Results()
{
    return View();
}
    }
}