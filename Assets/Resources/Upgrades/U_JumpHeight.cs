using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jump Height", menuName = "Bootstrapped/Upgrades/Jump Height")]
public class U_JumpHeight : PlayerModifierUpgrade
{
    public float additiveJumpHeightBuff;

    public override void ApplyModifierToPlayer(GameObject player)
    {
        player.GetComponent<PlayerMovement>().jumpForce += additiveJumpHeightBuff;
    }
}
