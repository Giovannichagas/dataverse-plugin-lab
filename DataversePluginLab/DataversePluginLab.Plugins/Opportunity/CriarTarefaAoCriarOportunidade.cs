using Microsoft.Xrm.Sdk;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace DataversePluginLab.Plugins.Opportunity
{
    public class CriarTarefaAoCriarOportunidade : PluginBase
    {
        protected override void ExecutePlugin()
        {
            TracingService.Trace("Iniciando plugin");

            Entity oportunidade = ObterTarget();

            TracingService.Trace("Target obtido");

            if (oportunidade == null)
            {
                TracingService.Trace("Target nulo, encerrando plugin");
                return;
            }

            string nomeOportunidade = ObterNomeOportunidade(oportunidade);
            
                if (string.IsNullOrWhiteSpace(nomeOportunidade))
                {
                    nomeOportunidade = "Sem nome";
                }
            

            TracingService.Trace("Criando tarefa");

            Entity tarefa = new Entity("task");

            tarefa["subject"] = "Entrar em contato com o cliente - " + nomeOportunidade;

            tarefa["scheduledend"] = DateTime.UtcNow.AddDays(2);
            tarefa["prioritycode"] = new OptionSetValue(2);

            tarefa["regardingobjectid"] =
                oportunidade.ToEntityReference();

            Service.Create(tarefa);

            TracingService.Trace("Tarefa criada com sucesso");
        }

        private Entity ObterTarget()
        {
            if (!Context.InputParameters.Contains("Target") || !(Context.InputParameters["Target"] is Entity target))
            {
                return null;
            }
            
            return target;
        }

      
        private string ObterNomeOportunidade(Entity target)
        {
            return target.GetAttributeValue<string>("gbc_name");
        }
    }
}
