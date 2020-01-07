using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum BattleState {startBattle , waitBattle, endBattle, startTurn, waitTurn, endTurn, startRound, endRound};
public class Battle 
{
    uint round = 1;
    BattleState battleState = BattleState.waitBattle;

    public Character characterA;
    public Character characterB;
    public Character currentChar;
    public Character winnerChar;

    List<Character> roundList;

    public Battle()
    {
        roundList = new List<Character>();
    }
    public void UpdateBattle()
    {
        CheckWinner();

        switch (battleState)
        {
            case BattleState.startBattle:
                break;
            case BattleState.waitBattle:
                break;
            case BattleState.endBattle:
                EndBattle();
                break;
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

    public Character GetOpponent()
    {
        return (currentChar == characterA) ? characterB : characterA;
    }

    // Win/Lose Conditions ----------------------
    public void CheckWinner()
    {
        Character winner = null;

        if (characterA != null && characterB != null)
        {
            if (characterA.GetStat("life").baseValue == 0)
            {
                winner =  characterA;
            }
            else if (characterB.GetStat("life").baseValue == 0)
            {
                winner =  characterB;
            }
        }

        if (winner != null)
        {
            winnerChar = winner;
            battleState = BattleState.endBattle;
        }
    }

    // Battle Functions -------------------------
    public void StartBattle( Character a, Character b)
    {
        characterA = a;
        characterB = b;

        if (characterA != null && characterB != null)
        {
            battleState = BattleState.startRound;
        }
    }
    public void EndBattle()
    {
        round = 1;
        battleState = BattleState.waitBattle;
        Object.FindObjectOfType<GameManager>().Events(new MyEvent(MyEventType.BattleFinished, null));
    }

    // Turn Functions ---------------------------
    public void StartTurn()
    {
        currentChar = roundList[0];

        Debug.Log("Start Turn " + currentChar.characterName);
        currentChar.StartTurn();

        battleState = BattleState.waitTurn;
    }
    public void WaitTurn()
    {
        if (currentChar.WaitTurn(GetOpponent())) 
        {
            battleState = BattleState.endTurn;
        }
    }
    public void EndTurn()
    {
        Debug.Log("End Turn " + currentChar.characterName);

        currentChar.EndTurn();
        roundList.Remove(currentChar);

        if (roundList.Count == 0)
        {
            battleState = BattleState.endRound;
        }
        else
        {
            battleState = BattleState.startTurn;
        }
    }

    // Round Functions -------------------------
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

    // Round order ----------------------------
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
}
