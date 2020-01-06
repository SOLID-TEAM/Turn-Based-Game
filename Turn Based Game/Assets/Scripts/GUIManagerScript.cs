using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GUIManagerScript : MonoBehaviour
{
    public enum gameState { PLAY, STOP};

    public GameObject player1;
    public GameObject player2;
    public GameObject PlaySimulate;
    public Text combatLogText;
    public ScrollRect scrollRect;
    private gameState GS;

    //private Button[] p1Buttons;
    //private Button[] p2Buttons;
    //private Button[] gameButtons;
    private Dictionary<string, Button> p1Buttons;
    private Dictionary<string, Button> p2Buttons;
    private Dictionary<string, Button> gameButtons;

    private string[] skillsToDisable;
    private string[] buttonsToDisable;

    // TODO: TESTING - REMOVE
    public GameManager gameMan;

    // Start is called before the first frame update
    void Start()
    {
        skillsToDisable = new string[] {   "Skill1Button", "Skill2Button", "Skill3Button", "Skill4Button"};
        buttonsToDisable = new string[] { "ButtonPlusHp", "ButtonMinHp","ButtonPlusAtk", "ButtonMinAtk",
                                            "ButtonPlusDef", "ButtonMinDef", "ButtonPlusSpeed", "ButtonMinSpeed", 
                                             "prevCharacter", "nextCharacter"};

        p1Buttons = new Dictionary<string, Button>();
        p2Buttons = new Dictionary<string, Button>();
        gameButtons = new Dictionary<string, Button>();
        // get all buttons from player1 ----------------------------------
        // player1 is capable to interact with all buttons
        Button[] tempButton = new Button[0];
        if (player1)
            tempButton = player1.GetComponentsInChildren<Button>();

        // TODO: get skill lenght from player and disable the buttons without skill
        foreach (Button button in tempButton)
        {
            button.onClick.AddListener(() => OnButtonClick(button.name, 0));
            p1Buttons.Add(button.name, button);
        }
        // ----------------------------------------------------------------

        // get all buttons from player2(NO CONTROLLABLE) ----------------------------------
        // player2 is NOT capable to interact with all buttons, actions doesn't do anything
        if (player2)
            tempButton = player2.GetComponentsInChildren<Button>();

        // TODO: get skill lenght from player and disable the buttons without skill
        foreach (Button button in tempButton)
        {
            button.onClick.AddListener(() => OnButtonClick(button.name, 1));
            p2Buttons.Add(button.name, button);
        }
        

        // Get general buttons ---------------------------------------
        if (PlaySimulate)
            tempButton = PlaySimulate.GetComponentsInChildren<Button>();

        foreach (Button button in tempButton)
        {
            button.onClick.AddListener(() => OnButtonClick(button.name, -1));
            gameButtons.Add(button.name, button);
        }
        // ------------------------------------------------------------

        if (combatLogText)
            combatLogText.text = "";

        if (scrollRect)
            scrollRect = scrollRect.GetComponent<ScrollRect>();

        // GAME STATE
        GS = gameState.STOP;
        StopToPlay();

    }

    // Update is called once per frame
    void Update()
    {
        // TESTING - REMOVE ------------------------
        if (Input.GetKeyDown(KeyCode.Return))
        {
            for (int i = 0; i < 10; ++i)
            {
                
                Battle battle = gameMan.GetBattleInfo();
                int numSimulations = 1;
                int numWins = 0;

                CSVManager.AppendToReport(battle.characterA, battle.characterB, numSimulations, numWins);
            }
        }
        // ------------------------------------------
    }

    void OnButtonClick(string butName, int playerIndex)
    {
        Debug.Log("player: " + playerIndex + ", " + butName);

        // TESTING
        Action action = new Action();
        action.name = butName;
        AddCombatLogEntry(playerIndex, playerIndex == 0 ? 1 : 0, action);

        switch (butName)
        {
            case "Skill1Button":
                {
                    //Debug.Log("blabla");
                    break;
                }
            case "PlayButton":
                {
                    StartToPlay();
                   
                    break;
                }
            case "StopButton":
                {
                    StopToPlay();
                    break;
                }
        }

    }

    void StartToPlay() // disable input for desired buttons
    {
        GS = gameState.PLAY;
        gameButtons["StopButton"].gameObject.SetActive(true);
        gameButtons["PlayButton"].gameObject.SetActive(false);
        gameButtons["SimulateButton"].interactable = false;

        // disable all base stats buttons and change character
        // enable all skill buttons
        // TODO: for(playerActions.count)
        foreach(string str in buttonsToDisable)
        {
            p1Buttons[str].interactable = false;
            p2Buttons[str].interactable = false;
        }
        foreach (string str in skillsToDisable)
        {
            p1Buttons[str].interactable = true;
            //p2Buttons[str].interactable = true;
        }

    }

    void StopToPlay()
    {
        GS = gameState.STOP;
        gameButtons["StopButton"].gameObject.SetActive(false);
        gameButtons["PlayButton"].gameObject.SetActive(true);
        gameButtons["SimulateButton"].interactable = true;

        // disable all skill buttons
        // TODO: for(playerActions.count)
        foreach (string str in buttonsToDisable)
        {
            p1Buttons[str].interactable = true;
            p2Buttons[str].interactable = true;
        }
        foreach (string str in skillsToDisable)
        {
            p1Buttons[str].interactable = false;
            p2Buttons[str].interactable = false;
        }

        combatLogText.text = "";

    }

    void AddCombatLogEntry(int fromPlayer, int toPlayer, Action action)
    {
        combatLogText.text += "Player " + fromPlayer + " used " + action.name + " to " + toPlayer + "\n";
        
        Canvas.ForceUpdateCanvases();
        combatLogText.GetComponent<ContentSizeFitter>().SetLayoutVertical();

        if (scrollRect)
        {
            scrollRect.SetLayoutVertical();
            scrollRect.verticalNormalizedPosition = 0.0f;
        }
        
    }
}
