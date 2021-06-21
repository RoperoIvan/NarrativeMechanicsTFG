using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarController : MonoBehaviour
{
    public GameObject pivotLine;
    public GameObject enemy;
    public GameObject ally;
    public GameManager gameManager;
    public GameObject[] enemyCheckpoints;
    public GameObject[] allyCheckpoints;
    public AudioSource uiAS;

    private AudioClip radarPulse;
    private SpriteRenderer enemySprite;
    private SpriteRenderer allySprite;

    private int currentAllyCheckpoint = 0;
    private int currentEnemyCheckpoint = 0;
    private int nextEnemyCheckpoint = 0;
    private int nextAllyCheckpoint = 0;


    private void Awake()
    {
        radarPulse = Resources.Load<AudioClip>("Sound/pulseRadar");
    }
    // Start is called before the first frame update
    void Start()
    {
        enemySprite = enemy.GetComponent<SpriteRenderer>();
        allySprite = ally.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        pivotLine.transform.rotation *= Quaternion.Euler(new Vector3(0f, 0f, +100f*Time.deltaTime));
        enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, enemySprite.color.a-0.2f * Time.deltaTime);
        allySprite.color = new Color(allySprite.color.r, allySprite.color.g, allySprite.color.b, allySprite.color.a - 0.2f * Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.M))
        {
            GoToNextCheckpoint(false, true);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            GoToNextCheckpoint(false, false);

        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            GoToNextCheckpoint(true, true);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            GoToNextCheckpoint(true, false);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == enemy)
        {
            if(HasChangedCheckpoint(false))
                MoveShips(false);

            uiAS.PlayOneShot(radarPulse,0.5f);
            enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 1f);
        }
        else
        {
            if (HasChangedCheckpoint(true))
                MoveShips(true);

            uiAS.PlayOneShot(radarPulse, 0.5f);
            allySprite.color = new Color(allySprite.color.r, allySprite.color.g, allySprite.color.b, 1f);
        }
    }

    public void MoveShips(bool isAlly)
    {
        if(isAlly)
        {
            if(currentAllyCheckpoint >= 0 && currentAllyCheckpoint < 6)
                ally.transform.position = allyCheckpoints[currentAllyCheckpoint].transform.position;
        }
        else
        {
            if (currentEnemyCheckpoint >= 0 && currentEnemyCheckpoint < 6)
                enemy.transform.position = enemyCheckpoints[currentEnemyCheckpoint].transform.position;
        }
    }
    private bool HasChangedCheckpoint(bool isAlly)
    {
        if(isAlly)
        {
            if (currentAllyCheckpoint != nextAllyCheckpoint)
            {
                currentAllyCheckpoint = nextAllyCheckpoint;
                return true;
            }
            else
                return false;
        }
        else
        {
            if (currentEnemyCheckpoint != nextEnemyCheckpoint)
            {
                currentEnemyCheckpoint = nextEnemyCheckpoint;
                return true;
            }
            else
                return false;
        }
    }

    public void GoToNextCheckpoint(bool isAlly, bool isForward)
    {
        if(isAlly)
        {
            if(isForward)
            {
                nextAllyCheckpoint++;
            }
            else
            {
                nextAllyCheckpoint--;
            }
        }
        else
        {
            if (isForward)
            {
                nextEnemyCheckpoint++;
            }
            else
            {
                nextEnemyCheckpoint--;
            }
        }

        if(nextEnemyCheckpoint == 6) //END WAR IS OVER
        {
            GameManager.isEnd = true;
            gameManager.ExecuteFinal(0);
        }

        if (nextAllyCheckpoint == 6) // END WAR IS ON
        {
            GameManager.isEnd = true;
            gameManager.ExecuteFinal(3);
        }

    }

    void LaunchMissile() // Launch Missile END
    {
        GameManager.isEnd = true;
    }
}
