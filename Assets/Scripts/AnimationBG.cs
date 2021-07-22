using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBG : MonoBehaviour
{
    Material material;
    Vector2 movement;
    private Vector2 speed = Vector2.down * 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        movement += speed * Time.deltaTime;
        material.mainTextureOffset = movement;
    }
}
