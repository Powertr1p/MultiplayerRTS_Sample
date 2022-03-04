using System;
using Combat;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace Units
{
    public class Unit : NetworkBehaviour
    {
        [SerializeField] private UnitMovement _unitMovement;
        [SerializeField] private Targeter _targeter;
        [SerializeField] private UnityEvent _onSelected;
        [SerializeField] private UnityEvent _onDeselected;

        public static event Action<Unit> ServerOnUnitSpawn;
        public static event Action<Unit> ServerOnUnitDespawn;
        public static event Action<Unit> AuthorityOnUnitSpawned;
        public static event Action<Unit> AuthorityOnUnitDespawned;

        public UnitMovement GetUnitMovement => _unitMovement;
        public Targeter GetTargeter => _targeter;

    #region Server

        public override void OnStartServer()
        {
            ServerOnUnitSpawn?.Invoke(this);
        }

        public override void OnStopServer()
        {
            ServerOnUnitDespawn?.Invoke(this);
        }

    #endregion
        
   #region Client

       public override void OnStartClient()
       {
           if (!isClientOnly || !hasAuthority) return;
           
           AuthorityOnUnitSpawned?.Invoke(this);
       }

       public override void OnStopClient()
       {
           if (!isClientOnly || !hasAuthority) return;
           
           AuthorityOnUnitDespawned?.Invoke(this);
       }

       [Client]

        public void Select()
        {
            if (!hasAuthority) return;

            _onSelected?.Invoke();
        }

        [Client]
        public void Deselect()
        {
            if (!hasAuthority) return;
            
            _onDeselected?.Invoke();
        }

    #endregion
    }
}