using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Voice Line", menuName = "Bootstrapped/Voice Line")]
public class VoiceLine : ScriptableObject
{
    public string text;
    public AudioClip clip;
}
