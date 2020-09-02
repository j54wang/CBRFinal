using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using EmpDac;

namespace Mvc2EmpWeb.Models
{
    public class MockEmpRepository : IEmpRepository
    {

        public IQueryable<Emp> Emps
        {
            get
            {
                List<Emp> emps = CreateEmpList();
                return emps.AsQueryable();
            }
        }

        private List<Emp> CreateEmpList()
        {
            List<Emp> lst = new List<Emp>()
            {
                new Emp(){FirstName="Testa", LastName="Test", Manager=""},
                new Emp(){FirstName="Testb", LastName="Test", Manager="Testa_Test"},
                new Emp(){FirstName="Testc", LastName="Test", Manager="Testb_Test"},
                new Emp(){FirstName="Testd", LastName="Test", Manager="Testa_Test"},
                new Emp(){FirstName="Teste", LastName="Test", Manager="Testb_Test"},
                new Emp(){FirstName="Testf", LastName="Test", Manager="Testb_Test"}
            };
            return lst;
        }
    }
}
