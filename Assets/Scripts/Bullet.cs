using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Destroy settings")]
    public GameObject ExplosionPrefab;
    public int DistanceToDestroy;

    private GameObject Player;

    private void Awake()
    {
        Player = GameObject.Find("Bunny Holder");
    }

    // Destroy bullet if the distance from the player is greater than the given DistanceToDestroy
    private void FixedUpdate()
    {
        int distance = Mathf.FloorToInt(Mathf.Sqrt(Mathf.Pow(transform.position.x - Player.transform.position.x, 2) + Mathf.Pow(transform.position.z - Player.transform.position.z, 2)));
        if (distance > DistanceToDestroy) Destroy(gameObject);
    }

    // Destroy bullet on collision
    private void OnCollisionEnter(Collision collision)
    {
        GameObject explosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 3f);
        Destroy(gameObject);
    }
}
