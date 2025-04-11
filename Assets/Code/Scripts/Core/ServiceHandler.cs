// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 11.04.25
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System;
using System.Linq;
using UnityEngine;

namespace Core
{
    public interface IService { }

    public class ServiceHandler : MonoBehaviour
    {
        ServiceHandler _instance;
        
        void Awake()
        {
            if (_instance != null) Destroy(this);
            else _instance = this;

            RegisterAllServices();
            DontDestroyOnLoad(this);
        }

        private void RegisterAllServices()
        {
            foreach (var service in GetComponents<IService>())
            {
                Type serviceType = service.GetType();

                // Holen der generischen Register-Methode für den spezifischen Typ
                var registerMethod = typeof(ServiceProvider)
                    .GetMethods() // Hole alle Methoden
                    .FirstOrDefault(m => m.IsGenericMethod && m.Name == "Register");

                if (registerMethod != null)
                {
                    // erstellen einer konkreten methode vom typ service type
                    var genericMethod = registerMethod.MakeGenericMethod(serviceType);

                    // aufruf mit null weil statische methode und ein array von objection, der die argumente der methode darstellt
                    genericMethod.Invoke(null, new object[] { service, gameObject });
                }
            }
        }
    }
}