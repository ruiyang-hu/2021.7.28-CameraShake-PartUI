using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private int spawnCount = 0;
    private int haveFlag;

    public List<GameObject> platforms = new List<GameObject>();
    public List<GameObject> groundObjects = new List<GameObject>();

    public float spawnTime;
    private float countTimeP;
    private float countTimeG;
    private Vector3 spawnPosition;
    private Vector3 brickSpawnPosition = new Vector3(-4.75f, -9.5f, 0f);
    private Vector3 flagPosition = new Vector3(-4f, -8.3f, 0f);
    private int spikeNum = 0;

    // Update is called once per frame
    void Update()
    {
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
        haveFlag = Random.Range(1, 2);
        //释放地板砖快
        for (int i = 0; i < 20; i++)
        {
            ground_brick.Add(PoolManager.Release(groundObjects[0], brickSpawnPosition + Vector3.right * i * 0.5f));
            spawnCount++;
        }
        //释放旗子
        if(haveFlag == 1)
        {
            System.Random r = new System.Random();
            int temp = (r.Next() & 2) - 1;
            var newFlag = createFlag(temp);
            int removeStartLeft = Random.Range(2, 6);
            int removeStartRight = Random.Range(12, 15);

            if (temp == -1)
            {
                for (int i = 0; i < 4 - Random.Range(0, 2); i++)
                {
                    ground_brick[removeStartRight + i].SetActive(false);
                    createBrickOff(removeStartRight + i, newFlag);
                }
            }
            else
            {
                for (int i = 0; i < 4 - Random.Range(0, 2); i++)
                {
                    ground_brick[removeStartLeft + i].SetActive(false);
                    createBrickOff(removeStartLeft + i, newFlag);
                }
            }
        }
        else
        {
            int removeStart = Random.Range(0, 17);
            for (int i = 0; i < 4 - Random.Range(0, 2); i++)
            {
                ground_brick[removeStart + i].SetActive(false);
            }
        }
        
    }

    public GameObject createFlag(int temp)
    {
        var newFlag = PoolManager.Release(groundObjects[2], flagPosition + new Vector3(4, 0, 0) * (temp + 1), Quaternion.identity, new Vector3(-temp, 1, 1));
        return newFlag;
    }

    public void createBrickOff(int removeStart, GameObject parent)
    {
        var newBrickOff = PoolManager.Release(groundObjects[1], brickSpawnPosition + Vector3.right * 0.5f * removeStart, parent);
    }
}
