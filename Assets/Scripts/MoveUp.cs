using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : MonoBehaviour
{
    Vector3 movement;
    private float speed = 1f;
    GameObject topLine;

    // Start is called before the first frame update
    void Start()
    {
        movement.y = speed;
        topLine = GameObject.Find("TopLine");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position += movement * Time.deltaTime;
        if (transform.position.y >= topLine.transform.position.y)
        {
            Destroy(gameObject);
        }
    }

}
