using UnityEngine;
using System.Linq;

public class SituationDirector : MonoBehaviour
{
    [Header("Situation pools")]
    [SerializeField] private SituationDefinition[] easySituations;
    [SerializeField] private SituationDefinition[] mediumSituations;
    [SerializeField] private SituationDefinition[] hardSituations;

    public SituationDefinition GetNextSituation()
    {
        DifficultyLevel difficulty = DifficultyManager.Instance.CurrentDifficulty;

        SituationDefinition[] pool = difficulty switch
        {
            DifficultyLevel.Medium => mediumSituations,
            DifficultyLevel.Hard => hardSituations,
            DifficultyLevel.Easy => easySituations,
            _ => easySituations
        };

        return PickWeighted(pool);
    }

    SituationDefinition PickWeighted(SituationDefinition[] defs)
    {
        int totalWeight = defs.Sum(d => d.weight);

        int roll = Random.Range(0, totalWeight);

        foreach (var d in defs)
        {
            if (roll < d.weight)
                return d;

            roll -= d.weight;
        }

        return defs[0];
    }
}