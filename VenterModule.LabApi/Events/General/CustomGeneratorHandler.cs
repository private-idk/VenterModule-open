using LabApi.Events.Arguments.ObjectiveEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Console;
using PlayerRoles.PlayableScps.Scp079;
using VenterModuleLabApi.Patches;

namespace VenterModuleLabApi.Events.ServerEvents
{
    public class CustomGeneratorHandler : CustomEventsHandler
    {
        public override void OnObjectiveActivatedGeneratorCompleted(GeneratorActivatedObjectiveEventArgs ev)
        {
            if (_isAllGeneratorsEngaged())
            {
                Scp079RecontainerPatch.RecontainerInstance.Recontain();
            }
            base.OnObjectiveActivatedGeneratorCompleted(ev);
        }

        private bool _isAllGeneratorsEngaged()
        {
            foreach (var generator in Scp079Recontainer.AllGenerators) 
                if (!generator.Engaged) return false;

            return true;
        }
    }
}