using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Effect", menuName = "Bootstrapped/Effect Data")]
public class EffectData : ScriptableObject
{
    public string displayName;
    public GameObject prefab;
    public Sprite icon;
    public GameObject particlePrefab;
}
