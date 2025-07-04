using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

using EmployeeManagementSystem.DAL;
using EmployeeManagementSystem.Entities;

namespace EmployeeManagementSystem.BLL
{
    public class EmployeeBLL
    {
        private readonly EmployeeDAL _employeeDAL;

        public EmployeeBLL(IConfiguration configuration)
        {
            _employeeDAL = new EmployeeDAL(configuration);
        }

        public List<Employee> GetAllEmployees()
        {
            return _employeeDAL.GetAllEmployees();
        }
    }
}
