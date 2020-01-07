using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShotAction : Action
{
    public PistolShotAction()
    {
        actionName = "Pistol Shot";
        cooldown = 1;
        finishTurn = true;
    }
    override public void ExecuteAction(Character owner, Character opponent)
    {
        opponent.ApplyDamage(owner.GetStat("damage").finalValue);
    }
}
