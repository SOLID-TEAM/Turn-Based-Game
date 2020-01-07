using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversor : Character
{
    override public void SetDefaultValues() 
    {
        characterName = "Conversor";

        SetInitStatValue("life",   100f);
        SetInitStatValue("damage", 8f);
        SetInitStatValue("armor",  10f);
        SetInitStatValue("speed",  10f);
    }
    override public void SetActions() 
    {
        AddAction(new PistolShotAction());
        AddAction(new SprintAction());
        AddAction(new AvoidAction());
        AddAction(new BloodBagAction());
    }
    public override void SetImage()
    {
        image = Resources.Load<Sprite>("Textures/7");
    }
}
