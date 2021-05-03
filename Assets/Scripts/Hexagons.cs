using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public abstract class AbsHexagon : MonoBehaviour {
    public int x;
    public int y;
    public bool even;
}

public class Bomb : Hexagon {
    public int bombCountdown;
    
    public void Initilize (int givenX,int givenY,int color,int countDown) {
        base.Initilize(givenX, givenY, color);
        GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;
        bombCountdown = countDown;
        GetComponentInChildren<TextMesh>().text = bombCountdown.ToString();
    }
    
    public void decrementCountDown() {
        bombCountdown--;
        GetComponentInChildren<TextMesh>().text = bombCountdown.ToString();
    }
}

public class BoardHexagon : AbsHexagon {
    
    public Hexagon myHexagon;
    
    public void Initilize(int givenX, int givenY) {
        gameObject.AddComponent<SpriteRenderer>();
        x = givenX;
        y = givenY;
        
        if (x % 2 == 0)
            even = true;
        else
            even = false;
        
        A = GameBoard.getBoardHexagon(x, y+1);
        B = even ? GameBoard.getBoardHexagon(x + 1, y) : GameBoard.getBoardHexagon(x + 1, y + 1);
        C = even ? GameBoard.getBoardHexagon(x + 1, y - 1) : GameBoard.getBoardHexagon(x + 1, y);
        D = GameBoard.getBoardHexagon(x, y - 1);
        E = even ? GameBoard.getBoardHexagon(x - 1, y - 1) : GameBoard.getBoardHexagon(x - 1, y);
        F = even ? GameBoard.getBoardHexagon(x - 1, y) : GameBoard.getBoardHexagon(x - 1, y + 1);
    }
    
    public BoardHexagon A;
    public BoardHexagon B;
    public BoardHexagon C;
    public BoardHexagon D;
    public BoardHexagon E;
    public BoardHexagon F;
    
    public Hexagon GetRealA() {
        if (A) {
            return A.myHexagon;
        }
        return null;
    }
    public Hexagon GetRealB() {
        if (B) {
        return B.myHexagon;
        }
        return null;
    }
    public Hexagon GetRealC() {
        if (C) {
        return C.myHexagon;
        }
        return null;
    }
    public Hexagon GetRealD() {
        if (D) {
        return D.myHexagon;
        }
        return null;
    }
    public Hexagon GetRealE() {
        if (E) {
        return E.myHexagon;
        }
        return null;
    }
    public Hexagon GetRealF() {
        if (F) {
        return F.myHexagon; 
        }
        return null;
    }
    
}

public class Hexagon : AbsHexagon {
    
    public BoardHexagon myBoardHexagon;
    public int color; //color index
    
    
    private static float cycleStepTime = 0.33f;
    private static float delay = 0.03f;
    private static int proccesCounter;
    //Neighbors of RealHexagon
    private static Color[] _colors = new[] {
        Color.clear,
        Color.blue,
        Color.red,
        Color.green,
        Color.yellow,
        Color.cyan,
        Color.magenta,
        Color.grey,
        Color.black,
    };
    
    public void Initilize (int givenX,int givenY,int color) {
        x = givenX;
        y = givenY;
        name = x + "," + y + " (Real)";
        var sp  = Resources.Load<Sprite>("Sprites/hexagon");
        gameObject.AddComponent<SpriteRenderer>().sprite = sp;
        setColor(color);
        if (GameBoard.getBoardHexagon(givenX, givenY).myHexagon == null) {
             myBoardHexagon = GameBoard.getBoardHexagon(givenX,givenY);
             myBoardHexagon.myHexagon = gameObject.GetComponent<Hexagon>();
             transform.position = myBoardHexagon.transform.position + new Vector3(0, Random.Range(1,10), 0);
             gameObject.GetComponent<Hexagon>().SingleFall(GameBoard.getBoardHexagon(givenX,givenY));
        }else {
            Debug.LogWarning(name+": this Board Hexagon already pick a real hexagon");
            Destroy(gameObject);
        }
        //setColor(Random.Range(1, GameBoard.getMaxColorNumber() + 1));
    }

    private void ProcessStart() {
        proccesCounter++;
    }
    private void ProcessFinish() {
        proccesCounter--;
    }

    public static bool AnyProcess {
        get => proccesCounter == 0;
    }
    
    
    public void setColor(int colorNumber) {
        // Debug.Log("Called with colornumber: " + colorNumber);
        if (0 <= colorNumber && colorNumber < _colors.Length) {
            //Debug.Log("color changed on "+ name+ "before :"+colorNumber+" after:"+colorNumber);
            color = colorNumber;
            GetComponent<SpriteRenderer>().color = _colors[color];
            
        }else {
            Debug.Log("colorNumber not in range");
        }
    }
    
    
    private List<Hexagon> checkPossibleExplosions (int whichColor, Hexagon except) {
        int checkColor = whichColor;
        if (checkColor == 0)
            checkColor = color;
        
        Hexagon A, B, C, D, E, F;
        
        A = myBoardHexagon.GetRealA();
        B = myBoardHexagon.GetRealB();
        C = myBoardHexagon.GetRealC();
        D = myBoardHexagon.GetRealD();
        E = myBoardHexagon.GetRealE();
        F = myBoardHexagon.GetRealF();

        List<Hexagon> tempList = new List<Hexagon>();
        if (A && B && A != except && B != except && checkColor == A.color && checkColor == B.color) {
            tempList.Add(myBoardHexagon.myHexagon);
            tempList.Add(A);
            tempList.Add(B);
        }
        
        if (B && C && B != except && C != except && checkColor == B.color && checkColor == C.color) {
            tempList.Add(myBoardHexagon.myHexagon);
            tempList.Add(B);
            tempList.Add(C);
        }
        
        if (C && D && C != except && D != except && checkColor == C.color && checkColor == D.color) {
            tempList.Add(myBoardHexagon.myHexagon);
            tempList.Add(C);
            tempList.Add(D);
        }
        
        if (D && E && D != except && E != except && checkColor == D.color && checkColor == E.color) {
            tempList.Add(myBoardHexagon.myHexagon);
            tempList.Add(D);
            tempList.Add(E);
        }

        if (E && F && E != except && F != except && checkColor == E.color && checkColor == F.color) {
            tempList.Add(myBoardHexagon.myHexagon);
            tempList.Add(E);
            tempList.Add(F);
        }
        
        if (F && A && F != except && A != except && checkColor == F.color && checkColor == A.color) {
            tempList.Add(myBoardHexagon.myHexagon);
            tempList.Add(F);
            tempList.Add(A);
        }
        
        // Debug.Log("im returing this:"+mustExplode+", my color is :"+color);
        return tempList;
    }

    public List<Hexagon> getNeighbors() {
        
        List<Hexagon> tempList = new List<Hexagon>();
        
        Hexagon A, B, C, D, E, F;
        
        A = myBoardHexagon.GetRealA();
        B = myBoardHexagon.GetRealB();
        C = myBoardHexagon.GetRealC();
        D = myBoardHexagon.GetRealD();
        E = myBoardHexagon.GetRealE();
        F = myBoardHexagon.GetRealF();
        
        tempList.Add(A);
        tempList.Add(B);
        tempList.Add(C);
        tempList.Add(D);
        tempList.Add(E);
        tempList.Add(F);

        Debug.Log(A);
        Debug.Log(B);
        Debug.Log(C);
        Debug.Log(D);
        Debug.Log(E);
        Debug.Log(F);
        
        return tempList;
    }

    public List<Hexagon> checkExplosionList() {
        return checkPossibleExplosions(0,null);
    }
    
    
    
    public void Explode () {
        Destroy(gameObject);
    }
    
    public static void FallAll() {
        var FallList = GameBoard.GetFallingHexagons();
        while (FallList.Any()) {
            if (!AnyProcess) {
                
                FallList = GameBoard.GetFallingHexagons();
                foreach (var hex in FallList) {
                    hex.SingleFall(GameBoard.getBoardHexagon(hex.x, hex.y-1));
                }
                
            }
        }

        foreach (var emptyBoardHexagon in GameBoard.GetEmptyBoardHexagons()) {
            GameBoard.createHexagon(emptyBoardHexagon.x, emptyBoardHexagon.y,
                Random.Range(1, GameManager.GetMaxColor() + 1));
        }
    }

    public void SingleFall(BoardHexagon targetBoardHexagon) {
        ProcessStart();
        float _currentSec = 0;
        myBoardHexagon.myHexagon = null;
        myBoardHexagon = null;
        Vector3 from = transform.position;
        StartCoroutine(singleFall(targetBoardHexagon.transform.position));
        
        IEnumerator singleFall(Vector3 To) {
        
            yield return new WaitForSeconds(delay);
            _currentSec += delay-0.02f;
            transform.position = Vector3.Lerp(from, To, _currentSec / cycleStepTime);
            
            
            if (_currentSec < cycleStepTime) {
                StartCoroutine(singleFall(To));
            }else {
                myBoardHexagon = targetBoardHexagon;
                myBoardHexagon.myHexagon = gameObject.GetComponent<Hexagon>();
                ProcessFinish();
                StopCoroutine(singleFall(To));
            }
        }
    }

    public static void Rotation(bool direction) {
        
        if (TripleGroup.GetSelectedTrpile().Count != 3) {
            Debug.LogWarning("SelectedTriple count is not equal 3");
            return;
        }
        
        var Hexagon1 = TripleGroup.GetSelectedTrpile()[0];
        var Hexagon2 = TripleGroup.GetSelectedTrpile()[1];
        var Hexagon3 = TripleGroup.GetSelectedTrpile()[2];
        
        if (direction == false) {
            Hexagon1 = TripleGroup.GetSelectedTrpile()[2];
            Hexagon2 = TripleGroup.GetSelectedTrpile()[1];
            Hexagon3 = TripleGroup.GetSelectedTrpile()[0];
        }


        GameManager.AnimationStart();
        for (int i = 0; i < 3;) {
            if (!AnyProcess) {
                
                List<Hexagon> tempExplodeList = new List<Hexagon>();
                foreach (var selectedHexagon in TripleGroup.GetSelectedTrpile()) {
                    selectedHexagon.checkExplosionList().ForEach(hex => tempExplodeList.Add(hex));
                }
                
                if (tempExplodeList.Any()) {
                    tempExplodeList.ForEach(hex => hex.Explode());
                    FallAll();
                    return;
                }
                
                Hexagon1.StepRotation(TripleGroup.GetCenterOfSelection(),direction,Hexagon2.myBoardHexagon);
                Hexagon2.StepRotation(TripleGroup.GetCenterOfSelection(),direction,Hexagon3.myBoardHexagon);
                Hexagon3.StepRotation(TripleGroup.GetCenterOfSelection(),direction,Hexagon1.myBoardHexagon);
                
               
                i++;
            }
        }
        GameManager.AnimationFinished(); //if player miss turn triple hexagons
        
    }
    

    
    public void StepRotation(Vector3 centerPos, bool direction, BoardHexagon targetBoardHexagon) {
        
        float _currentSec = 0;
        
        ProcessStart();
        myBoardHexagon.myHexagon = null;
        myBoardHexagon = null;
        var directionVector = Vector3.back;
        if (!direction) {
            directionVector = Vector3.forward;
        }
        StartCoroutine(StepRotate(centerPos));
        
        IEnumerator StepRotate(Vector3 center) {
        
            yield return new WaitForSeconds(delay);
            _currentSec += delay;
            transform.RotateAround(center, directionVector, 10);
            
            
            if (_currentSec < cycleStepTime) {
                StartCoroutine(StepRotate(center));
            }else {
                myBoardHexagon = targetBoardHexagon;
                myBoardHexagon.myHexagon = gameObject.GetComponent<Hexagon>();
                ProcessFinish();
                StopCoroutine(StepRotate(center));
            }
        }
    }

}



