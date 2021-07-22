using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineTransform : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.SetBool("go", true);
        }
    }

    private void Reset()
    {
        animator.SetBool("go", false);
    }
}
