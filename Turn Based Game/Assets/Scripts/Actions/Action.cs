using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action 
{
    public string actionName = "";

    public int cooldown = 1;
    public int currentCooldown = 0;

    public bool finishTurn = false;
    public bool active = true;
    virtual public void ExecuteAction(Character owner ,Character opponent) {}
    public void Execute(Character owner ,Character opponent)
    {
        active = false;
        currentCooldown = cooldown;
        ExecuteAction(owner, opponent);
    }
    public void UpdateCooldown()
    {
        if ( active == false)
        {
            --currentCooldown;

            if (currentCooldown == 0)
            {
                active = true;
            }

        }
    }
    public void Reset()
    {
        active = true;
        currentCooldown = 0;
    }

}
