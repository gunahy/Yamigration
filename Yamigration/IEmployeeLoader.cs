using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yamigration
{
    internal interface IEmployeeLoader
    {
        List<Employee> LoadEmployees();
    }
}
