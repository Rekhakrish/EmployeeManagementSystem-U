using EmployeeManagementSystem.Models;
using System.Linq;
using System.Web.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    public class StateController : Controller
    {
        [HttpGet]
        public ActionResult CreateState()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateState(StateViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new EmployeeContext())
                {
                    context.allState.Add(model);
                    context.SaveChanges();
                }

                TempData["Message"] = "State added successfully!";
                return RedirectToAction("Create"); // Redirect to empty form again or list
            }

            return View(model);
        }
    }
}
