﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversor : Character
{
    override public void SetDefaultValues() 
    {
        characterName = "Conversor";

        SetInitStatValue("life",   100f);
        SetInitStatValue("damage", 100f);
        SetInitStatValue("armor",  100f);
        SetInitStatValue("speed",  100f);
    }
    override public void SetActions() 
    {
        AddAction(new PistolShotAction());
        AddAction(new SprintAction());
        AddAction(new AvoidAction());
        AddAction(new BloodBagAction());
    }
}
