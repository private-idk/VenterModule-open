using System;
using UnityEngine;
using VenterModuleExiled.Subroles.Enums;

namespace VenterModuleExiled.Subroles
{
    public class AdminGunController : MonoBehaviour
    {
        public string CustomTaskForce { get; set; }
        public ServiceEnum Service { get; set; }
        public int Subrole { get; set; }

        private void Awake()
        {
            CustomTaskForce = String.Empty;
            Service = ServiceEnum.Scientist;
            Subrole = 0;
        }
    }
}