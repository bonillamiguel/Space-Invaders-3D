using UnityEngine;

public class Alien : MonoBehaviour
{
    [Header("Inscribed")]

    public GameObject bulletPrefab;
    public AlienFleet fleet;
    
    public float lastShot = 0;
    public static float shotCooldown = .1f;

    void FixedUpdate()
    {
        Vector3 position = transform.position;
        
        if(UnityEngine.Random.value < fleet.alienFireChance && Time.time - lastShot > shotCooldown){
            GameObject bullet = Instantiate<GameObject>(bulletPrefab);
            bullet.transform.position = position;
            lastShot = Time.time;
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        fleet.aliens.Remove(gameObject);
        Destroy(gameObject);

        Spaceship ship = coll.gameObject.GetComponent<Spaceship>();
        if (ship != null)
        {
            ship.LoseLife();
        }
        else
        {
            Destroy(coll.gameObject);
        }
    }
}
