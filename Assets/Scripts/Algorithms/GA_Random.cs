using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Random Gen", menuName = "Bootstrapped/Gen Obj/Random")]
public class GA_Random : BaseGenerationAlgorithm
{
    public override void Initialize()
    {

    }

    public override int[] GetNextWeapon(EvolutionaryData[] evolutionaryData)
    {
        return EvolutionAlgorithms.Randomised();
    }
}
