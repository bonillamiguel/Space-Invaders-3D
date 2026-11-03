using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class Spaceship : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject dartPrefab;

    public static float shotCooldown = .05f;
    public float lastShot = 0;
    public float speed = 10f;
    public float edgePadding = 1.3f;
    public int lives = 3;
    public float invulnerableTime = 1.5f;
    public float flashInterval = .1f;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public TextMeshProUGUI gameOverText;
    public GameMenu gameMenu;

    float invulnerableUntil = 0;
    Renderer[] renderers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameOverText != null)
        {
            gameOverText.text = "";
        }
        UpdateHearts();
        renderers = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        if (Keyboard.current.rightArrowKey.isPressed)
        {
            Vector3 position = transform.position;
            position.x += speed * Time.deltaTime;
            transform.position = position;
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            Vector3 position = transform.position;
            position.x -= speed * Time.deltaTime;
            transform.position = position;
        }

        // Keep the ship on screen, recalculated each frame in case the window is resized
        Camera cam = Camera.main;
        float limit = cam.orthographicSize * cam.aspect - edgePadding;
        Vector3 clamped = transform.position;
        clamped.x = Mathf.Clamp(clamped.x, cam.transform.position.x - limit, cam.transform.position.x + limit);
        transform.position = clamped;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && Time.time - lastShot > shotCooldown)
        {
            Vector3 position = transform.position;
            GameObject bullet = Instantiate<GameObject>(dartPrefab);
            bullet.transform.position = position;
            lastShot = Time.time;
        }
    }

    public void LoseLife()
    {
        if (Time.time < invulnerableUntil)
        {
            return;
        }

        lives--;
        invulnerableUntil = Time.time + invulnerableTime;
        UpdateHearts();

        if (lives > 0)
        {
            StartCoroutine(DamageFlash());
        }
        else
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        if (gameOverText != null)
        {
            gameOverText.text = "Game Over";
        }
        if (gameMenu != null)
        {
            gameMenu.ShowGameOver();
        }
        Time.timeScale = 0;
        Destroy(gameObject);
    }

    public void Win()
    {
        // Stop any damage flash so the ship isn't frozen while hidden
        StopAllCoroutines();
        SetVisible(true);

        if (gameOverText != null)
        {
            gameOverText.text = "You Win!";
        }
        if (gameMenu != null)
        {
            gameMenu.ShowGameOver();
        }
        Time.timeScale = 0;
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < lives ? fullHeart : emptyHeart;
        }
    }

    IEnumerator DamageFlash()
    {
        bool visible = false;
        while (Time.time < invulnerableUntil)
        {
            SetVisible(visible);
            visible = !visible;
            yield return new WaitForSeconds(flashInterval);
        }
        SetVisible(true);
    }

    void SetVisible(bool visible)
    {
        foreach (Renderer shipRenderer in renderers)
        {
            shipRenderer.enabled = visible;
        }
    }

}


