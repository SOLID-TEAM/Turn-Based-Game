using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action 
{
    public string name = "";
    public uint cooldown = 1;
    public bool finishTurn = false;

    public void Execute( Character character) 
    {
        ExecuteAction(character);
    }

    virtual public void ExecuteAction(Character character) {}

}
