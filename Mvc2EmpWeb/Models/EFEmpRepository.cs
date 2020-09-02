using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using EmpDac;
using EmpLib;

namespace Mvc2EmpWeb.Models
{
    public class EFEmpRepository : IEmpRepository
    {
        public IQueryable<Emp> Emps 
        {
            get
            {               
                EmpLib.CBREmpLib lib = new CBREmpLib("ef");
                List<Emp> emps = lib.GetEmps();
                return emps.AsQueryable();
            }
        }
    }
}
