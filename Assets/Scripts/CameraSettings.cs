using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettings : MonoBehaviour {
    
    public static float defaultYPos = -10f;
    
    public static void ReStartCam() {
        //Camera.main.transform.position = getMidPointOfHexagons();
        var xDiff = getDiffX() / 2;
        var yDiff = getDiffY() / 2;
        FindObjectOfType<GameBoard>().transform.localPosition = new Vector3(-xDiff,-yDiff,0);
        Debug.LogWarning("local pos is:"+ FindObjectOfType<GameBoard>().transform.localPosition);
    }
    
    private static float getDiffX() {
        
        var first = GameBoard.getBoardHexagon(0, 0);
        var last = GameBoard.getBoardHexagon(GameSettings.column-1, GameSettings.row-1);
        
        var xdiff =(Camera.main.WorldToScreenPoint(last.transform.position) -
         Camera.main.WorldToScreenPoint(first.transform.position)).x;

        Debug.LogWarning("xdiff = "+xdiff);
        return xdiff;
    }
    
    private static float getDiffY() {
        
        var first = GameBoard.getBoardHexagon(0, 0);
        var last = GameBoard.getBoardHexagon(GameSettings.column-1, GameSettings.row-1);
        
        var ydiff =(Camera.main.WorldToScreenPoint(last.transform.position) -
                    Camera.main.WorldToScreenPoint(first.transform.position)).y;
        
        Debug.LogWarning("ydiff = "+ydiff);
        return ydiff+38;
    }
    
    
    
    private static Vector3 getMidPointOfHexagons() {
        
        var first = GameBoard.getBoardHexagon(0, 0);
        var last = GameBoard.getBoardHexagon(GameSettings.column-1, GameSettings.row-1);

        var midX = first.transform.position.x + (first.transform.position.x - last.transform.position.x) / 2;
        var midY = first.transform.position.y + (first.transform.position.y - last.transform.position.y) / 2;
        
        Debug.LogWarning("mid point is :"+ new Vector3(-midX, -midY, defaultYPos));
        
        return new Vector3(-midX, -midY, defaultYPos);
    }

}
