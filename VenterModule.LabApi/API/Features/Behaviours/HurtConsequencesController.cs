using LabApi.Features.Wrappers;
using UnityEngine;

namespace VenterModuleLabApi.API.Features
{
    public class HurtConsequencesController : MonoBehaviour
    {
        private ReferenceHub hub => GetComponent<ReferenceHub>();
        private Player player => Player.Get(hub);
        
        private float lastBlurredTime { get; set; }
        private float lastHoldingItemTime { get; set; }
        private bool permBlur { get; set; }

        private void Awake()
        {
            lastBlurredTime = 0;
            lastHoldingItemTime = 0;
            permBlur = false;
        }
        
        private void Update()
        {
            if (!player.IsAlive || player.IsSCP) Destroy(this);
            
            if (player.Health > 15 && permBlur)
            {
                hub.playerEffectsController.TryGetEffect("blurred", out var blurredEffect);
                blurredEffect.ServerDisable();
                permBlur = false;
            }
            
            if (player.Health <= 50)
            {
                hub.playerEffectsController.TryGetEffect("slowness", out var slownessEffect);
                hub.playerEffectsController.TryGetEffect("blurred", out var blurredEffect);

                if (player.Health > 30) slownessEffect.ServerSetState(20, Time.deltaTime * 1.5f);
                
                if (player.Health <= 30 && player.Health > 15)
                {
                    slownessEffect.ServerSetState(40, Time.deltaTime * 1.5f);
                    
                    if (Time.time - lastBlurredTime >= 10f && !permBlur)
                    {
                        blurredEffect.ServerSetState(1, 3);
                        lastBlurredTime = Time.time;
                    }
                }
                else if (player.Health <= 15)
                {
                    slownessEffect.ServerSetState(60, Time.deltaTime * 1.5f);

                    if (player.CurrentItem != null && Time.time - lastHoldingItemTime >= 45f)
                    {
                        var pickup = player.DropItem(player.CurrentItem);
                        
                        lastHoldingItemTime = Time.time;
                    }
                    
                    if (!permBlur)
                    {
                        blurredEffect.ServerSetState(1);
                        permBlur = true;
                    }
                }
            }
        }
    }
}