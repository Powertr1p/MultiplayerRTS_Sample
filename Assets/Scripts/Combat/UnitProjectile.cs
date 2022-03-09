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

        private void Start()
        {
            _rigidbody.velocity = transform.forward * _launchForce;
        }

        public override void OnStartServer()
        {
            //Invoke(nameof(DestroySelf), _destroyAfterSeconds);
            StartCoroutine(DelayDestroy());
        }

        [Server]
        private void DestroySelf()
        {
            NetworkServer.Destroy(gameObject);
        }

        private IEnumerator DelayDestroy()
        {
            yield return new WaitForSeconds(_destroyAfterSeconds);
            DestroySelf();
        }
    }
}