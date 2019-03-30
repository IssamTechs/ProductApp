using ProductApp.Controllers;
using System;
using Xunit;

namespace ProductApp.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var controller = new EmployeesController(new Services.EmployeeService());
            var result = controller.Get();
        }
    }
}