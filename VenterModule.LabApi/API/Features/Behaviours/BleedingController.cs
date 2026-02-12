using System.Collections.Generic;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles.FirstPersonControl;
using PlayerStatsSystem;
using UnityEngine;

namespace VenterModuleLabApi.API.Features.Behaviours
{
    public class BleedingController : MonoBehaviour
    {
        public byte intensity { get; set; }
        public bool isBleeding { get; set; }
    
        private ReferenceHub hub => GetComponent<ReferenceHub>();
        private Player player => Player.Get(hub);
        int lifeId => player.LifeId;
        IFpcRole fpcRole => player.RoleBase as IFpcRole;
    
        private void Awake()
        {
            isBleeding = false;
        }

        public void ChangeBleeding(int time, bool newCondition)
        {
            if (newCondition == isBleeding) return;
        
            if (!newCondition) isBleeding = false;
            else
            {
                isBleeding = true;
                Timing.RunCoroutine(ProcessBleeding(time));
            }
        }

        private IEnumerator<float> ProcessBleeding(int time)
        {
            intensity = 1;
            float magnitudePerTime = 0;
            
            for (int i = 0; i < time; i++)
            {
                yield return Timing.WaitForSeconds(1);

                if (lifeId != player.LifeId || !isBleeding)
                    yield break;

                if (intensity > 3) intensity = 3;
            
                if (i % 3 == 0 || i == 0)
                {
                    if (magnitudePerTime >= 3f) DealBleedingDamage(magnitudePerTime * intensity);
                    else DealBleedingDamage(1);
                
                    magnitudePerTime = 0;
                }
                
                magnitudePerTime += fpcRole.FpcModule.CharController.velocity.magnitude;
            }
            
            isBleeding = false;
        }
    
        private void DealBleedingDamage(float amount) => player.Damage(new UniversalDamageHandler(amount, DeathTranslations.Bleeding));
    }
}