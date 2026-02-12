using System;
using PlayerRoles;
using Talky;
using UnityEngine;

namespace VenterModuleLabApi.API.Features
{
    public class GeneralController : MonoBehaviour
    {
        private ReferenceHub hub => GetComponent<ReferenceHub>();

        private readonly Type[] _behavioursToAdd =
        {
            typeof(PushTracker), typeof(SpeechTracker), typeof(DeathPretendController), typeof(HurtConsequencesController), typeof(ChaosInsurgencyKeycardController)
        };
        
        private MonoBehaviour[] _behaviours { get; set; }
        
        private void OnDestroy()
        {
            for (int i = 0; i < _behavioursToAdd.Length; i++)
            {
                if (hub.gameObject.TryGetComponent(_behavioursToAdd[i], out var component)) 
                    Destroy(component);
            }
        }
        
        private void Awake()
        {
            if (hub.roleManager.CurrentRole.Team == Team.Dead
                || hub.roleManager.CurrentRole.Team == Team.SCPs)
                Destroy(this);

            _behaviours = new MonoBehaviour[_behavioursToAdd.Length];
            
            for (int i = 0; i < _behavioursToAdd.Length; i++)
            {
                if (hub.gameObject.TryGetComponent(_behavioursToAdd[i], out var component)) 
                    Destroy(component);

                _behaviours[i] = (MonoBehaviour) hub.gameObject.AddComponent(_behavioursToAdd[i]);
            }
        }

        public bool TryGetBehaviour(Type type, out MonoBehaviour behaviour)
        {
            for (int i = 0; i < _behaviours.Length; i++)
            {
                if (_behaviours[i].GetType() == type)
                {
                    behaviour = _behaviours[i];
                    return true;
                }
            }

            behaviour = null;
            return false;
        }
    }
}