using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateAround : MonoBehaviour {

    public GameObject from;
    public GameObject target;
    private  float cycleStepTime = 0.33f;
    private  float _currentSec = 0;
    private  float delay = 0.03f;
    void Update()
    {
        
        
    }

    private void Start() {
        StartCoroutine(goTo(target));
    }
    
    

    IEnumerator goTo(GameObject target) {
        
        yield return new WaitForSeconds(delay);
        _currentSec += delay;
        
        transform.RotateAround(target.transform.position, Vector3.back, 10);
       
       
        if (_currentSec < cycleStepTime) {
            StartCoroutine(goTo(target));
        }else {
            
            StopCoroutine(goTo(target));
        }
    }
}
