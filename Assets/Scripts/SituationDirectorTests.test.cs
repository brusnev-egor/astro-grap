using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SituationDirectorTests
{
    class TestSituation : SituationDefinition
    {
        public TestSituation(float w)
        {
            weight = w;
        }
    }

    SituationDefinition PickWeighted(SituationDefinition[] defs)
    {
        float totalWeight = 0f;
        foreach (var d in defs)
            totalWeight += d.weight;

        float roll = Random.value * totalWeight;

        foreach (var d in defs)
        {
            if (roll < d.weight)
                return d;

            roll -= d.weight;
        }

        return defs[0];
    }

    [Test]
    public void PickWeighted_DistributionTest()
    {
        // Arrange
        var a = ScriptableObject.CreateInstance<SituationDefinition>();
        var b = ScriptableObject.CreateInstance<SituationDefinition>();
        var c = ScriptableObject.CreateInstance<SituationDefinition>();

        a.weight = 0.1f;
        b.weight = 0.3f;
        c.weight = 0.6f;

        var defs = new[] { a, b, c };

        int iterations = 100000;

        Dictionary<SituationDefinition, int> results = new()
        {
            { a, 0 },
            { b, 0 },
            { c, 0 }
        };

        // Act
        for (int i = 0; i < iterations; i++)
        {
            var picked = PickWeighted(defs);
            results[picked]++;
        }

        float aRatio = results[a] / (float)iterations;
        float bRatio = results[b] / (float)iterations;
        float cRatio = results[c] / (float)iterations;

        Debug.Log($"A: {aRatio}, B: {bRatio}, C: {cRatio}");

        // Assert (с допуском)
        Assert.That(aRatio, Is.InRange(0.08f, 0.12f));
        Assert.That(bRatio, Is.InRange(0.27f, 0.33f));
        Assert.That(cRatio, Is.InRange(0.57f, 0.63f));
    }

    [Test]
    public void PickWeighted_NonNormalizedWeights_DistributionTest()
    {
        // Arrange
        var a = ScriptableObject.CreateInstance<SituationDefinition>();
        var b = ScriptableObject.CreateInstance<SituationDefinition>();
        var c = ScriptableObject.CreateInstance<SituationDefinition>();

        a.weight = 0.6f;
        b.weight = 0.2f;
        c.weight = 0.4f;

        var defs = new[] { a, b, c };

        int iterations = 100000;

        var results = new Dictionary<SituationDefinition, int>
        {
            { a, 0 },
            { b, 0 },
            { c, 0 }
        };

        // Act
        for (int i = 0; i < iterations; i++)
        {
            var picked = PickWeighted(defs);
            results[picked]++;
        }

        float aRatio = results[a] / (float)iterations;
        float bRatio = results[b] / (float)iterations;
        float cRatio = results[c] / (float)iterations;

        Debug.Log($"NonNormalized → A: {aRatio}, B: {bRatio}, C: {cRatio}");

        // Assert (с допуском)
        Assert.That(aRatio, Is.InRange(0.47f, 0.53f));   // ~0.5
        Assert.That(bRatio, Is.InRange(0.14f, 0.19f));   // ~0.166
        Assert.That(cRatio, Is.InRange(0.30f, 0.36f));   // ~0.333
    }
}