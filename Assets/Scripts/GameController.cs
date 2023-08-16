using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject player;
    public GameObject pillarPrefab;
    public GameObject backgroundPrefab;


    private Vector3 screenBounds;
    private Vector3 lastPlatformPosition;
    private Vector3 firstPlatform;
    private float originalPlatformWidth;

    public float velocityThreshold = 10f;

    public float pillarHeight;

    private List<GameObject> platforms = new List<GameObject>();
    private List<GameObject> pillars = new List<GameObject>();
    private List<GameObject> backgrounds = new List<GameObject>();

    void Start()
    {
        screenBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.transform.position.z));
        originalPlatformWidth = platformPrefab.GetComponent<Renderer>().bounds.size.x;
        pillarHeight = pillarPrefab.GetComponent<BoxCollider2D>().size.y;


        SpawnBase();
        SpawnPlatforms();
    }

    void Update(){
        if(player == null){
            return;
        }

        Vector3 playerPosition = player.transform.position;

        if (lastPlatformPosition.y - playerPosition.y < -1f){
            SpawnPlatforms();
        }

        if(Mathf.Abs(firstPlatform.y - playerPosition.y) > 50f){
            removePlatforms();
        }
    }



    private void SpawnBase(){
        GameObject basePlatform = Instantiate(platformPrefab, new Vector3(player.transform.position.x, player.transform.position.y, -1), Quaternion.identity);
        basePlatform.transform.localScale = new Vector3(originalPlatformWidth, basePlatform.transform.localScale.y, basePlatform.transform.localScale.z);

        GameObject leftPillar = Instantiate(pillarPrefab, new Vector3(-screenBounds.x, player.transform.position.y, 1), Quaternion.identity);
        GameObject rightPillar = Instantiate(pillarPrefab, new Vector3(screenBounds.x, player.transform.position.y, 1), Quaternion.identity);

        leftPillar.transform.localScale = new Vector3(leftPillar.transform.localScale.x, 3, leftPillar.transform.localScale.z);
        rightPillar.transform.localScale = new Vector3(rightPillar.transform.localScale.x, 3, rightPillar.transform.localScale.z);

        pillars.Add(leftPillar);
        pillars.Add(rightPillar);

        GameObject background = Instantiate(backgroundPrefab, new Vector3(0, player.transform.position.y + 9, 10), Quaternion.identity);
        backgrounds.Add(background);
    }

    private void SpawnPlatforms(){
        int numPlatforms = Random.Range(3, 6);
        float yPos = Mathf.Max(player.transform.position.y + screenBounds.y, lastPlatformPosition.y + pillarHeight);

        for (int i = 0; i < numPlatforms; i++)
        {
            float width = Random.Range(0.1f, 1f);
            float halfPlatformWidth = width * originalPlatformWidth / 2;

            float xPos = Random.Range(-screenBounds.x + halfPlatformWidth, screenBounds.x - halfPlatformWidth);

            GameObject platform = Instantiate(platformPrefab, new Vector3(xPos, yPos, -1), Quaternion.identity);
            platform.transform.localScale = new Vector3(width, platform.transform.localScale.y, platform.transform.localScale.z);

            platforms.Add(platform);
            lastPlatformPosition = platform.transform.position;
            if (i == 0)
            {
                firstPlatform = platform.transform.position;
            }

            yPos += platform.GetComponent<Renderer>().bounds.size.y;

            GameObject leftPillar = Instantiate(pillarPrefab, new Vector3(-screenBounds.x, yPos + pillarHeight / 2, 1), Quaternion.identity);
            GameObject rightPillar = Instantiate(pillarPrefab, new Vector3(screenBounds.x, yPos + pillarHeight / 2, 1), Quaternion.identity);

            pillars.Add(leftPillar);
            pillars.Add(rightPillar);

            GameObject background = Instantiate(backgroundPrefab, new Vector3(0, yPos, 10), Quaternion.identity);
            backgrounds.Add(background);

            yPos += pillarHeight;
        }
    }




    private void removePlatforms(){
        float cameraBottomEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z)).y;

        for (int i = platforms.Count - 1; i >= 0; i--){
            if (platforms[i].transform.position.y < cameraBottomEdge - 4 )
            {
                Destroy(platforms[i]);
                platforms.RemoveAt(i);
            }
        }

        for (int i = pillars.Count - 1; i >= 0; i--){
            if (pillars[i].transform.position.y < cameraBottomEdge - 4 )
            {
                Destroy(pillars[i]);
                pillars.RemoveAt(i);
            }
        }

        for (int i = backgrounds.Count - 1; i >= 0; i--){
            if (backgrounds[i].transform.position.y < cameraBottomEdge - 4)
            {
                Destroy(backgrounds[i]);
                backgrounds.RemoveAt(i);
            }
        }
    }

}

