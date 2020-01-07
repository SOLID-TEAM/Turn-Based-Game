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
    private string[] actionButtons;

    // TODO: TESTING - REMOVE - ADAPT
    public GameManager gameMan;
    private Character characterA;
    private Character characterB;

    // Start is called before the first frame update
    void Start()
    {
        skillsToDisable = new string[] {   "Skill1Button", "Skill2Button", "Skill3Button", "Skill4Button"};
        buttonsToDisable = new string[] { "ButtonPlusHp", "ButtonMinHp","ButtonPlusAtk", "ButtonMinAtk",
                                            "ButtonPlusDef", "ButtonMinDef", "ButtonPlusSpeed", "ButtonMinSpeed", 
                                             "prevCharacter", "nextCharacter", "ButtonPlusLvl", "ButtonMinLvl"};

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

        // load characters info
        ReloadCharactersInfo();

    }
    
    void ReloadCharactersInfo()
    {
        characterA = gameMan.GetSelectedCharA();
        characterB = gameMan.GetSelectedCharB();

        UpdateButtons(p1Buttons, characterA);
        UpdateButtons(p2Buttons, characterB);
    }

    void UpdateButtons(Dictionary<string, Button> buttons, Character character)
    {
        buttons["prevCharacter"].GetComponentInParent<Text>().text = character.characterName;
        // image
        GameObject test = player1.transform.Find("ImageBg").gameObject;
        Image[] img = test.GetComponentsInChildren<Image>();
        img[img.Length - 1].sprite = character.image;

        // stats , get text from parent of any of the plus or min button
        buttons["ButtonPlusHp"].GetComponentInParent<Text>().text = "HP: " + character.GetStat("life").baseValue.ToString() + " Total: " + character.GetStat("life").finalValue.ToString();
        buttons["ButtonPlusAtk"].GetComponentInParent<Text>().text = "ATK: " + character.GetStat("damage").baseValue.ToString() + " Total: " + character.GetStat("damage").finalValue.ToString();
        buttons["ButtonPlusDef"].GetComponentInParent<Text>().text = "DEF: " + character.GetStat("armor").baseValue.ToString() + " Total: " + character.GetStat("armor").finalValue.ToString();
        buttons["ButtonPlusSpeed"].GetComponentInParent<Text>().text = "SPEED: " + character.GetStat("speed").baseValue.ToString() + " Total: " + character.GetStat("speed").finalValue.ToString();
        // skills -----------------------

        int maxSkills = 4;
        for (int x = maxSkills; x > 0; x--)
        {
            buttons[skillsToDisable[x - 1]].interactable = false;
            buttons[skillsToDisable[x - 1]].gameObject.SetActive(false);
        }

        int i = 0;

        if (gameMan.GetBattleInfo().currentChar ==  character)
        {
            foreach (Action action in character.actions)
            {
                buttons[skillsToDisable[i]].GetComponentInChildren<Text>().text = action.actionName;
                buttons[skillsToDisable[i]].gameObject.SetActive(true);
                p1Buttons[skillsToDisable[i]].interactable = action.active;
                ++i;
            }
        }
        else
        {
            foreach (Action action in character.actions)
            {
                buttons[skillsToDisable[i]].GetComponentInChildren<Text>().text = action.actionName;
                buttons[skillsToDisable[i]].gameObject.SetActive(true);
                p1Buttons[skillsToDisable[i]].interactable = false;
                ++i;
            }
        }


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

        ReloadCharactersInfo();
    }

    void OnButtonClick(string butName, int playerIndex)
    {
        Debug.Log("player: " + playerIndex + ", " + butName);

        //// TESTING
        //Action action = new Action();
        //action.actionName = butName;
        //AddCombatLogEntry(playerIndex, playerIndex == 0 ? 1 : 0, action);

        Character selectedChar = playerIndex == 0 ? characterA : characterB;
      

        switch (butName)
        {
            case "Skill1Button":
                {
                   // TODO
                    break;
                }
            case "Skill2Button":
                {
                    // TODO
                    break;
                }
            case "Skill3Button":
                {
                    // TODO
                    break;
                }
            case "Skill4Button":
                {
                    // TODO
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
            case "ButtonPlusHp":
                {
                    selectedChar.GetStat("life").baseValue += 1.0f;
                    break;
                }
            case "ButtonMinHp":
                {
                    selectedChar.GetStat("life").baseValue -= 1.0f; 
                    break;
                }
            case "ButtonPlusAtk":
                {
                    selectedChar.GetStat("damage").baseValue += 1.0f;
                    break;
                }
            case "ButtonMinAtk":
                {
                    selectedChar.GetStat("damage").baseValue -= 1.0f;
                    break;
                }
            case "ButtonPlusDef":
                {
                    selectedChar.GetStat("armor").baseValue += 1.0f;
                    break;
                }
            case "ButtonMinDef":
                {
                    selectedChar.GetStat("armor").baseValue -= 1.0f;
                    break;
                }
            case "ButtonPlusSpeed":
                {
                    selectedChar.GetStat("speed").baseValue += 1.0f;
                    break;
                }
            case "ButtonMinSpeed":
                {
                    selectedChar.GetStat("speed").baseValue -= 1.0f;
                    break;
                }
            case "prevCharacter":
                {
                    int newType = (playerIndex == 0) ? (int)gameMan.selectedCharA : (int)gameMan.selectedCharB;
                    --newType;

                    if (newType < 0)
                    {
                        newType = (int)CharacterType.Max -1;
                    }

                    if (playerIndex == 0)
                    {
                        gameMan.selectedCharA = (CharacterType)newType;
                    }
                    else
                    {
                        gameMan.selectedCharB = (CharacterType)newType;
                    }

                    break;
                }
            case "nextCharacter":
                {
                    int newType = (playerIndex == 0) ? (int)gameMan.selectedCharA : (int)gameMan.selectedCharB;
                    ++newType;

                    if (newType >= (int)CharacterType.Max)
                    {
                        newType = 0;
                    }

                    if (playerIndex == 0)
                    {
                        gameMan.selectedCharA = (CharacterType)newType;
                    }
                    else
                    {
                        gameMan.selectedCharB = (CharacterType)newType;
                    }

                    break;
                }
            case "ButtonPlusLvl":
                {
                    // TODO
                    break;
                }
            case "ButtonMinLvl":
                {
                    // TODO
                    break;
                }
            case "SimulateButton":
                {
                    int numSimulations = int.Parse(gameButtons[butName].GetComponentInChildren<InputField>().text);
                    Debug.Log(numSimulations);

                    break;
                }

        }

        // nerf this
        ReloadCharactersInfo();

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
        combatLogText.text += "Player " + fromPlayer + " used " + action.actionName + " to " + toPlayer + "\n";
        
        Canvas.ForceUpdateCanvases();
        combatLogText.GetComponent<ContentSizeFitter>().SetLayoutVertical();

        if (scrollRect)
        {
            scrollRect.SetLayoutVertical();
            scrollRect.verticalNormalizedPosition = 0.0f;
        }
        
    }
}
