//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Texture2D terrainMap;

    public Texture2D obstacleMap;

    public ColorToPrefab[] colorTerrainMappings;

    public ColorToPrefab[] colorObstacleMappings;

    public ColorToPrefab[] colorEnemyMappings;

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
        
        for (int x = obstacleMap.width-1; x >= 0; x--) // reversed to draw trees closer last
        {
            for (int y = obstacleMap.height-1; y >= 0; y--)
            {
                GenerateObstacle(x, y);
            }
        }

        
        for (int x = obstacleMap.width - 1; x >= 0; x--)
        {
            for (int y = obstacleMap.height - 1; y >= 0; y--)
            {
                GenerateEnemy(x, y);
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
            //if(colorTerrainMapping.color.Equals(pixelColor))
            if(IsEqualColor(colorTerrainMapping.color, pixelColor))
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

        
        if (pixelColor.a == 0) // The pixel is transparent.
        {
            return;
        }
        

       //Debug.Log(ColorUtility.ToHtmlStringRGBA(pixelColor) + " x: " + x + " y: " + y);

        foreach (ColorToPrefab colorObstacleMapping in colorObstacleMappings)
        {
            //if (colorObstacleMapping.color.Equals(pixelColor))
            if(IsEqualColor(colorObstacleMapping.color,pixelColor))
            {
                //Vector2 position = new Vector2(x * 10f / fXObstacleTerrainRatio , y * 10f / fYObstacleTerrainRatio);
                Vector2 position = new Vector2(x * 10f / fXObstacleTerrainRatio, y * 10f / fYObstacleTerrainRatio);

                Instantiate(colorObstacleMapping.prefab, position, Quaternion.identity, transform);

                //Debug.Log(colorObstacleMapping.prefab);
            }
            /*
            else
            {
                Debug.Log(" pixelColor: " + pixelColor + " colorObstacleMapping: " + colorObstacleMapping.color);
                Debug.Log(pixelColor.r + " == " + colorObstacleMapping.color.r);
                Debug.Log(pixelColor.g + " == " + colorObstacleMapping.color.g);
                Debug.Log(pixelColor.b + " == " + colorObstacleMapping.color.b);
                Debug.Log(pixelColor.a + " == " + colorObstacleMapping.color.a);


            }
            */
        }
    }

    void GenerateEnemy(int x, int y)
    {
        float fXObstacleTerrainRatio = obstacleMap.width / terrainMap.width;
        float fYObstacleTerrainRatio = obstacleMap.height / terrainMap.height;

        Color pixelColor = obstacleMap.GetPixel(x, y);


        if (pixelColor.a == 0) // The pixel is transparent.
        {
            return;
        }

        foreach (ColorToPrefab colorEnemyMapping in colorEnemyMappings)
        {
            if (IsEqualColor(colorEnemyMapping.color, pixelColor))
            {
                Vector2 position = new Vector2(x * 10f / fXObstacleTerrainRatio, y * 10f / fYObstacleTerrainRatio);

                Instantiate(colorEnemyMapping.prefab, position, Quaternion.identity, transform);
                Debug.Log("Enemy found");
            }
            else
                Debug.Log(" pixelColor: " + pixelColor + " colorEnemyMapping: " + colorEnemyMapping.color);
        }
    }

    private bool IsEqualColor(Color x, Color y)
    {
        int i, j, k, l;
        i = (int)(100 * x.r);
        j = (int)(100 * x.g);
        k = (int)(100 * x.b);
        l = (int)(100 * x.a);

        int p, q, r, s;
        p = (int)(100 * y.r);
        q = (int)(100 * y.g);
        r = (int)(100 * y.b);
        s = (int)(100 * y.a);

        return i == p && j == q && k == r && l == s;
    }

    /*
    public static void Main(string[] args)
    {
        GenerateLevel();
    }
    */
}
