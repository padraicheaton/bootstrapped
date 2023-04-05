using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveSpeed", menuName = "Bootstrapped/Upgrades/Move Speed")]
public class U_MoveSpeed : PlayerModifierUpgrade
{
    public float maxStacksMovementSpeed;

    public override void ApplyModifierToPlayer(GameObject player)
    {
        player.GetComponent<PlayerMovement>().maxSpeed += maxStacksMovementSpeed;
    }
}
