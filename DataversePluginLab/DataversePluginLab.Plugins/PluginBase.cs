using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataversePluginLab.Plugins
{
    public abstract class PluginBase : IPlugin
    {
        protected ITracingService TracingService;
        protected IPluginExecutionContext Context;
        protected IOrganizationService Service;
        public void Execute(IServiceProvider serviceProvider)
        {
            TracingService = 
                (ITracingService)serviceProvider.GetService(
                    typeof(ITracingService)
            );
            Context = 
                (IPluginExecutionContext)serviceProvider.GetService(
                    typeof(IPluginExecutionContext)
            );
            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(
                    typeof(IOrganizationServiceFactory)
            );
            Service = serviceFactory.CreateOrganizationService(
                Context.UserId
            );
            ExecutePlugin();

        }
        protected abstract void ExecutePlugin();
    }
}
