using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidAction : Action
{
    public AvoidAction()
    {
        actionName = "Avoid";
        cooldown = 4;
        finishTurn = false;
    }
    override public void ExecuteAction(Character owner, Character opponent)
    {

    }
}
