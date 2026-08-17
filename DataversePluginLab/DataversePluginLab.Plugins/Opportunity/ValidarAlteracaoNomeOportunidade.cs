using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace DataversePluginLab.Plugins.Opportunity
{
    public class ValidarAlteracaoNomeOportunidade : PluginBase
    {
        protected override void ExecutePlugin()
        {
            Entity target = ObterTarget();

            if (target == null)
            {
                return;
            }
            string nomeOportunidade = target.GetAttributeValue<string>("gbc_name");

            if (!Context.PreEntityImages.Contains("PreImage"))
            {
                return;
            }

            Entity preImage = Context.PreEntityImages["PreImage"];
            string nomeAnterior = preImage.GetAttributeValue<string>("gbc_name");

            if(nomeAnterior != nomeOportunidade)
            {
                TracingService.Trace("Nome da oportunidade alterado de '{0}' para '{1}' ", nomeAnterior, nomeOportunidade);
            }

        }
        private Entity ObterTarget()
        {
            if (!Context.InputParameters.Contains("Target") || !(Context.InputParameters["Target"] is Entity target))
            {
                return null;
            }
            return target;
        }
    }
}
