using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarController : MonoBehaviour
{
    public GameObject pivotLine;
    public GameObject enemy;
    public GameObject ally;
    public GameObject[] enemyCheckpoints;
    public GameObject[] allyCheckpoints;


    private SpriteRenderer enemySprite;
    private SpriteRenderer allySprite;

    private int currentAllyCheckpoint = 0;
    private int currentEnemyCheckpoint = 0;
    private int nextEnemyCheckpoint = 0;
    private int nextAllyCheckpoint = 0;


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
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == enemy)
        {
            if(HasChangedCheckpoint(false))
                MoveShips(false);

            enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 1f);
        }
        else
        {
            if (HasChangedCheckpoint(true))
                MoveShips(true);

            allySprite.color = new Color(allySprite.color.r, allySprite.color.g, allySprite.color.b, 1f);
        }
    }

    public void MoveShips(bool isAlly)
    {
        if(isAlly)
        {
            ally.transform.position = allyCheckpoints[currentAllyCheckpoint].transform.position;
            switch (currentAllyCheckpoint)
            {
                case 0:

                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                default:
                    Debug.LogWarning("NON EXISTENT CHECKPOINT! : " + currentAllyCheckpoint);
                    break;
            }
        }
        else
        {
            enemy.transform.position = enemyCheckpoints[currentEnemyCheckpoint].transform.position;
            switch (currentEnemyCheckpoint)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                default:
                    Debug.LogWarning("NON EXISTENT CHECKPOINT! : " + currentEnemyCheckpoint);
                    break;
            }
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

    }
}
