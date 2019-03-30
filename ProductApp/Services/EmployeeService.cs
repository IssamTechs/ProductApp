using GenFu;
using ProductApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductApp.Services
{
    public class EmployeeService
    {
        private readonly List<Employee> _employees;

        public EmployeeService()
        {
            var employees = A.ListOf<Employee>(50);
            var i = 1;
            _employees = employees.Select(e =>
            {
                e.Id = i++;
                return e;
            }).ToList();
        }

        public IEnumerable<Employee> GetAll()
        {
            return _employees.ToList();
        }

        public Employee GetByID(int id)
        {
            return _employees.Where(e => e.Id == id).FirstOrDefault();
        }
    }
}