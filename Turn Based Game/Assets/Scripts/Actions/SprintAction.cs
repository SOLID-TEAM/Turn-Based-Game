using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintAction : Action
{
    public SprintAction()
    {
        name = "Sprint";
        cooldown = 4;
        finishTurn = false;
    }
}
