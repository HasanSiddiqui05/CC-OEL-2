using System.Web.Mvc;
using CC_Lab_11.Models;
using CC_Lab_11.localhost; // Use the namespace of the service reference

namespace CC_Lab_11.Controllers
{
    public class ScriptCheckController : Controller
    {
        public ActionResult Index()
        {
            return View(new ScriptCheckModel());
        }

        [HttpPost]
        public ActionResult CheckScript(ScriptCheckModel model)
        {
            if (string.IsNullOrWhiteSpace(model.InputScript))
            {
                model.Result = "Please enter a script to check.";
                return View("Index", model);
            }

            // Use the generated client class
            using (var client = new CurrencyConverter())
            {
                model.Result = client.CheckForMaliciousPatterns(model.InputScript);
            }

            return View("Index", model);
        }
    }
}
