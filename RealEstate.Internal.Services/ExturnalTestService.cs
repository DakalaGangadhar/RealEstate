using RealEstate.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Internal.Services
{
    public class ExturnalTestService : IExturnalTestService
    {
        public string ExturnalTest()
        {
            return "Testing done";
        }
    }
}
