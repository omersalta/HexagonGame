using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Range(4, 7)]
    public int ColorRange;
    private static int maxColor;
    [Range(3, 9)]
    public int row;
    [Range(3, 9)]
    public int column;
    public GameObject pannel;
    [Range(75, 250)]
    public int bombThreshold;


    private int playerScore;
    private int currentBombThreshold;
    private State currentState;
    private State lastStateBeforePause;
    private Direction lastSwipeDirection;
    private InputState IS;
    private static int animationing; // if 0 meaning not animationing 
    private static bool anyExplosion;
    enum State {
        PAUSE,
        INITIAL,
        CHOSE_HEX_GROUP,
        ANIMATION,
        GAME_OVER
    }; 
    enum Direction {
        RIGHT,
        LEFT,
        NONE,
    }; 
    
    // Start is called before the first frame update
    void Start() {
        TripleGroup.setBorderObject(GameObject.Find("SelectionBorder"));
        maxColor = ColorRange;
        GameObject gameBoard = new GameObject("GameBoard");
        gameBoard.AddComponent<GameBoard>().Initialize(row, column,0.88f,1);
        IS = FindObjectOfType<InputState>();
        currentState = State.INITIAL;
    }

     public void StartGame() {
         pannel.SetActive(false);
         FindObjectOfType<AudioManager>().Play("BackgroundMusic");
         currentState = State.CHOSE_HEX_GROUP;
         
    }
    
    public void Pause() {
        pannel.SetActive(true);
        FindObjectOfType<AudioManager>().PauseAll();
        lastStateBeforePause = currentState;
    }
    
    public void UnPause() {
        pannel.SetActive(false);
        FindObjectOfType<AudioManager>().ResumeAll();
        currentState = lastStateBeforePause;
    }
    
    
    // Update is called once per frame
    void Update() {
        switch (currentState) {
            //////////////////////////////////////////
            //////////////////////////////////////////
            case State.INITIAL:
                animationing = 0;
                lastSwipeDirection = Direction.NONE;
                currentBombThreshold = 0;
                playerScore = 0;
                currentState = State.CHOSE_HEX_GROUP;
                GameBoard.LoadAllBoard(maxColor);
                break;
            //////////////////////////////////////////
            //////////////////////////////////////////
            case State.PAUSE:
                
                break;
            //////////////////////////////////////////
            //////////////////////////////////////////
            case State.CHOSE_HEX_GROUP:
                //Debug.Log("GM , State.SPIN_HEX_GROUP");
                checkSelection();
                if (checkSwipe()) {
                    TurnSelectedHexagons();
                    currentState = State.ANIMATION;
                    //call animation start in hexagons;
                }
                break;
            //////////////////////////////////////////
            //////////////////////////////////////////
            case State.ANIMATION:
                if (!GetAnyAnimationing()) {
                    if (true) {
                        currentState = State.GAME_OVER;
                    }else {
                        currentState = State.CHOSE_HEX_GROUP;
                    }
                }
                break;
            //////////////////////////////////////////
            //////////////////////////////////////////
            case State.GAME_OVER:
                // display score
                Debug.Log("Game is over");
                
                pannel.SetActive(true);
                break;
        }
    }

    public static void AnimationStart() {
        animationing++;
    }

    public static int GetMaxColor() {
        return maxColor;
    }
    
    public static void AnimationFinished() {
        animationing--;
    }
    
    public static bool GetAnyAnimationing() {
        return animationing == 0;
    }
    
    
    void checkSelection(){
        if (IS.Up) {
            TripleGroup.selectTripleHexagons(IS.upPos);
        }
    }
    
    bool checkSwipe() {
        bool swiping = false;
        if (IS.SwipeUp) {
            swiping = true;
            if (IS.downPos.x < TripleGroup.GetCenterOfSelection().x) {
                lastSwipeDirection = Direction.RIGHT;
            }else {
                lastSwipeDirection = Direction.LEFT;
            }
        }else if (IS.SwipeRight) {
            swiping = true;
            if (IS.downPos.y < TripleGroup.GetCenterOfSelection().y) {
                lastSwipeDirection = Direction.LEFT;
            }else {
                lastSwipeDirection = Direction.RIGHT;
            }
        }else if (IS.SwipeDown) {
            swiping = true;
            if (IS.downPos.x < TripleGroup.GetCenterOfSelection().x) {
                lastSwipeDirection = Direction.LEFT;
            }else {
                lastSwipeDirection = Direction.RIGHT;
            }
        }else if (IS.SwipeLeft) {
            swiping = true;
            if (IS.downPos.y < TripleGroup.GetCenterOfSelection().y) {
                lastSwipeDirection = Direction.RIGHT;
            }else {
                lastSwipeDirection = Direction.LEFT;
            }
        }

        if (TripleGroup.GetSelectedTrpile().Count != 3) {
            swiping = false;
        }
        
        return swiping;
    }
    
    void TurnSelectedHexagons() {
        
        if (TripleGroup.GetSelectedTrpile().Count != 3) {
            Debug.LogWarning("SelectedTriple count is not equal 3");
            return;
        }
        
        bool direction = false;
        if (lastSwipeDirection == Direction.RIGHT) {
            direction = true;
            Hexagon.Rotation(true);
        }
        else if (lastSwipeDirection == Direction.LEFT) {
            Hexagon.Rotation(false);
        }

    }
    
    
}
