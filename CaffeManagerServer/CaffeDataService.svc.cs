//------------------------------------------------------------------------------
// <copyright file="WebDataService.svc.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using CaffeManagerServer.Context;
using CaffeManagerServer.Model;
using System;
using System.Data.Services;
using System.Data.Services.Common;
using System.Data.Services.Providers;
using System.Linq.Expressions;
using System.ServiceModel;
using System.Web;
using static System.Net.WebRequestMethods;

namespace CaffeManagerServer
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class CaffeDataService : DataService<CaffeDbContext>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.UseVerboseErrors = true;
            config.SetEntitySetAccessRule("*", EntitySetRights.AllRead);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
        }

       
    }
}
