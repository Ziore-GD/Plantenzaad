using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOnHit : MonoBehaviour
{
    private ParticleSystem _particle;
    private List<ParticleCollisionEvent> _collisionEvents;

    public delegate void OnHit (Transform trans);
    public OnHit OnHitDelegate;

    void Start ()
    {
        _collisionEvents = new List<ParticleCollisionEvent> ();
        _particle = GetComponent<ParticleSystem> ();
    }

    void OnParticleCollision (GameObject other)
    {
        int numCollisionEvents = ParticlePhysicsExtensions.GetCollisionEvents (_particle, other, _collisionEvents);

        int i = 0;
        while (i < numCollisionEvents)
        {
            OnHitDelegate (_collisionEvents[i].colliderComponent.transform);
            i++;
        }

    }
}