using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dripper : Character
{
    override public void SetBaseStats()
    {
        SetBaseStat("life", 100f);
        SetBaseStat("damage", 100f);
        SetBaseStat("armor", 100f);
        SetBaseStat("speed", 100f);
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

