using System;
using System.Collections;
using Mirror;
using UnityEngine;

namespace Combat
{
    public class UnitProjectile : NetworkBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _launchForce = 10f;
        [SerializeField] private float _destroyAfterSeconds = 5f;
        [SerializeField] private int _damageToDeal = 20;

        private void Start()
        {
            _rigidbody.velocity = transform.forward * _launchForce;
        }

        public override void OnStartServer()
        {
            StartCoroutine(DelayDestroy());
        }

        [ServerCallback]
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<NetworkIdentity>(out NetworkIdentity networkIdentity))
                if (networkIdentity.connectionToClient == connectionToClient) return;

            if (other.TryGetComponent<Health>(out Health health))
                health.DealDamage(_damageToDeal);
            
            DestroySelf();
        }

        [Server]
        private void DestroySelf()
        {
            NetworkServer.Destroy(gameObject);
        }

        [Server]
        private IEnumerator DelayDestroy()
        {
            yield return new WaitForSeconds(_destroyAfterSeconds);
            DestroySelf();
        }
    }
}