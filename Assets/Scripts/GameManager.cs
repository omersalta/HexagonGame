using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int maxColor;
    public GameObject pannel;
    [Range(3, 9)]
    public int row;
    [Range(3, 9)]
    public int column;
    
    private State currentState;
    private State lastStateBeforePause;
    
    enum State {
        PAUSE,
        INITIAL,
        CHOSE_HEX_GROUP,
        SPIN_ANIMATION,
        FALL_HEX,
        GAME_OVER
    }; 
    
    // Start is called before the first frame update
    void Start() {
        
        GameObject gameBoard = new GameObject("GameBoard");
        gameBoard.AddComponent<GameBoard>().Initialize(row, column,1,1);
        //gameBoard.AddComponent<GameBoard>()
        
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
                
                currentState = State.PAUSE;
                break;
            //////////////////////////////////////////
            //////////////////////////////////////////
            case State.PAUSE:
                break;
            
            //////////////////////////////////////////
            //////////////////////////////////////////
            case State.CHOSE_HEX_GROUP:
                //Debug.Log("GM , State.SPIN_HEX_GROUP");
               
                break;
            //////////////////////////////////////////
            //////////////////////////////////////////
            case State.SPIN_ANIMATION:
               
                break;
            //////////////////////////////////////////
            //////////////////////////////////////////
            case State.FALL_HEX: 
                
                // bomb reaced to zero game over, goto GAME_OVER state
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
}
