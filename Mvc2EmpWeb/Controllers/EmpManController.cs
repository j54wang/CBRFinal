using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EmpDac;
using Mvc2EmpWeb.Models;

namespace Mvc2EmpWeb.Controllers
{
    public class EmpManController : Controller
    {
        private IEmpRepository empRepo = null;
        //
        // GET: /EmpMan/Create

        public EmpManController()
        {
            if ( System.Configuration.ConfigurationManager.AppSettings["RepoType"] == "ef" )
                empRepo = new EFEmpRepository();
            else if (System.Configuration.ConfigurationManager.AppSettings["RepoType"] == "mock")
            {
                empRepo = new MockEmpRepository();
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Search()
        {
            if (empRepo == null)
                throw new Exception("No EFEmpRepository");
            return View();
        }

        //
        // POST: /EmpMan/Create

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    string fn = collection["FirstName"];
                    string ln = collection["LastName"];

                    List<Emp> lst = (from c in empRepo.Emps where c.FirstName == fn && c.LastName == ln select c).ToList<Emp>();
                    EmpLib.mvcEmp mEmp = null;
                    if (lst.Count() == 1)
                    {
                        Emp emp = lst[0];
                        System.Web.HttpContext.Current.Session["NewEmp"] = false;
                        mEmp = new EmpLib.mvcEmp();
                        mEmp.FirstName = emp.FirstName;
                        mEmp.LastName = emp.LastName;
                        mEmp.Manager = emp.Manager;
                        mEmp.Notes = string.Empty;
                        System.Web.HttpContext.Current.Session["Emp"] = mEmp;
                    }
                    else
                    {
                        System.Web.HttpContext.Current.Session["NewEmp"] = true;
                        mEmp = new EmpLib.mvcEmp();
                        mEmp.FirstName = fn;
                        mEmp.LastName = ln;
                        mEmp.Manager = string.Empty;
                        mEmp.Notes = string.Empty;
                        System.Web.HttpContext.Current.Session["Emp"] = mEmp;
                    }

                    return RedirectToAction("Edit");
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /EmpMan/Edit
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit()
        {
            EmpLib.mvcEmp tmp = System.Web.HttpContext.Current.Session["Emp"] as EmpLib.mvcEmp;
            SetEditData(tmp);
            return View(tmp);
        }

        //
        // POST: /EmpMan/Edit

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                string msg = string.Empty;
                EmpLib.CBREmpLib lb = new EmpLib.CBREmpLib("ef");
                string id = string.Format("{0}_{1}", collection["LastName"], collection["FirstName"]);
                if (Convert.ToBoolean(System.Web.HttpContext.Current.Session["NewEmp"]))
                {
                    msg = lb.InsertEmp(id, collection["FirstName"], collection["LastName"], collection["ddlMgrs"]);
                }
                else
                {
                    msg = lb.UpdateEmp(collection["FirstName"], collection["LastName"], collection["ddlMgrs"]);
                }
                ViewData["EditMessage"] = msg;
                if (!string.IsNullOrEmpty(msg))
                {
                    EmpLib.mvcEmp tmp = new EmpLib.mvcEmp();
                    tmp.FirstName = collection["FirstName"];
                    tmp.LastName = collection["LastName"];
                    tmp.Notes = msg;
                    SetEditData(tmp);
                    return View(tmp);
                }
                return RedirectToAction("Search");
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return View();
            }
        }

        private EmpLib.mvcEmp SetEditData(EmpLib.mvcEmp tmp)
        {
            List<EmpMan> emps = Helpers.FormatManager(empRepo, tmp.LastName, tmp.FirstName);
            ViewData["ddlMgrs"] = new SelectList(emps,               //items
                                              "manid",             // datavaluefield
                                              "manid",             // datatextfield
                                               tmp.Manager);        // selected value
            return tmp;
        }

    }
}
