using System.Collections.Generic;
using UnityEngine;

public static class PlayerProgression
{
    public static List<Ability> UnlockedAbilities { get; private set; } = new List<Ability>();

    public static void InitializeAbilities()
    {
        UnlockedAbilities.Clear();
        UnlockedAbilities.Add(new Ability("Punch", 10, 5)); // 5 mana cost
        UnlockedAbilities.Add(new Ability("Kick", 15, 10)); // 10 mana cost
    }

    public static void UnlockAbility(Ability ability)
    {
        if (!UnlockedAbilities.Contains(ability))
        {
            UnlockedAbilities.Add(ability);
            Debug.Log($"Unlocked new ability: {ability.name}");
        }
    }
}
