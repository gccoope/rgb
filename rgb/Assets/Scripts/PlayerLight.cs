﻿using UnityEngine;
using System.Collections;

/*
 * This script keeps track of things like how much light the player still has and what type of light the player is.
 * 
 * Think of light like health. However, the player doesn't necessarily die when he/she runs out of light. They just can't
 * create any new light sources and possibly won't be able to see the rest of the level, so it would most likely be
 * game-over.
 */

public enum TypeOfLight
{
    Infrared,
    White, // white light can be thought of as all the different colors? Maybe a cool mechanic around that?
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Violet,
    Ultraviolet,
    XRay,
    GammaRay
}

[RequireComponent(typeof(Light))]

public class PlayerLight : MonoBehaviour
{
    // all the different types of light we could potentially put in the game


    [SerializeField]
    public TypeOfLight playerLightType;
    [SerializeField]
    float maxLight;

    public float lightLeft;
    private Light playerLight;
    private float startLightIntensity; // the intensity of the player light when the player is initially spawned

    // Use this for initialization
    void Start()
    {
        playerLightType = TypeOfLight.White; // the player is intilized as white light
        playerLight = gameObject.GetComponent<Light>();

        startLightIntensity = playerLight.intensity;

        lightLeft = maxLight;
    }

    // Update is called once per frame
    void Update()
    {
        playerLight.intensity = (lightLeft / maxLight) * startLightIntensity;
    }

    // use this function to change how much light the player still has
    public void incrementLight(float incr)
    {
        lightLeft += incr;
        if (lightLeft < 0) lightLeft = 0;
        else if (lightLeft > maxLight) lightLeft = maxLight;
    }
}