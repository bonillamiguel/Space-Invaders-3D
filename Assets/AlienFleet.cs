using UnityEngine;
using System.Collections.Generic;

public class AlienFleet : MonoBehaviour
{
    [Header("Inscribed")]

    public GameObject alienPrefab;

    public List<GameObject> aliens = new List<GameObject> { };

    float speed = .1f;
    public float maxSpeed = .4f;
    public float startShotsPerSecond = 3f;
    public float maxShotsPerSecond = 6f;
    [HideInInspector] public float alienFireChance;
    int startingAlienCount;
    int direction = 1;

    public float edgePadding = 1f;
    public float invasionMargin = 1.3f;
    float downTime = .1f;
    bool movingDown = false;
    Spaceship spaceship;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject alien = Instantiate<GameObject>(alienPrefab);
                alien.transform.position = new Vector3(2 * i - 12, 10 - 2 * j, 0);
                aliens.Add(alien);
                alien.GetComponent<Alien>().fleet = this;
            }
        }
        startingAlienCount = aliens.Count;
        spaceship = FindAnyObjectByType<Spaceship>();

    }

    void FixedUpdate()
    {
        if (aliens.Count == 0)
        {
            if (spaceship != null)
            {
                spaceship.Win();
            }
            return;
        }

        float progress = 1 - aliens.Count / (float)startingAlienCount;
        float currentSpeed = Mathf.Lerp(speed, maxSpeed, progress);

        // Split the fleet's total fire rate between the aliens that are left
        float shotsPerSecond = Mathf.Lerp(startShotsPerSecond, maxShotsPerSecond, progress);
        alienFireChance = shotsPerSecond * Time.fixedDeltaTime / aliens.Count;

        //move all aliens
        foreach (GameObject alien in aliens)
        {
            Vector3 position = alien.transform.position;
            if (!movingDown)
            {
                position.x += direction * currentSpeed;
            }
            else
            {
                position.y -= currentSpeed;
            }
            alien.transform.position = position;
        }

        //change alien direction
        float left = 0, right = 0, lowest = Mathf.Infinity;
        foreach (GameObject alien in aliens)
        {
            Vector3 position = alien.transform.position;
            if (position.y < lowest)
            {
                lowest = position.y;
            }
            if (position.x > right)
            {
                right = position.x;
            }
            if (position.x < left)
            {
                left = position.x;
            }
        }

        // Turn at the screen edges, recalculated in case the window is resized
        Camera cam = Camera.main;
        float limit = cam.orthographicSize * cam.aspect - edgePadding;
        float leftBound = cam.transform.position.x - limit;
        float rightBound = cam.transform.position.x + limit;
        if (direction > 0 && right >= rightBound || direction < 0 && left <= leftBound)
        {
            direction = -direction;
            movingDown = true;
            Invoke("startSidewaysMotion", downTime);
        }

        // Darts only fly upward from the ship, so the game is lost once an alien gets this close to its height
        if (spaceship != null && lowest <= spaceship.transform.position.y + invasionMargin)
        {
            spaceship.GameOver();
        }
    }

    void startSidewaysMotion()
    {
        movingDown = false;
    }
}
