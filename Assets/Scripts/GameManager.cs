using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public WaveSpawner waveSpawner;
    private GameObject crystal;

    void Update(){
        crystal = GameObject.FindGameObjectWithTag("Crystal");
        if(crystal != null){
            waveSpawner.isGameStarted = true;
        }
        else{
            waveSpawner.isGameStarted=false;
            SceneManager.LoadScene(0);
        }
    }
}
