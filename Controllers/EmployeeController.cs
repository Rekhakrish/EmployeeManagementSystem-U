using EmployeeManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    public class EmployeeController : Controller
    {
        public ActionResult Index(string searchTerm, int page = 1)
        {
            int pageSize = 5; // Only 5 records per page

            using (var context = new EmployeeContext())
            {
                var employeesQuery = context.Employees.AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    employeesQuery = employeesQuery.Where(e => e.Name.Contains(searchTerm));
                }

                int totalRecords = employeesQuery.Count();
                var employees = employeesQuery
                                    .OrderBy(e => e.EmployeeId)
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                ViewBag.SearchTerm = searchTerm;

                return View(employees);
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            using (var context = new EmployeeContext())
            {
                ViewBag.States = context.allState
                    .Select(s => new SelectListItem
                    {
                        Value = s.StateName,
                        Text = s.StateName
                    }).ToList();
            }

            var model = new EmployeeViewModel
            {
                DateOfJoin = DateTime.Now,
                 DateOfBirth = DateTime.Now
            };

            return View(model); // ← this ensures Model is not null
        }
        [HttpPost]
        public ActionResult Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (EmployeeContext context = new EmployeeContext())
                {
                    bool isDuplicate = context.Employees.Any(e => e.Name == model.Name && e.DateOfBirth == model.DateOfBirth);
                    if (isDuplicate)
                    {
                        ModelState.AddModelError("Name", "An employee with the same name and DOB already exists.");
                    }
                    else
                    {
                        var employee = new EmployeeViewModel
                        {
                            Name = model.Name,
                            Designation = model.Designation,
                            DateOfJoin = model.DateOfJoin,
                            Salary = model.Salary,
                            Gender = model.Gender,
                            State = model.State,
                            DateOfBirth = model.DateOfBirth
                        };
                        context.Employees.Add(employee);
                        context.SaveChanges();
                        TempData["Message"] = "Employee saved successfully!";
                        return RedirectToAction("Index");
                    }
                }
            }
            using (var context = new EmployeeContext())
            {
                ViewBag.States = context.allState
                    .Select(s => new SelectListItem
                    {
                        Value = s.StateName,
                        Text = s.StateName
                    }).ToList();
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var context = new EmployeeContext())
            {
                var employee = context.Employees.Find(id);
                if (employee == null)
                {
                    return HttpNotFound();
                }

                var model = new EmployeeViewModel
                {
                    EmployeeId = employee.EmployeeId,
                    Name = employee.Name,
                    Designation = employee.Designation,
                    DateOfJoin = employee.DateOfJoin,
                    Salary = employee.Salary,
                    Gender = employee.Gender,
                    State = employee.State,
                    DateOfBirth = employee.DateOfBirth
                };

                return PartialView("Edit", model); // or View("Edit", model);
            }
        }

        [HttpPost]
        public ActionResult Edit(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new EmployeeContext())
                {
                    var emp = context.Employees.Find(model.EmployeeId);
                    if (emp != null)
                    {
                        emp.Name = model.Name;
                        emp.Designation = model.Designation;
                        emp.DateOfJoin = model.DateOfJoin;
                        emp.Salary = model.Salary;
                        emp.Gender = model.Gender;
                        emp.State = model.State;
                        emp.DateOfBirth = model.DateOfBirth;
                        context.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return PartialView("Edit", model);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (var context = new EmployeeContext())
            {
                var emp = context.Employees.Find(id);
                if (emp != null)
                {
                    context.Employees.Remove(emp);
                    context.SaveChanges();
                    TempData["Message"] = "Employee deleted successfully.";
                }
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var context = new EmployeeContext())
            {
                var emp = context.Employees.Find(id);
                if (emp != null)
                {
                    context.Employees.Remove(emp);
                    context.SaveChanges();
                    TempData["Message"] = "Employee deleted successfully.";
                }
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public JsonResult DeleteMultiple(List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return Json(new { success = false, message = "No employees selected." });
            }

            using (var context = new EmployeeContext())
            {
                var employeesToDelete = context.Employees.Where(e => ids.Contains(e.EmployeeId)).ToList();
                context.Employees.RemoveRange(employeesToDelete);
                context.SaveChanges();
            }

            return Json(new { success = true, message = "Selected employees deleted successfully." });
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}
