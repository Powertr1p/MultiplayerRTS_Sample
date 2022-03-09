using System;
using Mirror;
using UnityEngine;

namespace Combat
{
    public class UnitFiring : NetworkBehaviour
    {
        [SerializeField] private Targeter _targeter;
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private Transform _projectileSpawnPoint;
        [SerializeField] private float _fireRange = 3f;
        [SerializeField] private float _fireRate = 1f;
        [SerializeField] private float _rotationSpeed = 20f;

        private float _lastFireTime = 0;

        [ServerCallback]
        private void Update()
        {
            if (ReferenceEquals(_targeter.Target, null)) return;
            if (!CanFireAtTarget()) return;

            Quaternion targetRotation = Quaternion.LookRotation(_targeter.Target.transform.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            if (Time.time > (1 / _fireRate) + _lastFireTime)
            {
                Quaternion projectileRotation = Quaternion.LookRotation(_targeter.Target.AimAtPoint.position - _projectileSpawnPoint.position);
                GameObject projectileInstance = Instantiate(_projectilePrefab, _projectileSpawnPoint.position, projectileRotation);
                
                NetworkServer.Spawn(projectileInstance, connectionToClient);
                
                _lastFireTime = Time.time;
            }
        }

        [Server]
        private bool CanFireAtTarget()
        {
            Targetable target = _targeter.Target;
            return (target.transform.position - transform.position).sqrMagnitude <= _fireRange * _fireRange;
        }
    }
}
