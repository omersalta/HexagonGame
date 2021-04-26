using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleGroup : MonoBehaviour {

    private static List<Hexagon> currentSelectedHexagons = new List<Hexagon>();

    public static void selectTripleHexagons(Vector2 upPos) {

        List<Hexagon> tempList = new List<Hexagon>();
        
        float minDist1 = float.MaxValue;
        float minDist2 = float.MaxValue;
        float minDist3 = float.MaxValue;

        Hexagon closest1 = null;
        Hexagon closest2 = null;
        Hexagon closest3 = null;
        
        foreach (var hex in GameBoard.GetCurrentRealHexagonList()) {
            var t = hex.transform;
            //Debug.Log("curentHex transform = "+ t.position);
            float dist = Vector2.Distance(Camera.main.WorldToScreenPoint(t.position), upPos);
            
            if (dist < minDist1) {
                closest2 = closest1;
                closest1 = hex;
                minDist2 = minDist1;
                minDist1 = dist;
            }
            if (dist < minDist2) {
                closest3 = closest2;
                closest2 = hex;
                minDist3 = minDist2;
                minDist2 = dist;
                
            }
            if (dist < minDist3) {
                closest3 = hex;
                minDist3 = dist;
            }
            
        }
        
        tempList.Add(closest1);
        tempList.Add(closest2);
        tempList.Add(closest3);
        
        currentSelectedHexagons = sortSelectedHexs(tempList);
    }
    
    public static List<Hexagon> sortSelectedHexs (List<Hexagon> unsortedHexs) {
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

        foreach (var h in sortedHexs) {
            //Debug.Log(h.name);
        }
        return sortedHexs;
    }

    public static List<Hexagon> GetSelectedTrpile() {
        return currentSelectedHexagons;
    } 
    
}
