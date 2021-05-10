using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebPresent.Controllers
{
    [Authorize(Roles = "QuarterMaster, Admin, Park Manager")]
    public class ToolsController : Controller
    {
        private ToolManager _toolManager = new ToolManager();
        private ProjectManager _projectManager = new ProjectManager();
        private int? _projectID;
        private IEnumerable<ProjectViewModel> _projects;
        private List<int?> _projectIds;
        // GET: Tools
        public ToolsController()
        {
            _projects = _projectManager.RetrieveAllProjects();
            _projectIds = _projects.Select(p => p.ProjectID).ToList();
        }
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View(_toolManager.RetrieveAllTools());
            } else
            {
                return Redirect("~/");
            }
            
        }

        public ActionResult Details(int? id)
        {
            int id2;
            if(id== null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                id2 = id ?? 0;
            }
            Tool t = (_toolManager.RetrieveAllTools()).Find(tool => tool.ToolID == id);
            if (t == null)
            {
                return HttpNotFound();
            }
            _projectID = _toolManager.RetrieveProjectByToolID(id2);
            ToolViewModel toolViewModel = new ToolViewModel(t.ToolID, t.ToolDescription, _projectID);
            ViewBag.Projects = _projectIds;
            ViewBag.ProjectID = _projectID;
            return View(toolViewModel);
        }
        public ActionResult CheckIn(int? pId, int? tId)
        {
            try
            {
                if (_toolManager.RemoveToolFromProject(pId, tId))
                {
                    RedirectToAction("Details", "ProjectController", pId);
                }
                Tool t = (_toolManager.RetrieveAllTools()).Find(tool => tool.ToolID == tId);
                ToolViewModel toolViewModel = new ToolViewModel(t.ToolID, t.ToolDescription, pId);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
           
            
        }
        public ActionResult CheckOut(int? pId, int? tId)
        {
            try
            {
                if(_toolManager.AddToolToProject(pId, tId))
                {
                    RedirectToAction("Index");
                }
                Tool t = (_toolManager.RetrieveAllTools()).Find(tool => tool.ToolID == tId);
                return RedirectToAction("Index");
            } catch (Exception)
            {
                throw;
            }
        }
        
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string toolDescription)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_toolManager.AddNewTool(toolDescription))
                    {
                        return RedirectToAction("Index");
                    }
                }
                catch(Exception)
                {
                    return View();
                }
            }
            return RedirectToAction("Index");
        }
    }
}