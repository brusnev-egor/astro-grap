using UnityEngine;
using System.Linq;

public class SituationDirector : MonoBehaviour
{
    [Header("Situation pools")]
    [SerializeField] private SituationDefinition tutorialSituation;
    [SerializeField] private SituationDefinition[] easySituations;
    [SerializeField] private SituationDefinition[] mediumSituations;
    [SerializeField] private SituationDefinition[] hardSituations;

    [Header("Debug")]
    [SerializeField] private bool isDebug = false;
    [SerializeField] private SituationDefinition _testSituation;

    private int situationsCount;
    private DifficultyLevel currentDifficulty;

    void Start()
    {
        situationsCount = 0;
    }

    public SituationDefinition GetNextSituation()
    {
        situationsCount++;

        if (isDebug)
        {
            return _testSituation;
        }

        if (situationsCount == 1)
            {
                // return tutorialSituation;
            }
        DifficultyLevel difficulty = DifficultyManager.Instance.CurrentDifficulty;
        if (currentDifficulty != difficulty)
        {
            currentDifficulty = difficulty;
            if (difficulty == DifficultyLevel.Medium)
            {
                // Medium transition
            }
            if (difficulty == DifficultyLevel.Hard)
            {
                // Hard transition
            }
        }

        SituationDefinition[] pool = difficulty switch
        {
            DifficultyLevel.Medium => mediumSituations,
            DifficultyLevel.Hard => hardSituations,
            DifficultyLevel.Easy => easySituations,
            _ => easySituations
        };

        return PickWeighted(pool);
        // return PickWeighted(hardSituations);
    }

    SituationDefinition PickWeighted(SituationDefinition[] defs)
    {
        float totalWeight = defs.Sum(d => d.weight);

        float roll = Random.Range(0, totalWeight);

        if (totalWeight <= 0f)
            return defs[Random.Range(0, defs.Length)];

        foreach (var d in defs)
        {
            if (roll < d.weight)
                return d;

            roll -= d.weight;
        }

        return defs[0];
    }
}