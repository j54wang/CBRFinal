using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace EmpDac
{
    // entityframework data access layer
    class EFDac : IDac
    {
        private static IDac _dac = null;
        public static IDac GetDAC()
        {
            if (_dac == null)
            {
                EFDac dc = new EFDac();
                if (dc.EmpEntity == null)
                {
                    dc.EmpEntity = new CBREmpEntities();
                }
                _dac = dc;
            }
            return _dac;
        }

        private EFDac() { }

        public CBREmpEntities EmpEntity {get; private set;}

        public List<Emp> GetEmp(string fn, string ln)
        {
            IList<Emp> employees = EmpEntity.Emp.ToList<Emp>();

            var query = from e in employees
                        where e.FirstName == fn && e.LastName == ln
                        select e;

            IList<Emp> result = query.ToList<Emp>();

            return result.ToList();

        }

        public List<Emp> GetEmps()
        {
            IList<Emp> employees = EmpEntity.Emp.ToList<Emp>();

            return employees.ToList();
        }

        public string InsertEmp(string sId, string ln, string fn, string man)
        {
            return AddEmpoyee(sId, fn, ln, man);
        }

        public string UpdateEmp(string ln, string fn, string man)
        {
            string msg = string.Empty;
            try
            {
                Emp EmpObj = EmpEntity.Emp.First(i => i.FirstName == fn && i.LastName == ln);
                if (EmpObj != null)
                {
                    EmpObj.Manager = man;
                    EmpEntity.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = string.Format("Exception EFDac UpdateEmp -- {0}", ex.Message);
            }
            return msg;
        }

        private string AddEmpoyee(string sId, string fn, string ln, string man)
        {
            string msg = string.Empty;
            try
            {
                Emp ep = new Emp();
                ep.EmpID = sId;
                ep.FirstName = fn;
                ep.LastName = ln;
                ep.Manager = man;
                EmpEntity.AddToEmp(ep);
                EmpEntity.SaveChanges();
            }
            catch (Exception ex)
            {
                msg = string.Format("Exception EFDac AddEmployee -- {0}", ex.Message);
            }
            return msg;
        }
    }


    // Data Access Layer factory -- create dataaccess layer as required
    public class DacFactory
    {
        private string myDBType;

        private DacFactory() { }

        public DacFactory(string dbType)
        {
            myDBType = dbType;
            //connStr = conn;
        }

        public IDac GetDac(string dbtypr)
        {
            if (string.IsNullOrEmpty(myDBType))
                throw new Exception("Database Server type rquired."); // No database server type provided, cannot continue;
            if (myDBType.ToLower() == "ef")
            {
                return EFDac.GetDAC();
            }
            else if (myDBType.ToLower() == "mock")
            {
                return MockDac.GetDAC();
            }
            else
                throw new Exception("Data Access to be implemented."); // No dac defined yet, cannot continue;
        }
    }
}
