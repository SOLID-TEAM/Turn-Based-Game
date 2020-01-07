using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversor : Character
{
    override public void SetDefaultValues() 
    {
        characterName = "Conversor";

        SetInitStatValue("life",   100f);
        SetInitStatValue("damage", 18f);
        SetInitStatValue("armor",  1f);
        SetInitStatValue("speed",  10f);
    }
    override public void SetActions() 
    {
        AddAction(new SharpClawAction());
        AddAction(new SprintAction());
    }
    public override void SetImage()
    {
        image = Resources.Load<Sprite>("Textures/7");
    }
}
