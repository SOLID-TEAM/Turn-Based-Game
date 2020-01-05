using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidAction : Action
{
    public AvoidAction()
    {
        name = "Avoid";
        cooldown = 4;
        finishTurn = false;
    }
}
