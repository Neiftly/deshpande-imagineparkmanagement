using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net;
using DataObjects;
using LogicLayer;

namespace WebPresent.Controllers
{
    [Authorize(Roles = "Admin, Park Staff Admin")]
    public class AdminController : Controller
    {

        // GET: Admin
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var users = userManager.Users.ToList().OrderBy(n => n.Surname);


                return View(users);
            } else
            {
                return Redirect("~/");
            }
            
        }

        public ActionResult Details(string id)
        {
            if(id == null)
            {
                return RedirectToAction("Index");
            }
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.Users.First(u => u.Id == id);

            if(user == null)
            {
                return HttpNotFound();
            }

            // roles viewbag
            var allRoles = new string[] {"Intern", "Park Manager", "Park Staff Admin", "QuarterMaster", "Volunteer"};
            var roles = userManager.GetRoles(id);
            var noRoles = allRoles.Except(roles);

            ViewBag.Roles = roles;
            ViewBag.NoRoles = noRoles;

            return View(user);
        }

        public ActionResult RemoveRole(string id, string role)
        {
            // identity system
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.Users.First(u => u.Id == id);

            // prevent first admin from being deleted.
            if(user.Email.Contains("admin") && role == "Admin")
            {
                Response.Write("<script>Alert('Cannot Remove This User from Admin Role');</script>");
                return RedirectToAction("Details", new { Id = id });
            }

            userManager.RemoveFromRole(id, role);

            // older system
            var oldWorkerManager = new LogicLayer.WorkerManager();
            var oldWorker = oldWorkerManager.RetrieveWorkersByActive().Find(e => e.Email == user.Email);

            if (oldWorker != null)
            {
                oldWorkerManager.SynchronizeRoles(oldWorker.WorkerID, userManager.GetRoles(user.Id).ToList());
            }

            return RedirectToAction("Details", new { Id = id });
        }

        public ActionResult AddRole(string id, string role)
        {
            // identity system
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.Users.First(u => u.Id == id);

            userManager.AddToRole(id, role);

            // older system
            var oldWorkerManager = new LogicLayer.WorkerManager();
            var oldWorker = oldWorkerManager.RetrieveWorkersByActive().Find(e => e.Email == user.Email);

            if (oldWorker != null)
            {
                oldWorkerManager.SynchronizeRoles(oldWorker.WorkerID, userManager.GetRoles(user.Id).ToList());
            }

            return RedirectToAction("Details", new { Id = id });
        }

        [HttpGet]
        public ActionResult RegisterWorker()
        {
            var roles =  new string[] { "Intern", "Park Manager", "Park Staff Admin", "QuarterMaster", "Volunteer" };
            ViewBag.Roles = roles;
            return View();
        }

        [HttpPost]
        public ActionResult RegisterWorker(WorkerViewModel model)
        {
            WorkerManager _workerManager = new WorkerManager();
            if (ModelState.IsValid)
            {
                var worker = new WorkerViewModel // creates worker object
                {
                    LastName = model.LastName,
                    FirstName = model.FirstName,
                    StreetAddress = model.StreetAddress,
                    ZIPCode = model.ZIPCode,
                    Phone = model.Phone,
                    Email = model.Email,
                    Active = true,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.MinValue
                };
                List<string> roles = new List<string>();
                roles.Add("Volunteer");
                try
                {
                    _workerManager.AddNewWorker(worker);
                    return View();
                }
                catch (Exception ex)
                {
                    ViewBag.Message ="Email Already in Use Please Use a Different Email.";
                    return View();
                }
            }

            return View(model);
        }

        public ActionResult Workers()
        {
            var workerManager = new WorkerManager();
            var workers = workerManager.RetrieveWorkersByActive().ToList().OrderBy(w => w.WorkerID);

            return View(workers);
        }

        public ActionResult WorkerDetails(int? id)
        {
            if (id == null)
            {
                RedirectToAction("Workers");
            }
            var workerManager = new WorkerManager();
            var worker = workerManager.RetreiveWorkerByID(id);

            if (worker == null)
            {
                return HttpNotFound();
            }

            var allRoles = new string[] { "Intern", "Park Manager", "Park Staff Admin", "QuarterMaster", "Volunteer" };
            var roles = worker.Roles;
            var noRoles = allRoles.Except(roles);

            ViewBag.Roles = roles;
            ViewBag.NoRoles = noRoles;

            return View(worker);
        }

        public ActionResult EditWorker(int? id)
        {
            if(id == null)
            {
                RedirectToAction("Workers");
            }
            var workerManager = new WorkerManager();
            var worker = workerManager.RetreiveWorkerByID(id);
            if(worker == null)
            {
                return HttpNotFound();
            }
            var allRoles = new string[] { "Intern", "Park Manager", "Park Staff Admin", "QuarterMaster", "Volunteer" };
            var roles = worker.Roles;
            var noRoles = allRoles.Except(roles);

            ViewBag.Roles = roles;
            ViewBag.NoRoles = noRoles;
            return View(worker);
        }

        public ActionResult InActiveWorkers()
        {
            var workerManager = new WorkerManager();
            var workers = workerManager.RetrieveWorkersByActive(false).ToList().OrderBy(w => w.WorkerID);
            return View(workers);
        }

        public ActionResult Activate(int? id)
        {
            var workerManager = new WorkerManager();
            if (id == null)
            {
                return RedirectToAction("InActiveWorkers");
            }
            try
            {
                var worker = workerManager.RetreiveWorkerByID(id);
                workerManager.ReactivateWorkerByID(id);
            }
            catch (Exception)
            {

                return RedirectToAction("InActiveWorkers");
            }
            
            return RedirectToAction("InActiveWorkers");
        }

        public ActionResult Deactivate(int? id)
        {
            var workerManager = new WorkerManager();
            if(id == null)
            {
                return RedirectToAction("Workers");
            }
            try
            {
                var worker = workerManager.RetreiveWorkerByID(id);
                workerManager.DeactivateWorkerByID(id);
            }
            catch (Exception)
            {
                return RedirectToAction("Workers");
            }
            
            return RedirectToAction("Workers");
        }
    }
}