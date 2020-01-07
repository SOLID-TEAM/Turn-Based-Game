using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dripper : Character
{
    override public void SetDefaultValues()
    {
        characterName = "Dripper";

        SetInitStatValue("life", 100f);
        SetInitStatValue("damage", 35f);
        SetInitStatValue("armor", 5f);
        SetInitStatValue("speed", 10f);
    }
    override public void SetActions()
    {
        AddAction(new VenomSpittleAction());
        AddAction(new ShellAction());
        AddAction(new LeaderAction());
    }
    public override void SetImage()
    {
        image = Resources.Load<Sprite>("Textures/4");
    }
}

