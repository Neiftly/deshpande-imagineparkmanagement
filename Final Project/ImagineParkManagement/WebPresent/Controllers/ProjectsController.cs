using DataObjects;
using LogicLayer;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebPresent.Models;

namespace WebPresent.Controllers
{
    public class ProjectsController : Controller
    {

        private ProjectManager _projectManager = new ProjectManager();
        private WorkerManager _workerManager = new WorkerManager();
        private ToolManager _toolManager = new ToolManager();
        private IEnumerable<Tool> _tools;
        private IEnumerable<int> _workerIDs;
        private IEnumerable<string> _projectNames;
        IEnumerable<ProjectViewModel> _projects;



        public ProjectsController()
        {
            _workerIDs = _workerManager.RetrieveAllWorkerIDByActive();
            _projectNames = _projectManager.RetrieveAllProjectNames();
            _tools = _toolManager.RetrieveAllTools();
                

            
        }

        // GET: Project/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Workers = _workerIDs;
            ViewBag.ProjectNames = _projectNames;
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        public ActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_projectManager.AddNewProject(project))
                    {
                        return RedirectToAction("Index");
                    }
                }
                catch 
                {
                    ViewBag.Workers = _workerIDs;
                    ViewBag.ProjectNames = _projectNames;
                    return View();
                }
            }

            ViewBag.Workers = _workerIDs;
            ViewBag.ProjectNames = _projectNames;
            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                IEnumerable<ProjectViewModel> _projects = _projectManager.RetrieveAllProjects();

                return View(_projects);
            } else
            {
                return Redirect("~/");
            }
                
        }

        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("Index");

            }
            ViewBag.Tools = _projectManager.RetrieveToolsByProjectID(id);
            var project = _projectManager.RetrieveProjectByID(id);
            return View(project);
        }

        public ActionResult MyProjects()
        {
            var email = User.Identity.Name;
            if (email == null)
            {
                return RedirectToAction("Index", "Home");

            }
            var workerManager = new WorkerManager();
            try
            {
                var user = workerManager.RetrieveWorkersByActive().Find(u => u.Email == email.ToLower());
                _projects = _projectManager.RetrieveProjectsByWorkerID(user.WorkerID);
            }
            catch (Exception)
            {

                RedirectToAction("Index", "Home");
            }

            if(_projects == null)
            {
                ViewBag.Message = "You do not have any assigned projects. Please speak to your manager.";
            }
            
            
            
            return View(_projects);
        }

        public ActionResult FinishProject(int? projectID)
        {
            var userName = User.Identity.Name;
            var workerManager = new WorkerManager();
            var user = workerManager.RetrieveWorkersByActive().Find(u => u.Email == userName.ToLower());
            if (projectID == null)
            {
                return RedirectToAction("MyProjects", user.WorkerID);
            }
            try
            {
                _projectManager.DeactivateProject(_projectManager.RetrieveProjectByID(projectID));
                return RedirectToAction("MyProjects", user.WorkerID);
            }
            catch (Exception)
            {
                ViewBag.Message = "That Project Did Not Exist.";
                return RedirectToAction("MyProjects", user.WorkerID);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CreateProjectName()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateProjectName(ProjectNameViewModel model)
        {
            var projectManager = new ProjectManager();
            try
            {
                projectManager.AddNewProjectName(model.ProjectName, model.ProjectDescription);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.Message = "Could not add project type.";
                return View();
            }
        }
    }
}