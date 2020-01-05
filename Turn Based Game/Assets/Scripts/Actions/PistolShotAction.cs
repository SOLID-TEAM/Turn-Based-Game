using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShotAction : Action
{
    public PistolShotAction()
    {
        name = "Pistol Shot";
        cooldown = 1;
        finishTurn = true;
    }
}
