using Mirror;
using UnityEngine;

namespace Combat
{
    public class Targeter : NetworkBehaviour
    {
        public Targetable Target => _target;

        private Targetable _target;

    #region Server
        
        [Command]
        public void CmdSetTarget(GameObject targetGameObject)
        {
            if (!targetGameObject.TryGetComponent(out Targetable target)) return;

            _target = target;
        }

        [Server]
        public void ClearTarget()
        {
            _target = null;
        }
        
    #endregion
        
    }
}
