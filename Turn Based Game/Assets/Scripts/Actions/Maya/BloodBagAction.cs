using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBagAction : Action
{
   public BloodBagAction()
    {
        actionName = "Blood Bag";
        cooldown = 4;
        finishTurn = true;
    }
    override public void ExecuteAction(Character owner, Character opponent) 
    {
        owner.AddLife(10f);
    }
}
