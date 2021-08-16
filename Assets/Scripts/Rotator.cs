using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Rotator : MonoBehaviour
{
    // Start is called before the first frame update
    #region Singleton Instance region
    private static Rotator instance = null;

    private void OnEnable() {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad( this.gameObject );
    }

    // Game Instance Singleton
    public static Rotator Instance {
        get { return instance; }
    }
    #endregion

    private static bool currentDirection;
    private static int step;
    private static int AnimationCounter;
    
    void Start() {
        
    }
    
    private static void IncrementCounter() {
        AnimationCounter++;
    }
    private static void DecrementCounter() {
        AnimationCounter--;
    }
    
    public static bool isAnimationing() {
        if (Faller.isAnimationing()) {
            return true;
        }
        return AnimationCounter != 0;
    }
    
    public static void finishCallback(TripleGroup tripleSelector) {
        DecrementCounter();
        if (!isAnimationing()) {
           
            //Cheking Explosion
            List<Hexagon> tempExplodeList = CheckingExplosion(tripleSelector);
            
            //Explode Theese
            if (tempExplodeList.Any()) {
                tempExplodeList = DistinctList(tempExplodeList);
                if (tempExplodeList.Count > 5) {
                    Debug.LogWarning("COMBOOOO over than 5 hex");
                    GameManager.IncreaseComboLevel();
                }
                //explode all must explode;
                FindObjectOfType<AudioManager>().Play("explodeHex",1+GameManager.GetCurrentCombo()*0.1f);
                Hexagon.ExplodeGivenList(tempExplodeList,GameManager.GetCurrentPlayer());
                if (GameBoard.GetFallingHexagons().Any()) {
                    Faller.FallThese(GameBoard.GetFallingHexagons());
                }
                else {
                    GameBoard.GenerateNewHexagons();
                }
                return;
            }

            if (step < 3) {
                StepRotation(tripleSelector);
            }
        }
    }

    public static List<Hexagon> CheckingExplosion (TripleGroup tripleSelector) {
        List<Hexagon> tempExplodeList = new List<Hexagon>();
        foreach (var selectedHexagon in tripleSelector.GetSelectedTrpile()) {
            var ExplosionList = selectedHexagon.GetExplosionList();
            ExplosionList.RemoveAll(item => item == null);
            ExplosionList.ForEach(hex => tempExplodeList.Add(hex));
        }
        return tempExplodeList;
    }
    
    public static List<Hexagon> DistinctList (List<Hexagon> givenList) {
        List<Hexagon> tempList = new List<Hexagon>();
        foreach (var VARIABLE in givenList) {
            if (!tempList.Contains(VARIABLE)) {
                tempList.Add(VARIABLE);
            }
        }
        return tempList;
    }
    
    public static void StartCallback() {
        IncrementCounter();
    }
    
    public static void Rotation(bool direction, TripleGroup tripleSelector) {
        GameManager.ResetComboLevel();
        step = 0;
        currentDirection = direction;
        StepRotation(tripleSelector);
    }

    public static void StepRotation(TripleGroup tripleSelector) {
        step++;
        
        if (tripleSelector.GetSelectedTrpile().Count != 3) {
            return;
        }
        FindObjectOfType<AudioManager>().Play("flipping");
        
        var Hexagon1 = tripleSelector.GetSelectedTrpile()[0];
        var Hexagon2 = tripleSelector.GetSelectedTrpile()[1];
        var Hexagon3 = tripleSelector.GetSelectedTrpile()[2];

        if (currentDirection == false) {
            Hexagon1 = tripleSelector.GetSelectedTrpile()[2];
            Hexagon2 = tripleSelector.GetSelectedTrpile()[1];
            Hexagon3 = tripleSelector.GetSelectedTrpile()[0];
        }

        FindObjectOfType<TripleGroup>().Rotation(currentDirection);
        Hexagon1.StepRotation(tripleSelector, currentDirection, Hexagon2.myBoardHexagon);
        Hexagon2.StepRotation(tripleSelector, currentDirection, Hexagon3.myBoardHexagon);
        Hexagon3.StepRotation(tripleSelector, currentDirection, Hexagon1.myBoardHexagon);
        
    }
    
}
