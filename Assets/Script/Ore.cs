using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    [SerializeField] private float price;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.npc))
        {
            Player.instance.playerPrice += price;
            Destroy(this.gameObject);
        }
    }
}
