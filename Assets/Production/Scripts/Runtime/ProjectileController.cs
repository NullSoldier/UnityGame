using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour, ITriggerable
{

    public Vector3 Direction = Vector3.zero;
    public float Speed = 0f;
    public float MaxDistance = 50f;
    public int Damage = 1;

    private Vector3 startedAt;

    void Start()
    {
        startedAt = transform.position;
    }
	
	void Update()
	{
	    transform.position += Direction * Speed;

	    float distanceTraveled = Vector3.Distance(startedAt, transform.position);

	    if (distanceTraveled >= MaxDistance)
	        killProjectile();
	}

    public void OnTrigger(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            killProjectile();
        }
        else if (other.tag == "Enemy")
        {
            var enemy = other.gameObject.GetComponent<GruntController>();

            if (!enemy.IsDead)
            {
                enemy.TakeDamage(Damage);
                killProjectile();
            }
        }
        else if (other.tag == "Generator")
        {
            var generator = other.gameObject.GetComponent<EnemyGenerator>();

            if (!generator.IsDead)
            {
                generator.TakeDamage(Damage);
                killProjectile();
            }
        }
    }

    private void killProjectile()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
