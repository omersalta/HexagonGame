using System;
using System.Collections.Generic;
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
        
        A = GameBoard.getBoardHexagons(x, y+1);
        B = even ? GameBoard.getBoardHexagons(x + 1, y) : GameBoard.getBoardHexagons(x + 1, y + 1);
        C = even ? GameBoard.getBoardHexagons(x + 1, y - 1) : GameBoard.getBoardHexagons(x + 1, y);
        D = GameBoard.getBoardHexagons(x, y - 1);
        E = even ? GameBoard.getBoardHexagons(x - 1, y - 1) : GameBoard.getBoardHexagons(x - 1, y);
        F = even ? GameBoard.getBoardHexagons(x - 1, y) : GameBoard.getBoardHexagons(x - 1, y + 1);
    }
    
    public BoardHexagon A;
    public BoardHexagon B;
    public BoardHexagon C;
    public BoardHexagon D;
    public BoardHexagon E;
    public BoardHexagon F;
    
    public Hexagon GetRealA() {
        return A.myHexagon;
    }
    public Hexagon GetRealB() {
        return B.myHexagon;
    }
    public Hexagon GetRealC() {
        return C.myHexagon;
    }
    public Hexagon GetRealD() {
        return D.myHexagon;
    }
    public Hexagon GetRealE() {
        return E.myHexagon;
    }
    public Hexagon GetRealF() {
        return F.myHexagon; 
    }
    
}

public class Hexagon : AbsHexagon {
    
    public BoardHexagon myBoardHexagon;
    public bool mustExplode; //mening must be mustExplode current frame
    public int color; //color index
    
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
        name = x + "," + y;
        mustExplode = false;
        setColor(color);
        if (GameBoard.getBoardHexagons(givenX, givenY).myHexagon == null) {
             myBoardHexagon = GameBoard.getBoardHexagons(givenX,givenY);
        }else {
            Debug.LogWarning(name+": this Abstract method already pick a real hexagon");
            Destroy(gameObject);
        }
        //setColor(Random.Range(1, GameBoard.getMaxColorNumber() + 1));
    }
    
    private void OnMouseUpAsButton() {
        //Debug.Log(gameObject.name+" is called setChosenHex() when mouse ="+ Input.mousePosition );
       
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
    
    public bool checkExplode() {
        
        Hexagon A, B, C, D, E, F;
        
        A = myBoardHexagon.GetRealA();
        B = myBoardHexagon.GetRealB();
        C = myBoardHexagon.GetRealC();
        D = myBoardHexagon.GetRealD();
        E = myBoardHexagon.GetRealE();
        F = myBoardHexagon.GetRealF();
        
        mustExplode = false;
        if (A && B && color == A.color && color == B.color) {
            
            mustExplode = true;
        }
        
        if (B && C && color == B.color && color == C.color) {
            mustExplode = true;
        }
        
        if (C && D && color == C.color && color == D.color) {
            mustExplode = true;
        }
        
        if (D && E && color == D.color && color == E.color) {
            mustExplode = true;
        }

        if (E && F && color == E.color && color == F.color) {
            mustExplode = true;
        }
        
        if (F && A && color == F.color && color == A.color) {
            mustExplode = true;
        }

        // Debug.Log("im returing this:"+mustExplode+", my color is :"+color);
        return mustExplode;
    }
    
    public bool checkPossibleExplosions (int whichColor, Hexagon except) {
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
        
        mustExplode = false;
        if (A && B && A != except && B != except && checkColor == A.color && checkColor == B.color) {
            mustExplode = true;
        }
        
        if (B && C && B != except && C != except && checkColor == B.color && checkColor == C.color) {
            mustExplode = true;
        }
        
        if (C && D && C != except && D != except && checkColor == C.color && checkColor == D.color) {
            mustExplode = true;
        }
        
        if (D && E && D != except && E != except && checkColor == D.color && checkColor == E.color) {
            mustExplode = true;
        }

        if (E && F && E != except && F != except && checkColor == E.color && checkColor == F.color) {
            mustExplode = true;
        }
        
        if (F && A && F != except && A != except && checkColor == F.color && checkColor == A.color) {
            mustExplode = true;
        }
        
        // Debug.Log("im returing this:"+mustExplode+", my color is :"+color);
        return mustExplode;
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

    public void Fall (){
        if (myBoardHexagon.GetRealD() != null)
            return;
        
    }
    
}



