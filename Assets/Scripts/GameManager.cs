using System.Collections.Generic;
using UnityEngine;

public static class GameSettings {
    
    public static int ColorRange;
    public static int row;
    public static int column;
    public static int bombThreshold;
    
}

public class GameManager : MonoBehaviour
{
    public static Player currentPlayer;
    public GameObject SelectionBorderPrefab;
    
    private int playerScore;
    private static int currentComboLevel;
    private State currentState;
    private State lastStateBeforePause;
    
    private static List<Player> players;
    private InputState IS;
    private Rotator rotator;
    private TripleGroup tripleGroup;

    enum State {
        INITIAL,
        CHOSE_HEX_GROUP,
        ANIMATION,
        GAME_OVER,
        PAUSE
    };
    
    // Start is called before the first frame update
    void Start() {
        currentComboLevel = 0;
        IS = FindObjectOfType<InputState>();
        currentState = State.PAUSE;
    }
    
    public void StartGame() {
        CreateGameBoard();
        CameraSettings.ReStartCam();
        GameObject SelectionBorder = Instantiate(SelectionBorderPrefab);
        SelectionBorder.transform.parent = MenuManager.GetPanel("GamePanel").transform;
        SelectionBorder.transform.localScale = new Vector3(70, 70, 70);
        PlayerListInitilizer();
        // TODO: start game vs restart game (may be an error at restart)
        //pannel.SetActive(false);
        FindObjectOfType<AudioManager>().Play("backgroundMusic");
        tripleGroup = FindObjectOfType<TripleGroup>();
        rotator = FindObjectOfType<Rotator>();
        playerScore = 0;
        currentState = State.INITIAL;
    }
    
    private void PlayerListInitilizer() {
        players = new List<Player>();
        addPlayer(FindObjectOfType<Player>().GetComponent<Player>());
        ChangeCurrentPlayer(getAdminPlayer());
    }
    
    public void addPlayer(Player given) {
        given.Initilize();
        players.Add(given);
    }
    
    private void CreateGameBoard() {
        FindObjectOfType<GameBoard>().Initialize(GameSettings.row, GameSettings.column,0.88f,1);
        GameBoard.LoadAllBoard(GameSettings.ColorRange);
    }
    
    public void EndGame() {
        Debug.LogWarning("endGame Called");
         currentState = State.PAUSE;
         FindObjectOfType<AudioManager>().StopAll();
         Debug.LogWarning("TripleGroup destroyed");
         GameBoard.GameOver();
         Destroy(tripleGroup.gameObject);
         FindObjectOfType<MenuManager>().OpenGameover();
         players.ForEach(P => { P.ResetPlayer(); });
     }
    
    public void Pause() {
        //pausePanel.SetActive(true);
        FindObjectOfType<AudioManager>().PauseAll();
        lastStateBeforePause = currentState;
    }
    
    public void UnPause() {
        //pausePanel.SetActive(false);
        FindObjectOfType<AudioManager>().ResumeAll();
        currentState = lastStateBeforePause;
    }
    
    
    // Update is called once per frame
    void Update() {
        switch (currentState) {
            //////////////////////////////////////////
            //////////////////////////////////////////
            case State.PAUSE:
                break;
            //////////////////////////////////////////
            //////////////////////////////////////////
            case State.INITIAL:
                currentState = State.CHOSE_HEX_GROUP;
                break;
            //////////////////////////////////////////
            //////////////////////////////////////////
            case State.CHOSE_HEX_GROUP:
                //Debug.Log("GM , State.SPIN_HEX_GROUP");
                if (!GetAnyPossibleMove()) {
                    EndGame();
                    break;
                }
                tripleGroup.checkSelection();
                if (tripleGroup.checkSwipe()) {
                    ResetComboLevel();
                    tripleGroup.RotateSelectedHexagons();
                    currentState = State.ANIMATION;
                    //call animation start in hexagons;
                }
                break;
            //////////////////////////////////////////
            //////////////////////////////////////////
            case State.ANIMATION:
                if (!Rotator.isAnimationing()) {
                    // if (GetAnyPossibleMove()) {
                    //     currentState = State.GAME_OVER; 
                    //     break;
                    // }
                    currentState = State.CHOSE_HEX_GROUP;
                }
                else {
                    Debug.Log("update idle loop");
                }
                break;
            //////////////////////////////////////////
            //////////////////////////////////////////
            case State.GAME_OVER:
                EndGame();
                break;
        }
    }
    
    
    
    public static bool GetAnyPossibleMove() {
        var hexagons = GameBoard.GetCurrentRealHexagonList();
        bool possibleExplosion = false;
        foreach (var hex in hexagons) {
            if (hex && hex.CheckAnyPosibleExplosion()) {
                possibleExplosion = true;
            }
        }
        Debug.LogWarning("possibleExplosion = "+ possibleExplosion);
        return possibleExplosion;
    }
    
    public static int GetCurrentCombo() {
        return currentComboLevel;
    }

    public static void IncreaseComboLevel() {
        currentComboLevel++;
    }
    
    public static void ResetComboLevel() {
        currentComboLevel = 0;
    }

    public static Player getPlayer (int id) {
        //TODO if there are player over than 1, we need a create player list and return their of given id from the list
        if (players[id]) {
            return players[id];
        }else {
            return null;
        }
    }

    public static Player GetCurrentPlayer() {
        return currentPlayer;
    }
    
    public static void ChangeCurrentPlayer(Player playerPlayingNow) {
        if (playerPlayingNow != null) {
            currentPlayer = playerPlayingNow;
        }
    }
    
    public static Player getAdminPlayer () {
        var id = 0; //default admin id;
        return players[id];
        //TODO if there are player over than 1, we need a create player list and return their of given id from the list
    }
    
}
