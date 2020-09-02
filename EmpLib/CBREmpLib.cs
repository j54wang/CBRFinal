using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.ComponentModel.DataAnnotations;

using EmpDac;

namespace EmpLib
{

    // the middle tier -- Middle tier
    public class CBREmpLib
    {
        private EmpDac.IDac _dac = null;

        public List<Emp> Emps { get; set; }

        private CBREmpLib() { }

        public CBREmpLib(string dbType)
        {
            if (dbType != "ef" && dbType != "mock")
                throw new Exception("The type is not supported");

            if (_dac == null)
            {
                EmpDac.DacFactory fac = new DacFactory(dbType);
                _dac = fac.GetDac(dbType);
            }
        }

        //private IDac GetDac(string dbType)
        //{
        //    if (_dac == null)
        //        throw new Exception("GetDac -- Dac not available");
        //    return _dac;
        //}

        public List<Emp> GetEmp(string sFn, string sLn)
        {
            if (_dac == null)
                throw new Exception("No data acess layer!");
            return _dac.GetEmp(sFn, sLn) as List<Emp>;
        }

        public List<Emp> GetEmps()
        {
            if (_dac == null)
                throw new Exception("No data acess layer!");
            return _dac.GetEmps();
        }

        public string InsertEmp(string sId, string sfn, string sln, string man)
        {
            string msg = string.Empty;
            try
            {
                if (_dac == null)
                    throw new Exception("No data acess layer!");
                List<Emp> emp = GetEmp(sfn, sln);
                if (emp != null && emp.Count == 0)  // emp == null could be serious problem
                    msg = _dac.InsertEmp(sId, sln, sfn, man);
                else if (emp != null && emp.Count > 0)
                {
                    msg = "Record exists already!";
                }
                else
                {
                    throw new Exception("Insert Emp Error!");
                }
            }
            catch (Exception ex)
            {
                msg = string.Format("Exception EmpLib InsertEmp -- {0}", ex.Message);
            }
            return msg;
        }

        public string UpdateEmp(string sfn, string sln, string man)
        {
            string msg = string.Empty;
            try
            {
                if (_dac == null)
                    throw new Exception("No data acess layer!");
                List<Emp> emp = GetEmp(sfn, sln);
                if (emp != null && emp.Count == 1)
                    msg = _dac.UpdateEmp(sln, sfn, man);
                else if (emp != null && emp.Count == 0)
                    msg = "Record not Exist!";
                else if (emp != null && emp.Count > 1)
                    msg = "More than one record found";
                else
                {
                    throw new Exception("Update Emp Error!");
                }
            }
            catch (Exception ex)
            {
                msg = string.Format("Exception EmpLib UpdateEmp -- {0}", ex.Message);
            }
            return msg;
        }
    }

    public class mvcEmp
    {
        [Required(ErrorMessage = "First Name Required")]
        [StringLength(100, ErrorMessage = "Must be under 100 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name Required")]
        [StringLength(100, ErrorMessage = "Must be under 100 characters")]
        public string LastName { get; set; }

        public string Manager { get; set; }

        public string Notes { get; set; }
    }
}
