using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class AbsHexagon : MonoBehaviour {
    public int x;
    public int y;
    public bool even;
}

public class Bomb : Hexagon {
    public int bombCountdown;
    
    public void Initilize (int givenX, int givenY, int color, int countDown) {
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
    
    private static float cycleStepTime = 0.50f;
    private static float delay = 0.01f;
    private static float cycleStepTimeR = 0.20f;
    private static float delayR = 0.005f;
    
    public static float getEndTime() {
        return cycleStepTime;
    }
    
    public static float getPlusTime() {
        return delay;
    }
    
    public static float getEndTimeR() {
        return cycleStepTimeR;
    }
    
    public static float getPlusTimeR() {
        return delayR;
    }
    
    public static float getRotAngleR() {
        return 120/((cycleStepTimeR/delayR)+1);
    }
    
    private static int rotationProccesCounter;
    //Neighbors of RealHexagon
    
    private static Color32[] _colors = new[] {
        new Color32(10,150,255, 255),
        new Color32(10,150,255, 255),
        new Color32(255,40,50, 255),
        new Color32(50,255,40, 255),
        new Color32(240,255,40, 255),
        new Color32(255,160,10, 255),
        new Color32(250,40,250, 255),
        new Color32(0,0,0, 255),
    };
    
    public void Initilize (int givenX,int givenY,int color) {
        x = givenX;
        y = givenY;
        name = x + "," + y + " (Real)";
        var sp  = Resources.Load<Sprite>("Artwork/Sprites/hexagon");
        gameObject.AddComponent<RecycleGameObject>();
        gameObject.AddComponent<SpriteRenderer>().sprite = sp;
        setColor(color);
        if (GameBoard.getBoardHexagon(givenX, givenY).myHexagon == null) {
             myBoardHexagon = GameBoard.getBoardHexagon(givenX,givenY);
             myBoardHexagon.myHexagon = gameObject.GetComponent<Hexagon>();
             transform.position = myBoardHexagon.transform.position + new Vector3(0, Random.Range(1,10), 0);
             gameObject.GetComponent<Hexagon>().GoTo(GameBoard.getBoardHexagon(givenX,givenY));
        }else {
            Debug.LogWarning(name+": this Board Hexagon already pick a real hexagon");
            Destroy(gameObject);
        }
        //setColor(Random.Range(1, GameBoard.getMaxColorNumber() + 1));
    }
    
    public void setColor(int colorNumber) {
        // Debug.Log("Called with colornumber: " + colorNumber);
        if (0 <= colorNumber && colorNumber < _colors.Length) {
            //Debug.Log("color changed on "+ name+ "before :"+colorNumber+" after:"+colorNumber);
            color = colorNumber;
            GetComponent<SpriteRenderer>().color = (Color) _colors[colorNumber];;
        }else {
            Debug.Log("colorNumber not in range");
        }
    }
    
    private List<Hexagon> CE (int whichColor, Hexagon except) {
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
        
        return tempList;
    }

    public List<Hexagon> GetExplosionList() {
        return CE(0,null);
    }
    
    public bool CheckAnyPosibleExplosion() {
        bool possibleExplosion = false;
        foreach (var myNeighbor in getNeighbors()) {
            if (myNeighbor && myNeighbor.CE(color, GetComponent<Hexagon>()).Any()) {
                possibleExplosion = true;
            }
        }
        return possibleExplosion;
    }

    public static void ExplodeGivenList(List<Hexagon> givenList, Player player) {
        givenList.ForEach(hex => {
            hex.myBoardHexagon.myHexagon = null;
            hex.Explode();
        });
        var CalculatedScore = calculateScore(givenList.Count, player);
        /*Debug.LogWarning("Count = " + givenList.Count + "calculated = " +
                         CalculatedScore + "combo:"+GameManager.GetCurrentCombo());*/
        
        player.AddScore(CalculatedScore);
        var hexColor = _colors[givenList[1].color];
        var Scoring = GameObjectUtil.Instantiate(Resources.Load("prefabs/Scoring") as GameObject, GetCenter(givenList));
        Scoring.GetComponent<PointsTextScirpt>().SetValue(CalculatedScore,hexColor);
        //Scoring.GetComponent<TextMeshProUGUI>().text = CalculatedScore.ToString();
    }
    
    

    public static Vector3 GetCenter(List<Hexagon> givenList) {
        var totalX = 0f;
        var totalY = 0f;
        
        foreach(var element in givenList)
        {
            totalX += element.transform.position.x;
            totalY += element.transform.position.y;
        }
        
        var centerX = totalX / givenList.Count;
        var centerY = totalY / givenList.Count;
        return new Vector3(centerX, centerY, 0f);
    }
    
    
    private static int calculateScore(int explodeCount, Player player) {
        return explodeCount * 15 * player.GetComboMultiplier();
    }
    
    public void Explode () {
        var variableForPrefab = Resources.Load("prefabs/ExplodeEffect") as GameObject;
        var ps = GameObjectUtil.Instantiate(variableForPrefab, transform.position);
        var main = ps.GetComponent<ParticleSystem>().main;
        main.startColor = (Color)_colors[color];
        Destroy(gameObject);
    }
    
    public void GoTo(BoardHexagon targetBoardHexagon) {
        float _currentSec = 0;
        Vector3 from = transform.localPosition;
        Faller.StartCallback();
        myBoardHexagon.myHexagon = null;
        myBoardHexagon = null;
        myBoardHexagon = targetBoardHexagon;
        myBoardHexagon.myHexagon = gameObject.GetComponent<Hexagon>();
        x = myBoardHexagon.x;
        y = myBoardHexagon.y;
        
        StartCoroutine(singleFall(targetBoardHexagon.transform.localPosition));
        
        IEnumerator singleFall(Vector3 To) {
        
            yield return new WaitForSeconds(getPlusTime());
            _currentSec += getPlusTime();
            transform.localPosition = LerpV2( from, To, _currentSec / getEndTime(), 3);
            //transform.position = Vector3.Lerp( from, To, _currentSec / cycleStepTime);
            
            if (_currentSec < getEndTime()) {
                StartCoroutine(singleFall(To));
            }else {
                StopCoroutine(singleFall(To));
                Faller.finishCallback();
            }
        }

        Vector3 LerpV2(Vector3 a, Vector3 b, float t, int x) {
            if (x == 1) {
                return Vector3.Lerp(a, b, t);
            }else {
                return LerpV2(Vector3.Lerp( a, b, t), b, t, x - 1);
            }
        }
        
        
    }
    
    
    public void StepRotation(TripleGroup tripleSelector, bool direction, BoardHexagon targetBoardHexagon) {

        Vector3 cPos = tripleSelector.GetCenterOfSelection();
        float _currentSec = 0;
        var directionVector = Vector3.back;
        if (!direction) {
            directionVector = Vector3.forward;
        }
        
        Rotator.StartCallback();
        StartCoroutine(StepRotate(cPos));
        
        IEnumerator StepRotate(Vector3 center) {
        
            yield return new WaitForSeconds(getPlusTimeR());
            _currentSec += getPlusTimeR();
            transform.RotateAround(center, directionVector, getRotAngleR());
            
            if (_currentSec < getEndTimeR()) {
                StartCoroutine(StepRotate(center));
            }else {
                myBoardHexagon = targetBoardHexagon;
                targetBoardHexagon.myHexagon = GetComponent<Hexagon>();
                StopCoroutine(StepRotate(center));
                Rotator.finishCallback(tripleSelector);
            }
        }
    }
    
}
