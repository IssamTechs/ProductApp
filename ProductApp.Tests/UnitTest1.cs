using ProductApp.Controllers;
using ProductApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ProductApp.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var controller = new EmployeesController(new Services.EmployeeService());
            var result = controller.Get() as IEnumerable<Employee>;
            Assert.NotNull(result);
            Assert.Equal(51, result.Count());
        }
    }
}