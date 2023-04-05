﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    const float width  = 3.7f;
    const float height = 7f;

    public float snakeSpeed = 1;

    public BodyPart   bodyPrefab = null;
    public GameObject rockPrefab = null;
    public GameObject EggPrefab = null;
    public GameObject GoldEggPrefab = null;
    public GameObject SpikePrefab = null;

    public Sprite tailSprite = null;
    public Sprite bodySprite = null;

    public SnakeHead snakeHead = null;

    public bool alive = true;

    public bool waitingToPlay = true;

    List<Egg> eggs = new List<Egg>();
    List<Spike> spikes = new List<Spike>();

    int level = 0;
    int noOfEggsForNextLevel = 0;

    public int score   = 0;
    public int hiScore = 0;

    public Text scoreText = null;
    public Text hiScoreText = null;

    public Text tapToPlayText = null;
    public Text gameOverText = null;
   

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Debug.Log("Starting Snake Game");
        CreateWalls();
        CreateSpike();
        KillOldSpikes();
        alive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitingToPlay)
        {
            foreach(Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Ended)
                {
                    StartGamePlay();
                }
            }
            if (Input.GetMouseButtonUp(0))
                StartGamePlay();
        }
    }    
    public void GameOver()
    {
        
        {
            alive = false;
            waitingToPlay = true;

            gameOverText.gameObject.SetActive(true);
            tapToPlayText.gameObject.SetActive(true);

        }
   
    }
    void StartGamePlay()
    {
        score = 0;
        level = 0;

        scoreText.text = "Score = "+score;
        hiScoreText.text = "HiScore = "+hiScore;

        gameOverText.gameObject.SetActive(false);
        tapToPlayText.gameObject.SetActive(false);

        waitingToPlay = false;
        alive = true;

        KillOldEggs();
        

        LevelUp();
    }
    void LevelUp()
    {

        if (level > 1)

        level++;

        noOfEggsForNextLevel = 4 + (level * 2);

        snakeSpeed = 1f + (level/4f);
        if (snakeSpeed>6) snakeSpeed=6;

        StartCoroutine(Reset5());

        snakeHead.ResetSnake();
        CreateEgg();
        
        
        
    }
    IEnumerator Reset5()
    {
        yield return new WaitforSeconds(0, 1f);
        snakeHead.ResetSnake();
        
    }

    public void EggEaten(Egg egg)
    {
        score++;

        noOfEggsForNextLevel--;
        if (noOfEggsForNextLevel==0)
        {
            score += 10;
            LevelUp();
        }          
        else if (noOfEggsForNextLevel==1) // last egg
            CreateEgg(true);
        else
            CreateEgg(false);

        if (score>hiScore)
        {
            hiScore = score;
            hiScoreText.text = "HiScore = "+hiScore;
        } 

        scoreText.text = "Score = "+score;

        eggs.Remove(egg);
        Destroy(egg.gameObject);
        
        

    }
    void CreateWalls()
    {
        float z = -1f;
        
        Vector3 start  = new Vector3(-width, -height, z);
        Vector3 finish = new Vector3(-width, +height, z);
        CreateWall(start, finish);

        start = new Vector3(+width, -height, z);
        finish = new Vector3(+width, +height, z);
        CreateWall(start, finish);

        start = new Vector3(-width, -height, z);
        finish = new Vector3(+width, -height, z);
        CreateWall(start, finish);

        start = new Vector3(-width, +height, z);
        finish = new Vector3(+width, +height, z);
        CreateWall(start, finish);
    }

    void CreateWall(Vector3 start, Vector3 finish)
    {
        float distance = Vector3.Distance(start,finish);
        int noOfRocks = (int)(distance * 3f);
        Vector3 delta = (finish-start)/noOfRocks;

        Vector3 position = start;
        for(int rock = 0; rock<=noOfRocks; rock++)
        {
            float rotation = Random.Range(0, 360f);
            float scale    = Random.Range(1.5f, 2f);
            CreateRock(position, scale, rotation);
            position = position + delta;
        }
    }

    void CreateRock(Vector3 position, float scale, float rotation)
    {
        GameObject rock = Instantiate(rockPrefab, position, Quaternion.Euler(0,0, rotation));
        rock.transform.localScale = new Vector3(scale,scale,1);
    }

    void CreateEgg(bool golden = false)
    {
        Vector3 EggPos;
        EggPos.x = -width + Random.Range(1f, (width*2)-2f);
        EggPos.y = -height + Random.Range(1f, (height * 2) - 2f);
        EggPos.z = -1f;
        
        Egg egg = null;
        if(golden)
        {
            egg = Instantiate(GoldEggPrefab, EggPos, Quaternion.identity).GetComponent<Egg>();
        }
        else
        {
            egg = Instantiate(EggPrefab, EggPos, Quaternion.identity).GetComponent<Egg>();
        }
        eggs.Add(egg);
    }

    void CreateSpike()
    { 
        Vector3 SpikePos;
        Spike spike  = null ;
       
           
        
        SpikePos.x = -width + Random.Range(1f, (width * 2) - 2f);
        
        SpikePos.y = -height + Random.Range(1f, (height * 2) - 2f);
       
        SpikePos.z = -1f;
        
        GameObject spikePrefab = Instantiate(SpikePrefab, SpikePos, Quaternion.identity);
        spikes.Add(spike);
       
           
    }
    void KillOldEggs()
    {
        foreach (Egg egg in eggs)
        {
            Destroy(egg.gameObject);
        }
        eggs.Clear();
    }
    void KillOldSpikes()
    {
        foreach (Spike spike in spikes)
        {
          

            
        }
        spikes.Clear();
    }

}
