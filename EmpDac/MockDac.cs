using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmpDac
{
    // Mock data access layer for unit test
    class MockDac : IDac
    {
        private static IDac _dac = null;

        List<Emp> _emps = new List<Emp>()
            {
                new Emp(){FirstName="Testa", LastName="Test", Manager=""},
                new Emp(){FirstName="Testb", LastName="Test", Manager="Testa_Test"},
                new Emp(){FirstName="Testc", LastName="Test", Manager="Testb_Test"},
                new Emp(){FirstName="Testd", LastName="Test", Manager="Testa_Test"},
                new Emp(){FirstName="Teste", LastName="Test", Manager="Testb_Test"},
                new Emp(){FirstName="Testf", LastName="Test", Manager="Testb_Test"}
            };

        public static IDac GetDAC()
        {
            if (_dac == null)
            {
                MockDac dc = new MockDac();
                _dac = dc;
            }
            return _dac;
        }

        private MockDac() { }

        public List<Emp> GetEmp(string fn, string ln)
        {
            var query = from e in _emps
                        where e.FirstName == fn && e.LastName == ln
                        select e;

            IList<Emp> result = query.ToList<Emp>();

            return result.ToList();

        }

        public List<Emp> GetEmps()
        {
            return _emps;
        }

        public string InsertEmp(string sId, string ln, string fn, string man)
        {
            string msg = string.Empty;
            try
            {
                msg = AddEmpoyee(sId, fn, ln, man);
            }
            catch (Exception ex)
            {
                msg = string.Format("Exception MockDac InsertEmp -- {0}", ex.Message);
            }
            return msg;
        }

        public string UpdateEmp(string ln, string fn, string man)
        {
            string msg = string.Empty;
            try
            {
                Emp EmpObj = _emps.First(i => i.FirstName == fn && i.LastName == ln);
                _emps.Remove(EmpObj);
                if (EmpObj != null)
                {
                    EmpObj.Manager = man;
                }
                _emps.Add(EmpObj);
            }
            catch (Exception ex)
            {
                msg = string.Format("Exception MockDac UpdateEmp -- {0}", ex.Message);
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
                _emps.Add(ep);
            }
            catch (Exception ex)
            {
                msg = string.Format("Exception MockDac AddEmpoyee -- {0}", ex.Message);
            }
            return msg;
        }
    }
}
