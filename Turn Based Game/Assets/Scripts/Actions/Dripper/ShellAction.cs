using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellAction : Action
{
    public ShellAction()
    {
        actionName = "Shell";
        cooldown = 4;
        finishTurn = false;
    }
    override public void ExecuteAction(Character owner, Character opponent)
    {
        owner.AddBuff(new ArmorBuff(owner));
    }
}
