using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace EmpDac
{
    public interface IDac
    {
        // retrieve empoyee information by name
        List<Emp> GetEmp(string fn, string ln);
        // retrieve all employees for make manager selection dropdown
        List<Emp> GetEmps();
        // insert new emp to emp table
        string InsertEmp(string sId, string ln, string fn, string mn);
        // update emp info
        string UpdateEmp(string ln, string fn, string mn);
    }
}
