using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public float speed;
    public LayerMask boundLayer;
    public Transform checkPoint;
    public float checkRadius;
    //private Collider2D coll;
    private bool isCollided;
    private int direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        isCollided = Physics2D.OverlapCircle(checkPoint.position, checkRadius, boundLayer);
        if (isCollided)
        {
            direction = -direction;
            isCollided = false;
        }
        Movement();
    }

    private void Movement()
    {
        
        transform.position += Vector3.left * speed * direction * Time.fixedDeltaTime;

        transform.localScale = new Vector3(direction, 1, 1);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(checkPoint.position, checkRadius);
    }
}
