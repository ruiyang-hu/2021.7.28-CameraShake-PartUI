using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    Animator animator;
    new Collider2D collider;
    public LayerMask checkLayer;
    public GameObject fireAttack;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (collider.IsTouchingLayers(checkLayer))
        {
            animator.SetBool("isHit", true);
        }

        if (animator.GetBool("fireOn"))
        {
            fireAttack.SetActive(true);
        }
        else
        {
            fireAttack.SetActive(false);
        }
    }

    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        animator.SetBool("isHit", true);
    //    }
    //}

    //private void OnCollisionStay2D(Collision2D other)
    //{
    //    if(other.gameObject.tag == "Player")
    //    {
    //        animator.SetBool("isHit", true);
    //    }
    //}

    private void HitEnd()
    {
        Invoke("SetFireOn", 1f);
    }

    private void SetFireOn()
    {
        animator.SetBool("fireOn", true);
    }

    private void TurnOff()
    {
        animator.SetBool("fireOn", false);
        animator.SetBool("isHit", false);
    }
}
