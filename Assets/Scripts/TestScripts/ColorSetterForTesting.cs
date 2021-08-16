using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;


public class ColorSetterForTesting : MonoBehaviour {
    
    static StreamWriter sw; 
    void Start()
    {
      
    }

    public void setColorForTest() {
        return;
        int[] array1 = { 3, 1, 3, 2, 4, 5, 1, 3, 1, 1, 4, 6, 5, 3, 5, 1, 4, 3, 6, 2, 1, 2, 5, 2, 5 };
        int[] array2 = { 3, 1, 3, 2, 4, 5, 1, 3, 1, 1, 4, 6, 5, 3, 5, 1, 4, 3, 6, 2, 1, 2, 5, 2, 5 };
        int i = 0;
        foreach (var hex in GameBoard.GetCurrentRealHexagonList()) {
            hex.setColor(array2[i]);
            i++;
        }
        
        foreach (var VARIABLE in GameBoard.GetCurrentRealHexagonList()) {
            
            VARIABLE.setColor(Random.Range(1,6));
            if (VARIABLE.x == 1 && VARIABLE.y == 2) {
                VARIABLE.setColor(1);
            }
            
            if (VARIABLE.x == 0 && VARIABLE.y == 2) {
                VARIABLE.setColor(1);
            }
            
            if (VARIABLE.x == 0 && VARIABLE.y == 1) {
                VARIABLE.setColor(1);
            }
            
            if (VARIABLE.x == 1 && VARIABLE.y == 0) {
                VARIABLE.setColor(1);
            }
            
            if (VARIABLE.x == 2 && VARIABLE.y == 2) {
                VARIABLE.setColor(1);
            }
            
        }
    }

    public static void saveHexagonColorMap() {
        return;
        
        sw = File.AppendText("C:\\Test.txt");
        Debug.Log("ColorSetterForTesting.saveHexagonColorMap();");
        //Pass the filepath and filename to the StreamWriter Constructor
        
        //Write a line of text
        sw.Write(" colors : ");
        foreach (var VARIABLE in GameBoard.GetCurrentRealHexagonList()) {
            sw.Write(VARIABLE.color+", ");
        }
        sw.Write("\n");
        
        sw.Dispose();
        
    }
    
    

    // Update is called once per frame
    void Update()
    {
    
    }
}
