using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderAction : Action
{
    public LeaderAction()
    {
        actionName = "Leader";
        cooldown = 3;
        finishTurn = false;
    }
    override public void ExecuteAction(Character owner, Character opponent)
    {
        owner.AddBuff(new DamageBuff(owner));
    }
}
