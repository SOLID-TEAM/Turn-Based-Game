using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum BattleState {startTurn, waitTurn, endTurn, startRound, endRound};
public class Battle 
{
    uint round = 1;
    BattleState battleState = BattleState.startRound;

    public Character characterA;
    public Character characterB;
    Character currentChar;
    List<Character> roundList;

    public Battle()
    {
        roundList = new List<Character>();
    }

    public void OrderRoundBySpeed()
    {
        roundList.Sort(CompareCharacterSpeed);
    }

    public int CompareCharacterSpeed(Character a, Character b)
    {
        Statistic speedA = a.GetStat("speed");
        Statistic speedB = b.GetStat("speed");

        if (speedA.finalValue < speedB.finalValue)
            return -1;
        else if (speedA.finalValue > speedB.finalValue)
            return 1;
        else
            return 0;
    }

    public void UpdateBattle()
    {
        switch (battleState)
        {
            case BattleState.startRound:
                StartRound();
                break;
            case BattleState.endRound:
                EndRound();
                break;
            case BattleState.startTurn:
                StartTurn();
                break;
            case BattleState.waitTurn:
                WaitTurn();
                break;
            case BattleState.endTurn:
                EndTurn();
                break;
            default:
                break;
        }
    }

    public void StartRound()
    {
        Debug.Log("Start Round : " + round.ToString());
        roundList.Clear();
        roundList.Add(characterA);
        roundList.Add(characterB);
        OrderRoundBySpeed(); 

        battleState = BattleState.startTurn;
    }
    public void EndRound()
    {
        Debug.Log("End Round : " + round.ToString());
        ++round;

        battleState = BattleState.startRound;
    }
    public void StartTurn()
    {
        currentChar = roundList[0];

        Debug.Log("Start Turn " + currentChar.name + " : " + round.ToString());
        currentChar.StartTurn();

        battleState = BattleState.waitTurn;
    }
    public void WaitTurn()
    {
        Debug.Log("Wait Turn " + currentChar.name + " : " + round.ToString());

        if (currentChar.WaitTurn())
        {
            battleState = BattleState.endTurn;
        }
    }
    public void EndTurn()
    {
        Debug.Log("End Turn " + currentChar.name + " : " + round.ToString());

        currentChar.EndTurn();
        roundList.Remove(currentChar);

        if (roundList.Count > 0)
        {
            battleState = BattleState.startTurn;
        }
        else
        {
            battleState = BattleState.startRound;
        }
    }
}
