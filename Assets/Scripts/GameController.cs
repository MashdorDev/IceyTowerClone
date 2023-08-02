using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject player;
    public GameObject pillarPrefab; // Prefab for the pillars

    private Vector3 screenBounds;
    private Vector3 lastPlatformPosition; // The position of the last spawned platform
    private float originalPlatformWidth; // The original width of the platform

    public float velocityThreshold = 10f;

    public float pillarHeight; // The height of the pillars

    private List<GameObject> platforms = new List<GameObject>();
    private List<GameObject> pillars = new List<GameObject>();

    void Start()
    {
        screenBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.transform.position.z));

        // Get the original width of the platform
        originalPlatformWidth = platformPrefab.GetComponent<Renderer>().bounds.size.x;

        pillarHeight = pillarPrefab.GetComponent<BoxCollider2D>().size.y;

        // Spawn base platform and pillars
        SpawnBase();

        // Spawn initial platforms
        SpawnPlatforms();
    }

    void Update()
    {
        Vector3 playerPosition = player.transform.position;

        // If the player is close to the last platform, spawn new platforms
        if (lastPlatformPosition.y - playerPosition.y < 2f) // Change 2f to whatever distance you want
        {
            SpawnPlatforms();
        }

        // Remove platforms and pillars that are below the screen
        for (int i = platforms.Count - 1; i >= 0; i--)
        {
            if (platforms[i].transform.position.y < player.transform.position.y - screenBounds.y)
            {
                Destroy(platforms[i]);
                platforms.RemoveAt(i);
            }
        }

        for (int i = pillars.Count - 1; i >= 0; i--)
        {
            if (pillars[i].transform.position.y < player.transform.position.y - screenBounds.y)
            {
                Destroy(pillars[i]);
                pillars.RemoveAt(i);
            }
        }
    }



    private void SpawnBase()
    {
        // Spawn base platform
        GameObject basePlatform = Instantiate(platformPrefab, player.transform.position, Quaternion.identity);
        basePlatform.transform.localScale = new Vector3(originalPlatformWidth, basePlatform.transform.localScale.y, basePlatform.transform.localScale.z);

        // Spawn pillars on the left and right
        GameObject leftPillar = Instantiate(pillarPrefab, new Vector3(-screenBounds.x, player.transform.position.y, 0), Quaternion.identity);
        GameObject rightPillar = Instantiate(pillarPrefab, new Vector3(screenBounds.x, player.transform.position.y, 0), Quaternion.identity);

        pillars.Add(leftPillar);
        pillars.Add(rightPillar);
    }

    private void SpawnPlatforms()
    {
        // Determine the number of platforms to spawn
        int numPlatforms = Random.Range(3, 6);

        // Determine the y position of the first platform to spawn
        float yPos = Mathf.Max(player.transform.position.y + screenBounds.y, lastPlatformPosition.y + pillarHeight);

        for (int i = 0; i < numPlatforms; i++)
        {
            // Determine the x position and width of the platform
            float xPos = Random.Range(-screenBounds.x, screenBounds.x);
            float width = Random.Range(0.1f, 1f);

            // Create a new platform
            GameObject platform = Instantiate(platformPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
            platform.transform.localScale = new Vector3(width, platform.transform.localScale.y, platform.transform.localScale.z);

            // Add the platform to the list of platforms
            platforms.Add(platform);

            // Update the last platform position
            lastPlatformPosition = platform.transform.position;

            // Increase the y position for the next platform
            yPos += pillarHeight;

            // Spawn pillars on the left and right
            GameObject leftPillar = Instantiate(pillarPrefab, new Vector3(-screenBounds.x, yPos, 0), Quaternion.identity);
            GameObject rightPillar = Instantiate(pillarPrefab, new Vector3(screenBounds.x, yPos, 0), Quaternion.identity);

            pillars.Add(leftPillar);
            pillars.Add(rightPillar);
        }
    }
}
