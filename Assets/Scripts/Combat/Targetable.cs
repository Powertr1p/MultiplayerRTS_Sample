using Mirror;
using UnityEngine;

namespace Combat
{
    public class Targetable : NetworkBehaviour
    {
        [SerializeField] private Transform _aimAtPoint;

        public Transform AimAtPoint => _aimAtPoint;
    }
}
