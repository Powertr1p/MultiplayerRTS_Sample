using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Buildings
{
    public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject _unitPrefab;
        [SerializeField] private Transform _unitSpawnPoint;
    
    #region Server

        [Command]
        private void CmdSpawnUnit()
        {
            var instance = Instantiate(_unitPrefab, _unitSpawnPoint.position, _unitSpawnPoint.rotation);
            NetworkServer.Spawn(instance, connectionToClient);
        }
        
    #endregion

   #region Client

       public void OnPointerClick(PointerEventData eventData)
       {
           if (eventData.button != PointerEventData.InputButton.Left) return;
           if (!hasAuthority) return;
           
           CmdSpawnUnit();
       }
       
    #endregion
    }
}
