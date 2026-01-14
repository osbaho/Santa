using System.Collections.Generic;
using Santa.Core;
using UnityEngine;
using VContainer;

namespace Santa.Domain.Combat
{
    [CreateAssetMenu(fileName = "New Gain AP Ability", menuName = "Santa/Abilities/Gain AP Ability")]
    public class GainAPAbility : Ability
    {
        public override void Execute(List<GameObject> targets, GameObject caster, IUpgradeService upgradeService, ICombatLogService combatLogService, IReadOnlyList<GameObject> allCombatants)
        {
            ICombatLogService _combatLog = combatLogService; // Local variable to minimize code changes
                                                             // Get energy gained from UpgradeService
            int amountToGain = upgradeService?.APRecoveryAmount ?? 34;

            // Use for loop instead of foreach for mobile performance
            for (int i = 0; i < targets.Count; i++)
            {
                GameObject target = targets[i];
                if (target == null) continue;

                if (target.TryGetComponent<ActionPointComponentBehaviour>(out var apComponent))
                {
                    apComponent.AffectValue(amountToGain);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
                    GameLog.Log($"{target.name} gained {amountToGain} AP.");
#endif
                    _combatLog?.LogMessage($"{target.name} gained {amountToGain} AP.", CombatLogType.ActionPoints);
                }
            }
        }
    }
}
