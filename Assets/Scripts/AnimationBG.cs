using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBG : MonoBehaviour
{
    Material material;
    Vector2 movement;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        speed = ((GameManager.currentLevel - 1) * 0.2f + 1) * 0.5f;
        movement.y -= speed * Time.deltaTime;
        material.mainTextureOffset = movement;
    }
}
