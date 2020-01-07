using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maya : Character
{
    override public void SetDefaultValues()
    {
        characterName = "Maya";

        SetInitStatValue("life", 7f);
        SetInitStatValue("damage", 3f);
        SetInitStatValue("armor", 2f);
        SetInitStatValue("speed", 4f);
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
        image = Resources.Load<Sprite>("Textures/1");
    }
}

