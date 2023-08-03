using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseGenerationAlgorithm : ScriptableObject
{
    // This base class will be inherited by any algorithm implemented, and will contain any base features

    public abstract void Initialize();

    public abstract int[] GetNextWeapon(EvolutionaryData[] evolutionaryData);
}
