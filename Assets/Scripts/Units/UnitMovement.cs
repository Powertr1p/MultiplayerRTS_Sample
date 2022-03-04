using Combat;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    public class UnitMovement : NetworkBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Targeter _targeter;
        [SerializeField] private float _chaseRange = 5;
        
    #region Server

        [ServerCallback]
        private void Update()
        {
            Targetable target = _targeter.Target;
            
            if (!ReferenceEquals(target, null))
            {
                ChaseTarget(target);
                return;
            }
            
            if (!_navMeshAgent.hasPath) return;
            if (_navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance) return;
            
            _navMeshAgent.ResetPath();
        }

        private void ChaseTarget(Targetable target)
        {
            if ((target.transform.position - transform.position).sqrMagnitude > _chaseRange * _chaseRange)
                _navMeshAgent.SetDestination(target.transform.position);
            else if (_navMeshAgent.hasPath)
                _navMeshAgent.ResetPath();
        }

        [Command]
        public void CmdMove(Vector3 position)
        {
            _targeter.ClearTarget();
            
            if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) return;
        
            _navMeshAgent.SetDestination(position);
        }
    
    #endregion
    }
}
