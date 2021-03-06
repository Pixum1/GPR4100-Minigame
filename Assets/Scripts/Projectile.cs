using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Damage { get { return m_damage; } }
    public GameObject ObjectThatShot { get { return objectThatShot; } }
    [SerializeField]
    private float m_damage; //the damage the projectile does on impact
    private Rigidbody m_rigidbody; //rigidbody
    private GameObject objectThatShot; //the GameObject that launched the projectile

    private Noise noiseObj;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();

        noiseObj = GetComponent<Noise>();
    }
    private void Start()
    {
        noiseObj.StartCoroutine(noiseObj.MakeNoise());
    }
    private void FixedUpdate()
    {
        if (m_rigidbody.velocity.sqrMagnitude > 0)
        {
            transform.forward = m_rigidbody.velocity;
        }
    }
    private void OnCollisionEnter(Collision _other)
    {
        //if anything else than the guards vision hitbox was hit
        if (!_other.gameObject.CompareTag("Ignore Projectile"))
        {
            DeactivateProjectile();
        }
    }
    public void Launch(Vector3 _impulse, float _lifeSpan, GameObject _whoShot)
    {
        objectThatShot = _whoShot; //set object that hot equal to the given object
        m_rigidbody.AddForce(_impulse, ForceMode.Impulse); //launch the projectile
        Destroy(gameObject, _lifeSpan); //destroy the projectile after X seconds
    }
    private void OnDestroy()
    {

    }

    private void DeactivateProjectile()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        if (GetComponent<ParticleSystem>().time <= 0f)
        {
            Destroy(gameObject);
        }

    }

}
