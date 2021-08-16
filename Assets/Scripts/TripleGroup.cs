using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TripleGroup : MonoBehaviour {

    private static List<Hexagon> currentSelectedHexagons = new List<Hexagon>();
    private static Vector3 centerOfSelectedHexagons;
    private static GameObject selectionBorder;
    private static int rotationProccesCounter;
    private static Direction lastSwipeDirection;
    private  InputState IS;
    
    enum Direction {
        RIGHT,
        LEFT,
        NONE,
    };

    void Start() {
         IS = FindObjectOfType<InputState>();
         lastSwipeDirection = Direction.NONE;
         selectionBorder = gameObject;
    }

    public void setBorderObject(GameObject givenAdress) {
        //selectionBorder = givenAdress;
    }
    
    public void Rotation(bool direction) {
        
        float _currentSec = 0;
        
        var directionVector = Vector3.back;
        if (!direction) {
            directionVector = Vector3.forward;
        }
        StartCoroutine(StepRotate());
        IEnumerator StepRotate() {
        
            yield return new WaitForSeconds(Hexagon.getPlusTimeR());
            _currentSec += Hexagon.getPlusTimeR();
            selectionBorder.transform.RotateAround(centerOfSelectedHexagons, directionVector, Hexagon.getRotAngleR());
            
            
            if (_currentSec < Hexagon.getEndTimeR()) {
                StartCoroutine(StepRotate());
            }else {
                StopCoroutine(StepRotate());
            }
        }
    }

    public  void selectTripleHexagons(Vector2 upPos) {
        
        List<Hexagon> tempList = new List<Hexagon>();
        float minDist = float.MaxValue;
        
        Hexagon closest1 = null;
        Hexagon closest2 = null;
        Hexagon closest3 = null;

        foreach (var hex in GameBoard.GetCurrentRealHexagonList()) {
            //Debug.LogWarning(hex);
            var t = hex.transform;
            float dist = Vector2.Distance(Camera.main.WorldToScreenPoint(t.position), upPos);
            if (dist < minDist) {
                closest1 = hex;
                minDist = dist;
            }
        }
        
        tempList.Add(closest1);
        minDist = float.MaxValue;

        var neighbors = closest1.getNeighbors();
        neighbors.RemoveAll(item => item == null);
        foreach (var neighbor in neighbors) {
            var t = neighbor.transform;
            float dist = Vector2.Distance(Camera.main.WorldToScreenPoint(t.position), upPos);
            if (dist < minDist) {
                closest2 = neighbor;
                minDist = dist;
            }
        }
        
        tempList.Add(closest2);
        minDist = float.MaxValue;
        
        var intersectList = closest1.getNeighbors().Intersect(closest2.getNeighbors()).ToList();
        intersectList.RemoveAll(item => item == null);
        foreach (var intersectNeigbor in intersectList ) {
            var t = intersectNeigbor.transform;
            float dist = Vector2.Distance(Camera.main.WorldToScreenPoint(t.position), upPos);
            if (dist < minDist) {
                closest3 = intersectNeigbor;
                minDist = dist;
            }
        }
        
        tempList.Add(closest3);
        currentSelectedHexagons = sortSelectedHexs(tempList);
    }
    
    public static List<Hexagon> sortSelectedHexs (List<Hexagon> unsortedHexs) {
        
        foreach (var h in unsortedHexs) {
            //Debug.Log("unsorted of name."+h.name +" adding to center");
        }
        
        List<Hexagon> sortedHexs = new List<Hexagon>();
        Hexagon hex;
        Hexagon hexYmin = null;
        float yMin = float.MaxValue;
        for (int i = 0; i < unsortedHexs.Count; i++) {
            hex = unsortedHexs[i];
            if (hex.transform.position.y < yMin) {
                yMin = hex.transform.position.y;
                hexYmin = hex;
            }
        }
        sortedHexs.Add(hexYmin);
        unsortedHexs.Remove(hexYmin);

        if (unsortedHexs[0].transform.position.x < unsortedHexs[1].transform.position.x ) {
            sortedHexs.Add(unsortedHexs[0]);
            sortedHexs.Add(unsortedHexs[1]);
        }
        else {
            sortedHexs.Add(unsortedHexs[1]);
            sortedHexs.Add(unsortedHexs[0]);
        }
        
        Vector3 center = Vector3.zero;
        foreach (var h in sortedHexs) {
            center += h.transform.position/sortedHexs.Count;
        }
        
        centerOfSelectedHexagons = center;
        selectionBorder.GetComponent<SpriteRenderer>().flipX = sortedHexs[0].myBoardHexagon.x == sortedHexs[1].myBoardHexagon.x;
        selectionBorder.transform.position = center;
        return sortedHexs; 
    }
    
    public  List<Hexagon> GetSelectedTrpile() {
        return currentSelectedHexagons;
    }

    public  Vector3 GetCenterOfSelection() {
        return centerOfSelectedHexagons;
    }
    
    public  Vector2 GetCenterOfSelectionV2() {
        var x = centerOfSelectedHexagons.x;
        var y = centerOfSelectedHexagons.y;
        return new Vector2(x,y);
    }
    
    
    public void checkSelection(){
        if (IS.Up) {
            selectTripleHexagons(IS.upPos);
            FindObjectOfType<AudioManager>().Play("selection");
        }
    }
    
    public bool checkSwipe() {
        
        bool swiping = false;
        Vector3 downPos = IS.downPos;
        Vector2 difference = downPos - Camera.main.WorldToScreenPoint(GetCenterOfSelection());
        
        if (IS.SwipeUp) {
            swiping = true;
            if (difference.x < 0) {
                lastSwipeDirection = Direction.RIGHT;
            }else {
                lastSwipeDirection = Direction.LEFT;
            }
        }else if (IS.SwipeRight) {
            swiping = true;
            if (difference.y < 0) {
                lastSwipeDirection = Direction.LEFT;

            }else {
                lastSwipeDirection = Direction.RIGHT;
            }
        }else if (IS.SwipeDown) {
            swiping = true;
            if (difference.x < 0) {
                lastSwipeDirection = Direction.LEFT;
                
            }else {
                lastSwipeDirection = Direction.RIGHT;
            }
        }else if (IS.SwipeLeft) {
            swiping = true;
            if (difference.y < 0) {
                lastSwipeDirection = Direction.RIGHT;
            }else {
                lastSwipeDirection = Direction.LEFT;
            }
        }

        if (GetSelectedTrpile().Count != 3) {
            swiping = false;
        }
        
        return swiping;
    }

    public void RotateSelectedHexagons() {
        
        if (GetSelectedTrpile().Count != 3) {
            Debug.LogWarning("SelectedTriple count is not equal 3");
            return;
        }
        Vector2 center = Camera.main.WorldToScreenPoint(GetCenterOfSelectionV2());
        selectTripleHexagons(center);
        //ColorSetterForTesting.saveHexagonColorMap();
        bool direction = false;
        var comp = gameObject.GetComponent<TripleGroup>();
        
        if (lastSwipeDirection == Direction.RIGHT) {
            direction = true;
            Rotator.Rotation(true, comp);
        }else if (lastSwipeDirection == Direction.LEFT) {
            Rotator.Rotation(false, comp);
        }
        
    }
    
    
    
    
}
