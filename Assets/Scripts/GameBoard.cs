using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GameBoard : MonoBehaviour {
    //Singleton pattern 

    #region Singleton Instance region

    private static GameBoard instance = null;

    private void OnEnable() {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }

        instance = this;
        //DontDestroyOnLoad( this.gameObject );
    }

    // Game Instance Singleton
    public static GameBoard Instance {
        get { return instance; }
    }

    #endregion
    public GameObject hexPrefab;
    
    private float _xOffset;
    private float _yOffset;
    private static List <BoardHexagon> _BoardHexs;
    private static int _row;
    private static int _column;


    public void Initialize(int row, int column, float xOffset, float yOffset) {
        _xOffset = xOffset;
        _yOffset = yOffset;
        _row = row;
        _column = column;
        _BoardHexs = createBoard(_row, _column);
        
        for (int x = 0; x < column; x++) {
            for (int y = 0; y < row; y++) {
                _BoardHexs[y * _row + x].Initilize(x,y);
            }
        }
        
        Debug.Log("all element created");
    }

    private List <BoardHexagon> createBoard(int row, int column) {
        List <BoardHexagon> tempList = new List<BoardHexagon>();
        
        for (int x = 0; x < column; x++) {
            for (int y = 0; y < row; y++) {
                tempList.Add(createBoardHexagon(x, y));
            }
        }
        
        return tempList;
    }

    BoardHexagon createBoardHexagon(int x, int y) {
        GameObject go = new GameObject();
        BoardHexagon hex = go.AddComponent<BoardHexagon>();
        go.transform.parent = transform;
        go.name = x + ", " + y;
        if (x % 2 == 0)
            go.transform.position = new Vector3(x * _xOffset, y * _yOffset, 0);
        else
            go.transform.position = new Vector3(x * _xOffset, _yOffset / 2 + y * _yOffset, 0);
        return hex;
    }

    Hexagon GetHex (int x, int y) {
        return _BoardHexs[y*_row + x].myHexagon;
    }
    
    
    public static BoardHexagon getBoardHexagons (int x, int y) {
        if (_row > x && x >= 0 && _column > y && y >= 0)
            return _BoardHexs[y * _row + x];
        return null;
    }
    
    
    public static List<Hexagon> GetCurrentRealHexagonList() {
        List<Hexagon> tempHexs = new List<Hexagon>();
        foreach (var abstractHexagon in _BoardHexs) {
            tempHexs.Add(abstractHexagon.myHexagon);
        }
        return tempHexs;
    }
    
    public static int getRowNumber() {
        return _row;
    }

    public static int getColumnNumber() {
        return _column;
    }
    
    // public static List<RealHexagon> GetRow() {
    //     
    // }
    
    public static List<Hexagon> GetColumn (int index) {
        if (0 > index || index > getRowNumber()) {
            throw new InvalidOperationException("index should be in --- 0 <= inex < maxRow --- ");
        }
        
        List<Hexagon> tempList = new List<Hexagon>();
        foreach (var hex in GetCurrentRealHexagonList()) {
            if (hex.x == index) {
                tempList.Add(hex);
            }
        }
        return tempList;
        
    }
    
}