using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] private Transform mainObstacleObject;
    [SerializeField] private Transform fracturedObstacleObject;


    private static readonly float explosionForce = 500f;
    private static readonly float explosionRadius = 200f;


    private List<Rigidbody> objectParticles = new List<Rigidbody>();
    public event Action ExplodeEvent;

    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        SetRigidbodysKinematic(true);
    }
    private void Start()
    {

        //Get Particles' Rigidbodys to a list
        FindParticles();
        ExplodeEvent += ExplodeObstacle;

    }

    private void FindParticles()
    {
        foreach (Rigidbody parca in fracturedObstacleObject.GetComponentsInChildren<Rigidbody>())
        {
            objectParticles.Add(parca);
            parca.gameObject.SetActive(false);
        }
    }



    #region Explosion Releated Stuff

    [ContextMenu("Explode Me")]
    public void ExplodeMe()
    {
        ExplodeEvent?.Invoke();
    }



    private void ExplodeObstacle()
    {
        
        Vector3 explosionPos = transform.position + (Vector3.down * 0.5f);
        foreach (var particle in objectParticles)
        {
            GetReadyForExplosion(particle.gameObject);
            particle.AddExplosionForce(explosionForce, explosionPos, explosionRadius);
        }


        //mainObstacleObject.gameObject.SetActive(false);
        Utilities.DestroyItemAfterTime(this.gameObject, 0f);
    }




    private void GetReadyForExplosion(GameObject particle)
    {
        //particle.SetActive(true);
        Utilities.SetActiveAfterTime(true, particle, 0f);
        particle.transform.parent = null;
        Utilities.DestroyItemAfterTime(particle, 2f);
    }


    #endregion


    public void GiveMeForce(Vector3 direction, float force)
    {
        rb.AddForce(direction * force, ForceMode.Impulse);
    }





    bool soundPlayed;
    bool activated;
    private void OnCollisionEnter(Collision collision)
    {
        if (!activated && !collision.gameObject.CompareTag("Ground"))
        {
            SetRigidbodysKinematic(false);
            activated = true;
        }
        if(!soundPlayed && collision.transform.CompareTag("Player"))
        {
            SoundManager.instance.PlaySound(Soundlar.PickUp);
            soundPlayed = true;
        }
    }

    public void SetRigidbodysKinematic(bool status)
    {
        rb.isKinematic = status;
    }


}
