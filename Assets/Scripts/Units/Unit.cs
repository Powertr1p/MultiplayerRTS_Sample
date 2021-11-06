using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace Units
{
    public class Unit : NetworkBehaviour
    {
        [SerializeField] private UnityEvent _onSelected;
        [SerializeField] private UnityEvent _onDeselected;

    #region Client

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