﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ReceiveDamage : NetworkBehaviour
{
    [SerializeField]
    private int maxHealth = 10;

    [SyncVar]
    private int currentHealth;

    [SerializeField]
    private string damageTag;

    [SerializeField]
    private bool destroyOnDeath;

    private Vector2 initialPosition;

    // Use this for initialization
    void Start()
    {
        this.currentHealth = this.maxHealth;

        this.initialPosition = this.transform.position;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == this.damageTag)
        {
            this.TakeDamage(1);

            Destroy(collider.gameObject);
        }
    }

    void TakeDamage(int amount)
    {
        if (this.isServer)
        {
            this.currentHealth -= amount;
            if (this.currentHealth <= 0)
            {
                if (this.destroyOnDeath)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    this.currentHealth = this.maxHealth;
                    RpcRespawn();
                }
            }
        }
    }

    [ClientRpc]
    void RpcRespawn()
    {
        this.transform.position = this.initialPosition;
    }
}
