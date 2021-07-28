using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartController : MonoBehaviour
{
    private Animator animator;
    new private Collider2D collider;
    public LayerMask checkLayer;
   
    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        FlagTouch();
    }

    public void FlagTouch()
    {
        if (collider.IsTouchingLayers(checkLayer))
        {
            animator.SetTrigger("FlagOut");
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
