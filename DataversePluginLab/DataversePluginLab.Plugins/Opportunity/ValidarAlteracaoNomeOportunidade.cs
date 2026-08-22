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

            if(string.IsNullOrWhiteSpace(nomeOportunidade))
            {
                throw new InvalidPluginExecutionException("O nome da oportunidade não pode ser nulo ou vazio.");
            }

            if (!Context.PreEntityImages.Contains("PreImage"))
            {
                return;
            }

            Entity preImage = Context.PreEntityImages["PreImage"];
            string nomeAnterior = preImage.GetAttributeValue<string>("gbc_name");

            if(nomeAnterior != nomeOportunidade)
            {
                TracingService.Trace("Nome da oportunidade alterado de '{0}' para '{1}' ", nomeAnterior, nomeOportunidade);

                Entity anotacao = new Entity("annotation");
                anotacao["subject"] = "Alteração do nome da oportunidade";
                anotacao["notetext"] = $"O nome da oportunidade foi alterado de '{nomeAnterior}' para '{nomeOportunidade}'.";
                anotacao["objectid"] = target.ToEntityReference();
                Service.Create(anotacao);
                

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
