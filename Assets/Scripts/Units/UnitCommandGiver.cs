using System;
using Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Units
{
    public class UnitCommandGiver : MonoBehaviour
    {
        [SerializeField] private UnitSelectionHandler _unitSelectionHandler;
        [SerializeField] private LayerMask _layerMask;

        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (!Mouse.current.rightButton.wasPressedThisFrame) return;

            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerMask)) return;

            if (hit.collider.TryGetComponent(out Targetable target))
            {
                if (target.hasAuthority)
                {
                    TryMove(hit.point);
                    return;
                }

                TryTarget(target);
                return;
            }
            
            TryMove(hit.point);
        }

        private void TryMove(Vector3 position)
        {
            foreach (var unit in _unitSelectionHandler.SelectedUnits)
            {
                unit.GetUnitMovement.CmdMove(position);
            }
        }
        
        private void TryTarget(Targetable target)
        {
            foreach (var unit in _unitSelectionHandler.SelectedUnits)
            {
                unit.GetTargeter.CmdSetTarget(target.gameObject);
            }
        }
    }
}