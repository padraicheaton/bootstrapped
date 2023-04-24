using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GenConfig", menuName = "Bootstrapped/Gen Config")]
public class GenConfig : ScriptableObject
{
    [Header("Settings")]
    [Range(0f, 1f)] public float noveltyChance;
    public bool shouldIncrementNovelty;
    public float noveltyChanceIncrement;
    [Range(0f, 1f)] public float mutationChance;
}
