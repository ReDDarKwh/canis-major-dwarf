using System;
using System.Collections;
using System.Collections.Generic;
using BayatGames.SaveGameFree;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private bool mouseDown;

    public LineRendererArrow lineRenderer;

    public Camera camera;

    public GameObject planet;

    public Transform start;
    public Transform end;

    public Text timeText;
    public Text triesText;

    public int tries = 0;
    public float startTry;
    public bool trying;
    public float tryTime;


    private float highscoreTime = 0;
    private int highscoreTries = int.MaxValue;

    public GameObject gameOverMenu;

    public Text highscoreText;
    

    // Start is called before the first frame update
    void Start()
    {

        highscoreTime = SaveGame.Exists("time")? SaveGame.Load<float>("time"): 0;
        highscoreTries= SaveGame.Exists("tries")? SaveGame.Load<int>("tries"): int.MaxValue;
        
    }


    private Vector3 getPos (){
        return  Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, 200 - Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !mouseDown){
            mouseDown = true;
            lineRenderer.ArrowOrigin = start.position;
        }

        if(Input.GetMouseButtonUp(0)){
            mouseDown = false;
          //  if(!trying){
                var pos = getPos();
                var p = Instantiate(planet, start.position, Quaternion.identity);
                var attractor = p.GetComponent<Attractor>();
                attractor.playerControl = this;
                attractor.startForce =  (start.position - getPos()) * 1.5f;
                trying = true;
                startTry = Time.time;
          //  }
        }
        
        if(mouseDown){
            lineRenderer.ArrowTarget = getPos();
        }

        timeText.text = trying? Mathf.Round(Time.time - startTry).ToString() : tryTime.ToString();
        triesText.text = tries.ToString();
    }

    internal void YEAHHH()
    {
        gameOverMenu.SetActive(true);

        highscoreText.text = $@"Time : {(highscoreTime = Time.time - startTry > highscoreTime?  Mathf.Round(Time.time - startTry): highscoreTime).ToString()} Tries { (highscoreTries = tries < highscoreTries?  tries: highscoreTries).ToString()}
        ";

        SaveGame.Save("tries", highscoreTries);
        SaveGame.Save("time", highscoreTime);
    }

    public void Reset(){
        tries = 0;
        SceneManager.LoadScene(0);
    }

    internal void planetDead()
    {
        tries ++;
        trying = false;

        tryTime = Mathf.Round(Time.time - startTry);
    }

    void FixedUpdate(){
        lineRenderer.cachedLineRenderer.enabled = mouseDown; 
    }
}
