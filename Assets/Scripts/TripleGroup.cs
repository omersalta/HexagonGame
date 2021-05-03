using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TripleGroup : MonoBehaviour {

    private static List<Hexagon> currentSelectedHexagons = new List<Hexagon>();
    private static Vector3 centerOfSelectedHexagons;
    private static GameObject selectionBorder;
    
    public static void setBorderObject(GameObject givenAdress) {
        Debug.Log("selection border name is :" + givenAdress.name);
        selectionBorder = givenAdress;
    }

    public static void selectTripleHexagons(Vector2 upPos) {
        List<Hexagon> tempList = new List<Hexagon>();
        
        float minDist = float.MaxValue;
        
        Hexagon closest1 = null;
        Hexagon closest2 = null;
        Hexagon closest3 = null;

        foreach (var hex in GameBoard.GetCurrentRealHexagonList()) {
            var t = hex.transform;
            float dist = Vector2.Distance(Camera.main.WorldToScreenPoint(t.position), upPos);
            if (dist < minDist) {
                closest1 = hex;
                minDist = dist;
            }
        }
        
        tempList.Add(closest1);
        Debug.Log("first:"+closest1.name);
        minDist = float.MaxValue;

        var neighbors = closest1.getNeighbors();
        
        neighbors.RemoveAll(item => item == null);
        foreach (var neighbor in neighbors) {
            Debug.Log("second finder foreach:"+neighbor.name);
            var t = neighbor.transform;
            float dist = Vector2.Distance(Camera.main.WorldToScreenPoint(t.position), upPos);
            if (dist < minDist) {
                closest2 = neighbor;
                minDist = dist;
            }
        }
        
        tempList.Add(closest2);
        Debug.Log("second:"+closest2.name);
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
        Debug.Log("Third:"+closest3.name);
        
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
            Debug.Log("sortedhex of name."+h.name);
            center += h.transform.position/sortedHexs.Count;
        }
        Debug.Log("center calculated :"+center);
        
        
        
        centerOfSelectedHexagons = center;
        selectionBorder.GetComponent<SpriteRenderer>().flipX = sortedHexs[0].x == sortedHexs[1].x;
        selectionBorder.transform.position = center;
        return sortedHexs; 
    }
    
    public static List<Hexagon> GetSelectedTrpile() {
        return currentSelectedHexagons;
    }

    public static Vector3 GetCenterOfSelection() {
        return centerOfSelectedHexagons;
    }
    
}
