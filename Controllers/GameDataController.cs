using Microsoft.AspNetCore.Mvc;
using CognitiveOverloadLMS.Models;
using CognitiveOverloadLMS.Services;
using MongoDB.Driver;

namespace CognitiveOverloadLMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameDataController : ControllerBase
    {
        private readonly IMongoCollection<GameResult> _gameResults;
        private readonly IMongoCollection<UserSession> _userSessions;
        private readonly ILogger<GameDataController> _logger;

        public GameDataController(MongoDBService mongoDBService, ILogger<GameDataController> logger)
        {
            _gameResults = mongoDBService.GetCollection<GameResult>("GameResults");
            _userSessions = mongoDBService.GetCollection<UserSession>("UserSessions");
            _logger = logger;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { 
                success = true, 
                message = "GameData API is working!",
                timestamp = DateTime.UtcNow
            });
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new { 
                success = true, 
                message = "pong",
                timestamp = DateTime.UtcNow
            });
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveGameResult([FromBody] GameResult gameResult)
        {
            try
            {
                _logger.LogInformation("========== GAME DATA SAVE ATTEMPT ==========");
                _logger.LogInformation("Received game result for session: {SessionId}", gameResult?.SessionId);
                
                if (gameResult == null)
                {
                    _logger.LogError("GameResult is null");
                    return BadRequest(new { success = false, error = "GameResult is null" });
                }

                // Log the entire object
                _logger.LogInformation("Full GameResult: {@GameResult}", gameResult);

                // Validate required fields
                if (string.IsNullOrEmpty(gameResult.SessionId))
                {
                    _logger.LogError("SessionId is null or empty");
                    return BadRequest(new { success = false, error = "SessionId is required" });
                }

                // Check if session exists
                var session = await _userSessions.Find(s => s.Id == gameResult.SessionId).FirstOrDefaultAsync();
                if (session == null)
                {
                    _logger.LogError("Session not found: {SessionId}", gameResult.SessionId);
                    return BadRequest(new { success = false, error = $"Session not found: {gameResult.SessionId}" });
                }

                // Ensure collections are initialized
                gameResult.BehaviorData ??= new BehaviorData();
                gameResult.GameData ??= new GameSpecificData();
                
                // Calculate total time
                gameResult.TotalTimeSeconds = (gameResult.EndTime - gameResult.StartTime).TotalSeconds;
                
                // Calculate behavior averages
                if (gameResult.BehaviorData?.MouseMovements != null && gameResult.BehaviorData.MouseMovements.Any())
                {
                    gameResult.BehaviorData.AverageMouseSpeed = 
                        gameResult.BehaviorData.MouseMovements.Average(m => m.Speed);
                }
                
                // Save to GameResults collection first
                _logger.LogInformation("Attempting to insert into GameResults collection...");
                await _gameResults.InsertOneAsync(gameResult);
                _logger.LogInformation("Successfully inserted into GameResults with ID: {Id}", gameResult.Id);
                
                // Then add to UserSession's Games array
                _logger.LogInformation("Attempting to update UserSession...");
                var update = Builders<UserSession>.Update.Push(u => u.Games, gameResult);
                var updateResult = await _userSessions.UpdateOneAsync(
                    u => u.Id == gameResult.SessionId,
                    update
                );
                
                _logger.LogInformation("Update result - ModifiedCount: {ModifiedCount}, MatchedCount: {MatchedCount}", 
                    updateResult.ModifiedCount, updateResult.MatchedCount);
                
                if (updateResult.ModifiedCount > 0)
                {
                    _logger.LogInformation("✅ Successfully added game result to UserSession: {SessionId}", gameResult.SessionId);
                }
                else if (updateResult.MatchedCount > 0)
                {
                    _logger.LogWarning("Session found but document not modified. This might mean the game was already in the array?");
                }
                else
                {
                    _logger.LogError("❌ UserSession not found for ID: {SessionId}", gameResult.SessionId);
                }
                
                _logger.LogInformation("========== GAME DATA SAVE COMPLETE ==========");
                
                return Ok(new { success = true, id = gameResult.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error saving game result: {Message}", ex.Message);
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("session/{sessionId}")]
        public async Task<IActionResult> GetSessionResults(string sessionId)
        {
            try
            {
                var results = await _gameResults
                    .Find(r => r.SessionId == sessionId)
                    .ToListAsync();
                    
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
        
        [HttpGet("session/{sessionId}/summary")]
        public async Task<IActionResult> GetSessionSummary(string sessionId)
        {
            try
            {
                var session = await _userSessions
                    .Find(s => s.Id == sessionId)
                    .FirstOrDefaultAsync();
                    
                if (session == null)
                {
                    return NotFound(new { success = false, error = "Session not found" });
                }
                
                return Ok(new { 
                    success = true,
                    userName = session.UserName,
                    startTime = session.StartTime,
                    gamesPlayed = session.Games?.Count ?? 0,
                    games = session.Games
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
    }
}