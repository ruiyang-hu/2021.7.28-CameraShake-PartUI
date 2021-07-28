using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : MonoBehaviour
{
    Vector3 movement;
    public float moveUpSpeed;
    GameObject topLine;

    // Start is called before the first frame update
    void Start()
    {
        topLine = GameObject.Find("TopLine");
    }

    // Update is called once per frame
    void Update()
    {
        //moveUpSpeed = (int)GameManager.gameTime / (int)GameManager.levelTime * 0.2f + 1;
        moveUpSpeed = (GameManager.currentLevel - 1) * 0.2f + 1;
        movement.y = moveUpSpeed;
        Move();
    }

    void Move()
    {
        transform.position += movement * Time.deltaTime;
        if (transform.position.y >= topLine.transform.position.y)
        {
            gameObject.SetActive(false);
        }
    }

}
