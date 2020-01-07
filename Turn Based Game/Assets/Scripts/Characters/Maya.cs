using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maya : Character
{
    override public void SetBaseStats()
    {
        SetBaseStat("life", 7f);
        SetBaseStat("damage", 3f);
        SetBaseStat("armor", 2f);
        SetBaseStat("speed", 4f);
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

