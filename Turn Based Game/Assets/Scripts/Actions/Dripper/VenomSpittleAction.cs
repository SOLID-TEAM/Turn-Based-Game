using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomSpittleAction : Action
{
    public VenomSpittleAction()
    {
        actionName = "Venom Spittle";
        cooldown = 1;
        finishTurn = false;
    }
    override public void ExecuteAction(Character owner, Character opponent)
    {
        opponent.ApplyDamage(owner.GetStat("damage").finalValue);
        opponent.AddBuff(new PoisonBuff(owner));
    }
}
