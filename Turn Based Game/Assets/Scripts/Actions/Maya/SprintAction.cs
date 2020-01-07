using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintAction : Action
{
    public SprintAction()
    {
        actionName = "Sprint";
        cooldown = 4;
        finishTurn = false;
    }
    override public void ExecuteAction(Character owner, Character opponent)
    {
        owner.AddBuff(new SpeedBuff(owner));
    }
}
