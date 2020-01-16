using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {StopSimulation, WaitSimulation, StartSimulation, EndSimulation, WaitResetSimulation ,ResetSimulation}
public enum CharacterType { Maya = 0, Dripper = 1, Conversor = 2, Max = 3}
public enum MyEventType { BattleFinished, BuffDestroyed, CharDestroyed, Unknown}
public class MyEvent
{
    public MyEventType type = MyEventType.Unknown;
    public object myObject;
    public MyEvent(MyEventType type, object myObject )
    {
        this.type = type;
        this.myObject = myObject;
    }
}

public class GameManager : MonoBehaviour
{
    [HideInInspector] public GameState gameState = GameState.StopSimulation;
    [HideInInspector] public CharacterType selectedCharA;
    [HideInInspector] public CharacterType selectedCharB;

    private List<Character> charSelectionA;
    private List<Character> charSelectionB;

    private GameObject mayaPrefab;
    private GameObject dripperPrefab;
    private GameObject conversorPrefab;

    private Battle battle;

    [HideInInspector] public bool controlCharacterA = false;
    [HideInInspector] public int  onlyUseAction = -1;

    [HideInInspector] public int  currentSimulation;
    [HideInInspector] public int  numSimulations = 10;
    [HideInInspector] public int  winBattles = 0;
    [HideInInspector] public int  loseBattles = 0;    


    private void Awake()
    {
        battle = new Battle();

        charSelectionA = new List<Character>();
        charSelectionB = new List<Character>();

        mayaPrefab      = Resources.Load<GameObject>("Prefabs/Characters/Maya");
        dripperPrefab   = Resources.Load<GameObject>("Prefabs/Characters/Dripper");
        conversorPrefab = Resources.Load<GameObject>("Prefabs/Characters/Conversor");

        // Selection A --------------------------------

        charSelectionA.Add(Instantiate(mayaPrefab.GetComponent<Character>()));
        charSelectionA.Add(Instantiate(dripperPrefab.GetComponent<Character>()));
        charSelectionA.Add(Instantiate(conversorPrefab.GetComponent<Character>()));

        // Selection B --------------------------------

        charSelectionB.Add(Instantiate(mayaPrefab.GetComponent<Character>()));
        charSelectionB.Add(Instantiate(dripperPrefab.GetComponent<Character>()));
        charSelectionB.Add(Instantiate(conversorPrefab.GetComponent<Character>()));

        selectedCharA = CharacterType.Maya;
        selectedCharB = CharacterType.Conversor;
    }
    void Start()
    {
        numSimulations = 6;
        winBattles = 0;
        loseBattles = 0;
    }
    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        { 
            case GameState.StartSimulation:
                {
                    currentSimulation = numSimulations;
                    battle.StartBattle(GetSelectedCharA(), GetSelectedCharB());
                    gameState = GameState.WaitSimulation;
                    break;
                }
            case GameState.WaitSimulation:
                {
                    battle.UpdateBattle();
                    break;
                }
            case GameState.EndSimulation:
                {
                    CSVManager.AppendToReport(battle.characterA, battle.characterB, numSimulations, winBattles);
                    gameState = GameState.WaitResetSimulation;
                    break;
                }
            case GameState.WaitResetSimulation:
                {
                    // TODO: Button to reset simulation
                    gameState = GameState.ResetSimulation;
                    break;
                }
            case GameState.ResetSimulation:
                {
                    ResetSimulation();
                    break;
                }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    public void StartSimulation( int simulations, bool controlCharacterA  = false, int onlyUseAction = -1)
    {
        if (gameState == GameState.StopSimulation)
        {
            ResetSimulation();
            numSimulations = simulations;
            gameState = GameState.StartSimulation;

            this.controlCharacterA = controlCharacterA;
            this.onlyUseAction = onlyUseAction;
        }
    }

    public void ResetSimulation()
    {
        onlyUseAction = -1;
        controlCharacterA = false;
        currentSimulation = numSimulations;
        battle.ResetBattle();
        winBattles = 0;
        loseBattles = 0;

        gameState = GameState.StopSimulation;
    }

    public void Events(MyEvent myEvent)
    { 
        switch (myEvent.type)
        {
            case MyEventType.BattleFinished:
                {
                    if (battle.winnerChar == GetSelectedCharA())
                    {
                        ++winBattles;
                    }
                    else
                    {
                        ++loseBattles;
                    }

                    if(--currentSimulation == 0)
                    {
                        gameState = GameState.EndSimulation;
                    }
                    else
                    {
                        battle.ResetBattle();
                        battle.StartBattle(GetSelectedCharA(), GetSelectedCharB());
                    }

                    break;
                }
        }
    }

    // Get Selected ----------------------------------
    public Character GetSelectedCharA()
    {
        return charSelectionA[(int)selectedCharA];
    }

    public Character GetSelectedCharB()
    {
        return charSelectionB[(int)selectedCharB];
    }

    // TESTING
    public Battle GetBattleInfo()
    {
        return battle;
    }
}
