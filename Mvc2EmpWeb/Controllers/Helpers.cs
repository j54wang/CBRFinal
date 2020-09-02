using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using EmpDac;
using Mvc2EmpWeb.Models;

namespace Mvc2EmpWeb.Controllers
{
    public class Helpers
    {
        public static List<EmpMan> FormatManager(IEmpRepository empRepo, string ln, string fn)
        {
            var lst2 = (from c in empRepo.Emps where !(c.FirstName == fn && c.LastName == ln) select c).Distinct();
            List<Emp> emps = lst2.ToList<Emp>();
            List<EmpMan> em = new List<EmpMan>();
            foreach (Emp ep in emps)
            {
                string smn = ep.FirstName + "_" + ep.LastName;
                if (smn.Length >= 100)
                    smn = smn.Substring(0, 98);
                EmpMan nmn = new EmpMan();
                nmn.manid = smn;
                if (!em.Contains(nmn))
                    em.Add(nmn);
            }
            return em;
        }

    }

    public class EmpMan
    {
        public string manid { get; set; }
    }
}
