using Microsoft.AspNetCore.Mvc;

namespace CognitiveOverloadLMS.Controllers
{
    public class QuestionsController : Controller
    {
        public IActionResult Section1()
        {
            return View();
        }

        public IActionResult Section2()
        {
            return View();
        }

        public IActionResult Section3()
        {
            return View();
        }
    }
}