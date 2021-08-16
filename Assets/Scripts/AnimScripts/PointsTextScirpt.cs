using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsTextScirpt : MonoBehaviour
{
    private TextMeshPro myText;
    public float delay = 0f;
    
    // Use this for initialization
    private void Awake() {
        myText = GetComponent<TextMeshPro>();
    }

    void Start () {
        StartCoroutine(DelayedShutdownRecycle(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length));
    }

    public void SetValue(int value, Color32 color) {
        myText.text = value.ToString();
        myText.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, color);
    }
    
    IEnumerator DelayedShutdownRecycle(float delay) {
        var recycleGameObject = gameObject.GetComponent<RecycleGameObject>();
        yield return new WaitForSeconds(delay);
        recycleGameObject.Shutdown();
    }
    
    private void OnEnable() {
        Start();
    }
    
}
