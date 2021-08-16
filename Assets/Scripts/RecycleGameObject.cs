using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRecycle {
    void Restart();
    void Shutdown();
}

public class RecycleGameObject : MonoBehaviour {
    private List<IRecycle> recycleComponents;
    //RECYCLE GAME OBJECTS are not destroy just inactive when die,if player play again same level they gonna active
    void Awake() {
        var components = GetComponents<MonoBehaviour>();
        recycleComponents = new List<IRecycle>();
        foreach (var comp in components) {
            if (comp is IRecycle) {
                recycleComponents.Add(comp as IRecycle);
            }
        }
    }

    private ParticleSystem ps;
 
    ////////////////////////////////////////////////////////////////
 
    void Start () {
        ps = GetComponent<ParticleSystem>();
    }
 
    void Update () {
        if(ps)
        {
            if(!ps.IsAlive())
            {
                Shutdown();
            }
        }
    }
    
    
    
    
    public void Restart() {
        gameObject.SetActive(true);

        foreach (var comp in recycleComponents) {
            comp.Restart();
        }
    }

    public void Shutdown() {
        gameObject.SetActive(false);

        foreach (var comp in recycleComponents) {
            comp.Shutdown();
        }
    }
}