//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Texture2D terrainMap;

    public Texture2D obstacleMap;

    public ColorToPrefab[] colorTerrainMappings;

    public ColorToPrefab[] colorObstacleMappings;

    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        for(int x = 0; x < terrainMap.width; x++) // may need to be x++
        {
            for(int y = 0; y < terrainMap.height; y++)
            {
                GenerateTerrainTile(x, y);
            }
        }

        for (int x = 0; x < obstacleMap.width; x++) // may need to be x++
        {
            for (int y = 0; y < obstacleMap.height; y++)
            {
                GenerateObstacle(x, y);
            }
        }
    }

    void GenerateTerrainTile(int x, int y)
    {
        Color pixelColor = terrainMap.GetPixel(x, y);

        
        if(pixelColor.a == 0) // The pixel is transparent.
        {
            return;
        }
        

        //Debug.Log(ColorUtility.ToHtmlStringRGB(pixelColor));

        foreach(ColorToPrefab colorTerrainMapping in colorTerrainMappings)
        {
            if(colorTerrainMapping.color.Equals(pixelColor))
            {
                Vector2 position = new Vector2(x*10f, y*10f);
                Instantiate(colorTerrainMapping.prefab, position, Quaternion.identity, transform);
            }
        }
    }

    void GenerateObstacle(int x, int y)
    {
        float fXObstacleTerrainRatio = obstacleMap.width / terrainMap.width;
        float fYObstacleTerrainRatio = obstacleMap.height / terrainMap.height;

        Color pixelColor = obstacleMap.GetPixel(x, y);

        /*
        if (pixelColor.a == 0) // The pixel is transparent.
        {
            return;
        }
        */

        Debug.Log(ColorUtility.ToHtmlStringRGB(pixelColor));

        foreach (ColorToPrefab colorObstacleMapping in colorObstacleMappings)
        {
            if (colorObstacleMapping.color.Equals(pixelColor))
            {
                Vector2 position = new Vector2(x * 10f / fXObstacleTerrainRatio , y * 10f / fYObstacleTerrainRatio);
                Instantiate(colorObstacleMapping.prefab, position, Quaternion.identity, transform);
            }
        }
    }

}
