//------------------------------------------------------------------------------
// <copyright file="WebDataService.svc.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using CaffeManagerServer.Context;
using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Data.Services.Providers;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;
using System.Data.Entity;

namespace CaffeManagerServer
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class CaffeManagerDataService : DataService< CaffeDbContext >
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("*", EntitySetRights.AllRead);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
        }
        protected override void OnStartProcessingRequest(ProcessRequestArgs args)
        {
            base.OnStartProcessingRequest(args);
        }
        protected override CaffeDbContext CreateDataSource()
        {
            return base.CreateDataSource();
        }
        protected override void HandleException(HandleExceptionArgs args)
        {
            base.HandleException(args);
        }

     }
}
