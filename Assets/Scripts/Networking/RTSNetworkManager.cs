using Mirror;
using UnityEngine;

namespace Networking
{
    public class RTSNetworkManager : NetworkManager
    {
        [SerializeField] private GameObject _unitSpawnerPrefab;
        
        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            base.OnServerAddPlayer(conn);
            
            var instance = Instantiate(_unitSpawnerPrefab, GetStartPosition().position, GetStartPosition().rotation);
            
            NetworkServer.Spawn(instance, conn);
        }
    }
}
