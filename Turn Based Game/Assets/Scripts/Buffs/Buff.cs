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
    }
    public virtual void StartTurnEffect() {}
    public virtual void AvoidActionEffect() {}
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
            return true;
        }

        return false;
    }

    ~Buff()
    {
        character.Events(new MyEvent(MyEventType.BuffDestroyed, this ));
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
        Statistic life = character.GetStat("life");
        life.baseValue -= 100f;
    }

}

public class SpeedBuff : Buff
{
    public SpeedBuff(Character character) : base(character)
    {
        name = "Speed";
        turnsDuration = 3;
    }
}
