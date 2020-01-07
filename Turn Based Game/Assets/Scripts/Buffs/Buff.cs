using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Buff 
{
    public Character    character;
    public string       name;
    public uint         turnsDuration = 1u;
    public Buff(Character character) 
    {
        this.character = character;
        StartTurnEffect();
        ApplyStatModifiers();
    }
    public virtual void StartTurnEffect() {}
    public virtual void ApplyStatModifiers() {}

    public void AddModifier( string statName, float modValue, StatModType type)
    {
        Statistic stat = character.GetStat(statName);
        stat.AddModifier(this, modValue, type);
    }
    public bool CheckBuffDuration()
    {
        --turnsDuration;

        if (turnsDuration == 0)
        {
            Destroy();
            return true;
        }

        return false;
    }
    public void Destroy()
    {
        character.Events(new MyEvent(MyEventType.BuffDestroyed, this));
    }
}

public class PoisonBuff : Buff
{
    public PoisonBuff(Character character) : base(character)
    {
        name = "Posion";
        turnsDuration = 3;
    }
    public override void StartTurnEffect() 
    {
        character.ApplyDamage(10f);
    }
}

public class SpeedBuff : Buff
{
    public SpeedBuff(Character character) : base(character)
    {
        name = "+ Speed";
        turnsDuration = 3;
    }
    override public void ApplyStatModifiers() 
    {
        AddModifier("speed", 0.2f, StatModType.addPercent);
    }
}

public class AvoidBuff : Buff
{
    public AvoidBuff(Character character) : base(character)
    {
        name = "+ Avoid";
        turnsDuration = 3;
    }
    override public void ApplyStatModifiers()
    {
        AddModifier("avoid", 0.5f, StatModType.addTotal);
    }
}

public class ArmorBuff : Buff
{
    public ArmorBuff(Character character) : base(character)
    {
        name = "+ Armor";
        turnsDuration = 3;
    }
    override public void ApplyStatModifiers()
    {
        AddModifier("armor", 0.2f, StatModType.addPercent);
    }
}

public class DamageBuff : Buff
{
    public DamageBuff(Character character) : base(character)
    {
        name = "+ Damage";
        turnsDuration = 3;
    }
    override public void ApplyStatModifiers()
    {
        AddModifier("damage", 0.2f, StatModType.addPercent);
    }
}