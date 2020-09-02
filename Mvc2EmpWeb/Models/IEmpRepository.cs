using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EmpDac; 

namespace Mvc2EmpWeb.Models
{
    public interface IEmpRepository
    {
        IQueryable<Emp> Emps { get; }
    }
}
