using System.Collections.Generic;
using Mirror;
using Units;
using UnityEngine;

namespace Networking
{
    public class RTSPlayer : NetworkBehaviour
    {
        [SerializeField] private List<Unit> _units = new List<Unit>();

    #region Server
    
        public override void OnStartServer()
        {
            Unit.ServerOnUnitSpawn += ServerHandleUnitSpawned;
            Unit.ServerOnUnitDespawn += ServerHandleUnitDespawned;
        }

        public override void OnStopServer()
        {
            Unit.ServerOnUnitDespawn -= ServerHandleUnitSpawned;
            Unit.ServerOnUnitDespawn -= ServerHandleUnitDespawned;
        }

        private void ServerHandleUnitSpawned(Unit unit)
        {
            if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;
            
            _units.Add(unit);
        }
        
        private void ServerHandleUnitDespawned(Unit unit)
        {
            if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;
            
            _units.Remove(unit);
        }

    #endregion

    #region Client

        public override void OnStartClient()
        {
            if (!isClientOnly) return;
            
            Unit.AuthorityOnUnitDespawned += AuthorityHandleUnitSpawned;
            Unit.AuthorityOnUnitDespawned += AuthorityHandleUnitDespawn;
        }

        public override void OnStopClient()
        {
            Unit.AuthorityOnUnitDespawned -= AuthorityHandleUnitSpawned;
            Unit.AuthorityOnUnitDespawned -= AuthorityHandleUnitDespawn;
        }

        private void AuthorityHandleUnitSpawned(Unit unit)
        {
            if (!hasAuthority) return;
            
            _units.Add(unit);
        }

        private void AuthorityHandleUnitDespawn(Unit unit)
        {
            if (!hasAuthority) return;
            
            _units.Remove(unit);
        }

    #endregion
    }
}