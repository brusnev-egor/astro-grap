using UnityEngine;

[CreateAssetMenu(menuName = "Runner/Situation")]
public class SituationDefinition : ScriptableObject
{
    [Header("Chunks sequence")]
    public GameObject[] chunks;

    [Header("Spawn weight")]
    public int weight = 1;
}