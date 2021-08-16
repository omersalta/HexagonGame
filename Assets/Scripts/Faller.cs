using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;

public class Faller : MonoBehaviour
{
    // Start is called before the first frame update
    #region Singleton Instance region
    private static Faller instance = null;

    private void OnEnable() {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad( this.gameObject );
    }

    // Game Instance Singleton
    public static Faller Instance {
        get { return instance; }
    }
    #endregion
    
    private static int step;
    private static int AnimationCounter;
    private static List<Hexagon> lastFallHexagons = new List<Hexagon>();

    private void Awake() {
        Debug.LogWarning("lastFallHexagons CREATED");
        lastFallHexagons = new List<Hexagon>();
    }

    void Start() {
        
    }
    
    private static void IncrementCounter() {
        AnimationCounter++;
    }
    private static void DecrementCounter() {
        AnimationCounter--;
    }
    
    public static bool isAnimationing() {
        return AnimationCounter != 0;
    }
    
    public static void StartCallback() {
        IncrementCounter();
    }

    public static void addToLastFall(Hexagon givenFalling) {
        lastFallHexagons.Add(givenFalling);
    }
    
    public static void finishCallback() {
        
        DecrementCounter();
        //Debug.LogWarning(lastFallHexagons);
        //Debug.LogWarning("ANSWER");
        if (!isAnimationing()) {
            
            List<Hexagon> tempExplodeList = new List<Hexagon>();
            //Debug.LogWarning("ExplodeIfExist()");
            
            //Cheking Explosion
            if (lastFallHexagons.Any()) {
                foreach (var selectedHexagon in lastFallHexagons) {
                    //Debug.LogWarning("lastfaller:" + selectedHexagon +", color:"+ selectedHexagon.color);
                    var ExplosionList = selectedHexagon.GetExplosionList();
                    ExplosionList.RemoveAll(item => item == null);
                    ExplosionList.ForEach(hex => tempExplodeList.Add(hex));
                }
            }
            
            //Explode Theese
            if (tempExplodeList.Any()) {
                tempExplodeList = Rotator.DistinctList(tempExplodeList);
                GameManager.IncreaseComboLevel();
                FindObjectOfType<AudioManager>().Play("explodeHex",1+GameManager.GetCurrentCombo()*0.1f);
                Hexagon.ExplodeGivenList(tempExplodeList,GameManager.GetCurrentPlayer());
            }
            FallHexagonsIfExist();
        }
    }
    
    private static bool ExplodeIfExist() {

        bool existExplosion = false;
        List<Hexagon> tempExplodeList = new List<Hexagon>();
        //Debug.LogWarning("ExplodeIfExist()");
        
        //Cheking Explosion
        if (lastFallHexagons.Any()) {
            foreach (var selectedHexagon in lastFallHexagons) {
                //Debug.LogWarning("lastfaller:" + selectedHexagon +", color:"+ selectedHexagon.color);
                var ExplosionList = selectedHexagon.GetExplosionList();
                ExplosionList.RemoveAll(item => item == null);
                ExplosionList.ForEach(hex => tempExplodeList.Add(hex));
            }
        }
            
        //Explode Theese
        if (tempExplodeList.Any()) {
            tempExplodeList = Rotator.DistinctList(tempExplodeList);
            Debug.LogWarning("COMBOOOO Falling callback explosion");
            GameManager.IncreaseComboLevel();
            existExplosion = true;
            FindObjectOfType<AudioManager>().Play("explodeHex",1+GameManager.GetCurrentCombo()*0.1f);
            Hexagon.ExplodeGivenList(tempExplodeList,GameManager.GetCurrentPlayer());
        }
        return existExplosion;
    }

    private static void FallHexagonsIfExist() {
        
        if (GameBoard.GetFallingHexagons().Any()) {
            FallThese(GameBoard.GetFallingHexagons());
        }
        else {
            GameBoard.GenerateNewHexagons();
        }
        
    }
    
    public static void FallThese (List<Hexagon> FallingHexagons) {
        lastFallHexagons = FallingHexagons;
        foreach (var currentFall in FallingHexagons) {
            Fall(currentFall);
        }
    }   
    
    private static void Fall(Hexagon fallingHexagon) {
        
        int deepLevel = 1;
        
        BoardHexagon target = GameBoard.getBoardHexagon(fallingHexagon.myBoardHexagon.x,
            fallingHexagon.myBoardHexagon.y - deepLevel);
        
        while (target && target.D && target.GetRealD() == null) {
            deepLevel++;
            target = GameBoard.getBoardHexagon(fallingHexagon.myBoardHexagon.x,
                fallingHexagon.myBoardHexagon.y - deepLevel);
        }
        fallingHexagon.GoTo(target);
    }

}
