using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> platforms = new List<GameObject>();
    public List<GameObject> groundObjects = new List<GameObject>();
    public LayerMask checkLayer;

    public float spawnTime;
    private float countTimeP;
    private float countTimeG;
    private Vector3 spawnPosition;
    private Vector3 brickSpawnPosition = new Vector3(-4.75f, -7.75f, 0f);
    private Vector3 flagPosition = new Vector3(-4f, -6.55f, 0f);
    private int spikeNum = 0;

    // Update is called once per frame
    void Update()
    {
        //SpawnPlatform();
        SpawnGround();
    }

    public void SpawnPlatform()
    {
        countTimeP += Time.deltaTime;
        spawnPosition = transform.position;
        spawnPosition.x = Random.Range(-3.5f, 3.5f);

        if(countTimeP >= spawnTime)
        {
            createPlatform();
            countTimeP = 0;
        }
    }

    public void SpawnGround()
    {
        countTimeG += Time.deltaTime;

        if (countTimeG >= spawnTime)
        {
            createGround();
            countTimeG = 0;
        }
    }

    public void createPlatform()
    {
        int index = Random.Range(0, platforms.Count);
        if(index == 4)
        {
            spikeNum++;
        }
        if(spikeNum > 1)
        {
            index = Random.Range(0, platforms.Count - 1);
            spikeNum = 0;
        }

        GameObject newPlatform = Instantiate(platforms[index], spawnPosition, Quaternion.identity);
        newPlatform.transform.SetParent(this.gameObject.transform);
    }

    public void createGround()
    {
        List<GameObject> ground_brick = new List<GameObject>();
        List<GameObject> ground_brickOff = new List<GameObject>();
        for (int i = 0; i < 20; i++)
        {
            GameObject newBrick = Instantiate(groundObjects[0], brickSpawnPosition + Vector3.right * i * 0.5f, Quaternion.identity);
            ground_brick.Add(newBrick);
            GameObject newBrickOff = Instantiate(groundObjects[1], brickSpawnPosition + Vector3.right * i * 0.5f, Quaternion.identity);
            newBrickOff.SetActive(false);
            ground_brickOff.Add(newBrickOff);
        }

        System.Random r = new System.Random();
        int temp = (r.Next() & 2) - 1;
        GameObject newFlag = Instantiate(groundObjects[2], flagPosition + new Vector3(4, 0, 0) * (temp + 1), Quaternion.identity);
        newFlag.transform.localScale = new Vector3(-temp, newFlag.transform.localScale.y, newFlag.transform.localScale.z);
        StartController sc = newFlag.gameObject.GetComponent<StartController>();

        int removeStartLeft = Random.Range(2, 6);
        int removeStartRight = Random.Range(12, 15);
        
        if (temp == -1)
        {
            for (int i = 0; i < 4; i++)
            {
                ground_brick[removeStartRight + i].SetActive(false);
                GameObject newBrickOff = Instantiate(groundObjects[1], brickSpawnPosition + Vector3.right * 0.5f * removeStartRight * (i + 1), Quaternion.identity);
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                ground_brick[removeStartLeft + i].SetActive(false);
                GameObject newBrickOff = Instantiate(groundObjects[1], brickSpawnPosition + Vector3.right * 0.5f * removeStartLeft * (i + 1), Quaternion.identity);
            }
        }

        if (sc.flagTouched)
        {
            foreach (var item in ground_brickOff)
            {
                item.SetActive(false);
            }
        }

    }

    public void createFlag()
    {
        System.Random r = new System.Random();
        int temp = (r.Next() & 2) - 1;
        GameObject newFlag = Instantiate(groundObjects[0], flagPosition + new Vector3(4, 0, 0) * (temp + 1), Quaternion.identity);
        newFlag.transform.localScale = new Vector3(-temp, newFlag.transform.localScale.y, newFlag.transform.localScale.z);
    }
}
