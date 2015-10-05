using UnityEngine;
using System.Collections;

/*
 * This script keeps track of things like how much light the player still has and what type of light the player is.
 * 
 * Think of light like health. However, the player doesn't necessarily die when he/she runs out of light. They just can't
 * create any new light sources and possibly won't be able to see the rest of the level, so it would most likely be
 * game-over.
 */

// all the different types of light we could potentially put in the game
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
    // all colors used throughout the game for CONSISTENCY!
    public static Color color_red = new Color(1.0f, 0f, 0f);
    public static Color color_orange = new Color(1.0f, 0.35f, 0f);
    public static Color color_yellow = new Color(1.0f, 1.0f, 0f);
    public static Color color_green = new Color(0f, 1.0f, 0f);
    public static Color color_blue = new Color(0f, 0.35f, 1f);
    public static Color color_violet = new Color(1.0f, 0f, 1.0f);
    public static Color color_white = new Color(1.0f, 1.0f, 1.0f);

    [SerializeField]
    public TypeOfLight playerLightType;
    [SerializeField]
    public float maxLight;

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

    // use this function to change how much light the player still has by a percentage of maxLight
    public void incrLightByPrcnt(float percent)
    {
        float incr = percent * maxLight;
        lightLeft += incr; //modify light value
        if (lightLeft < 0) lightLeft = 0;
        else if (lightLeft > maxLight) lightLeft = maxLight;
    }

    public void changeLightType(TypeOfLight newLightType)
    {
        playerLightType = newLightType;
        switch (newLightType)
        {
            case TypeOfLight.White:
                playerLight.color = color_white;
                break;
            case TypeOfLight.Red:
                playerLight.color = color_red;
                break;
            case TypeOfLight.Orange:
                playerLight.color = color_orange;
                break;
            case TypeOfLight.Yellow:
                playerLight.color = color_yellow;
                break;
            case TypeOfLight.Green:
                playerLight.color = color_green;
                break;
            case TypeOfLight.Blue:
                playerLight.color = color_blue;
                break;
            case TypeOfLight.Violet:
                playerLight.color = color_violet;
                break;
            default:
                Debug.Log("changeLightType switch defaulted");
                playerLight.color = color_white;
                break;
        }
    }

    //Getter for player light type
    public TypeOfLight getLightType()
    {
        return playerLightType;
    }
}
