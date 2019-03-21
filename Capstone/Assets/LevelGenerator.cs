﻿//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Texture2D map;

    public ColorToPrefab[] colorMappings;

    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        for(int x = 0; x < map.width; ++x) // may need to be x++
        {
            for(int y = 0; y < map.height; ++y)
            {
                GenerateTile(x, y);
            }
        }
    }

    void GenerateTile(int x, int y)
    {
        Color pixelColor = map.GetPixel(x, y);

        if(pixelColor.a == 0) // The pixel is transparent.
        {
            return;
        }

        Debug.Log(pixelColor);

        foreach(ColorToPrefab colorMapping in colorMappings)
        {
            if(colorMapping.color.Equals(pixelColor))
            {
                Vector2 position = new Vector2(x*5.12f, y*5.12f);
                Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
            }
        }
    }

}
