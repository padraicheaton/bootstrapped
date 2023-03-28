using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Bootstrapped/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string displayName;
    public GameObject prefab;
    public Sprite icon;
}
