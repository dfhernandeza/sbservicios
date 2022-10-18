using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Compute;
using Azure.ResourceManager.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Logica.Servicios
{
   public class ProcesadorVm
    {
        public static async Task<bool> vmOnAsync()
        {
            // First we construct our armClient
            var vsc = new VisualStudioCredential();
            var armClient = new ArmClient(vsc);
            
            // Next we get a resource group object
            // ResourceGroup is a [Resource] object from above
            ResourceGroup resourceGroup = await armClient.DefaultSubscription.GetResourceGroups().GetAsync("SANTA");

            // Next we get the container for the virtual machines
            // vmContainer is a [Resource]Container object from above
            VirtualMachineContainer vmContainer = resourceGroup.GetVirtualMachines();
            foreach (var vm in vmContainer.GetAll())
            {
                if (vm.Data.Name == "SantaBeatriz")
                {
                    var on = vm.PowerOn();
                    if (on.HasCompleted)
                    {
                        return true;
                    }
                    else
                    {
                       return false;
                    }
                }
               
            }
            return false;
        }
        public static async Task vmOffAsync()
        {
            var vsc = new VisualStudioCredential();
            var armClient = new ArmClient(vsc);

            // Next we get a resource group object
            // ResourceGroup is a [Resource] object from above
            ResourceGroup resourceGroup = await armClient.DefaultSubscription.GetResourceGroups().GetAsync("SANTA");

            // Next we get the container for the virtual machines
            // vmContainer is a [Resource]Container object from above
            VirtualMachineContainer vmContainer = resourceGroup.GetVirtualMachines();
            foreach (var vm in vmContainer.GetAll())
            {
                if (vm.Data.Name == "SantaBeatriz")
                {
                    vm.PowerOff();
                }

            }
        }
    }
}
