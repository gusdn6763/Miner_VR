using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenOre : Ore
{
    [SerializeField] private ParticleSystem particle;
    public List<Ore> ore = new List<Ore>();

    private bool isQuitting = false;
    public bool possible = false;

    public void Start()
    {
        
        StartCoroutine(CheckGround());
    }

    IEnumerator CheckGround()
    {
        yield return new WaitForSeconds(1f);
        possible = true;
    }

    public void BrokenObject()
    {
        particle.Play();
        for (int i = 0; i < ore.Count; i++)
        {
            Instantiate(ore[i], transform.position, Quaternion.identity);
        }
    }
}
