using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dripper : Character
{
    override public void SetDefaultValues()
    {
        characterName = "Dripper";

        SetInitStatValue("life", 100f);
        SetInitStatValue("damage", 100f);
        SetInitStatValue("armor", 100f);
        SetInitStatValue("speed", 100f);
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
        image = Resources.Load<Sprite>("Textures/4");
    }
}

