using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using System.Text;
using System.Threading.Tasks;


namespace DataversePluginLab.Plugins.Contact
{
    public class ValidarTelefone : PluginBase
    {
        protected override void ExecutePlugin()
        {
            Entity target = ObterTarget();

            if(target == null)
            {
                return;
            }

            string telefoneOriginal = ObterTelefone(target);

            if(string.IsNullOrWhiteSpace(telefoneOriginal))
            {
                return;
            }

            ValidarLetras(telefoneOriginal);
            string telefoneLimpo = LimparTelefone(telefoneOriginal);
            ValidarQuantidadeDigitos(telefoneLimpo, telefoneOriginal);
            ValidarDigitosIguais(telefoneLimpo, telefoneOriginal);

        }

        private Entity ObterTarget()
        {
            if(!Context.InputParameters.Contains("Target") || !(Context.InputParameters["Target"] is Entity target))
            {
                return null;
            }
            if (!target.Attributes.Contains("mobilephone"))
            {
                return null;
            }
            return target;
        }

        private string ObterTelefone(Entity target)
        {
            return target.GetAttributeValue<string>("mobilephone");
        }

        private void ValidarLetras(string telefoneOriginal)
        {
            bool contemLetras = telefoneOriginal.Any(char.IsLetter);

            if (!contemLetras)
            {
                return;
            }

            TracingService.Trace("Telefone contem letras: {0} ", telefoneOriginal);

            throw new InvalidPluginExecutionException("Telefone contem letras: " + telefoneOriginal);
        }

        private string LimparTelefone(string telefoneOriginal)
        {
            return new string(telefoneOriginal.Where(char.IsDigit).ToArray());
        }

        private void ValidarQuantidadeDigitos(string telefoneLimpo, string telefoneOriginal)
        {
            if (telefoneLimpo.Length == 11)
            {
                return;
            }

            TracingService.Trace("Telefone invalido: {0} ", telefoneOriginal);

            throw new InvalidPluginExecutionException("Telefone invalido. O telefone deve conter 11 dígitos");
        }

        private void ValidarDigitosIguais(string telefoneLimpo, string telefoneOriginal)
        {
            bool todosDigitosIguais = telefoneLimpo.All(digito => digito == telefoneLimpo[0]);

            if(!todosDigitosIguais)
            {
                return;
            }

            TracingService.Trace("Telefone com todos os dígitos iguais: {0} ", telefoneOriginal);

            throw new InvalidPluginExecutionException("Telefone inválido. O telefone não pode conter todos os dígitos iguais.");
        }
        
    }
}
