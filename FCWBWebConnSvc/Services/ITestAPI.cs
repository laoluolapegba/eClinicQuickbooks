using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace FCQBWebConnAPI.Services
{
    [ServiceContract]
    interface ITestAPI
    {
        [OperationContract]
        bool IsWorkAvailable();
    }
   
}