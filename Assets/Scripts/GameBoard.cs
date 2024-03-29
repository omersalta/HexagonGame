﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


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
    private Vector2 zeroPosition;
    
    private float _xOffset;
    private float _yOffset;
    private static List <BoardHexagon> _BoardHexs;
    private static int _row;
    private static int _column;
    private static GameObject _reals;
    private static GameObject _board;


    public void Initialize(int row, int column, float xOffset, float yOffset) {
        transform.localPosition = new Vector3(0, 0, 0);
        
        _board = new GameObject("_Board");
        _board.transform.parent = transform;
        _board.transform.localPosition = new Vector3(0, 0, 0);
        
        Debug.LogWarning("initilize called");
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
        
        _reals = new GameObject("_Reals");
        _reals.transform.parent = transform;
        _reals.transform.localPosition = new Vector3(0, 0, 0);

        //CameraSettings.ReStartCam();
    }

    public static void GameOver() {
        Destroy(GameObject.Find("_Reals"));
        Destroy(GameObject.Find("_Board"));
    }
    
    private List <BoardHexagon> createBoard (int row, int column) {
        List <BoardHexagon> tempList = new List<BoardHexagon>();
        
        for (int y = 0; y < column; y++) {
            for (int x = 0; x < row; x++) {
                //first hexagons of rows must created because of single dimension list
                tempList.Add(createBoardHexagon(x, y));
            }
        }
        
        return tempList;
    }

    public static void LoadAllBoard (int maxColor) {
        Debug.LogWarning("loadallbard called");
        for (int x = 0; x < _column; x++) {
            for (int y = 0; y < _row; y++) {
                CreateHexagon(x,y,Random.Range(1,maxColor+1));
            }
        }
    }
    
    public static Hexagon CreateHexagon (int x, int y, int color) {
        //Debug.LogWarning("if already there is real hexagon in same position it will destroy");
        GameObject hex = new GameObject();
        hex.transform.parent = _reals.transform;
        hex.AddComponent<Hexagon>().Initilize(x,y,color);
        return hex.GetComponent<Hexagon>();
    }
    
    BoardHexagon createBoardHexagon(int x, int y) {
        GameObject go = new GameObject();
        BoardHexagon hex = go.AddComponent<BoardHexagon>();
        go.transform.parent = _board.transform;
        go.name = x + ", " + y;
        if (x % 2 == 0)
            go.transform.localPosition = new Vector3(x * _xOffset, y * _yOffset, 0);
        else
            go.transform.localPosition = new Vector3(x * _xOffset, _yOffset / 2 + y * _yOffset, 0);
        return hex;
    }

    Hexagon GetHex (int x, int y) {
        return _BoardHexs[y*_row + x].myHexagon;
    }
    
    public static BoardHexagon getBoardHexagon (int x, int y) {
        if (_row > x && x >= 0 && _column > y && y >= 0)
            return _BoardHexs[y * _row + x];
        return null;
    }
    
    public static List<BoardHexagon> GetEmptyBoardHexagons () {
        List<BoardHexagon> tempList = new List<BoardHexagon>();
        foreach (var boardHex in _BoardHexs) {
            if (boardHex.myHexagon == null) {
                tempList.Add(boardHex);
            }
        }
        return tempList;
    }
    
    public static List<Hexagon> GetFallingHexagons () {
        
        List<Hexagon> tempList = new List<Hexagon>();
        for (int x = 0; x < getColumnNumber(); x++) {
            bool stimulus = false;
            for (int y = 0; y < getRowNumber(); y++) {
                var myHex = getBoardHexagon(x, y).myHexagon;
                if (myHex == null) {
                    stimulus = true;
                }
                if (stimulus && myHex) {
                    tempList.Add(myHex);
                }
            }  
        }
        
        return tempList;
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
    
    public static void GenerateNewHexagons () {
        
        int maxColor = GameSettings.ColorRange;
        
        // private static  List<BoardHexagon> getHexs() {
        //     List<BoardHexagon> EmptyBoardHexs = new List<BoardHexagon>();
        //     GetColumn()
        // }
        //Debug.LogWarning("maxColor :"+ maxColor);
        foreach (var emptyBH in GetEmptyBoardHexagons()) {
            //Debug.Log("emptyBH = " + emptyBH.name);
            Faller.addToLastFall(CreateHexagon(emptyBH.x, emptyBH.y, Random.Range(1, maxColor + 1)));
        }
        
    }
    
}