using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpParticle : MonoBehaviour
{
    public PlayerController playerController;
    new ParticleSystem particleSystem;
    bool p = false;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isOnGround)
        {
            p = true;
            if (particleSystem.isPlaying)
            {
                p = false;
            }
        }
        
        if (p)
        {
            particleSystem.Play();
        }
    }
}
