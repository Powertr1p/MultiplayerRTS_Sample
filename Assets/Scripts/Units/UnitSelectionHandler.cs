using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Units
{
    public class UnitSelectionHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask _layer;
        
        private Camera _camera;

        private List<Unit> _selectedUnits = new List<Unit>();
        
        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                foreach (var selectedUnit in _selectedUnits)
                    selectedUnit.Deselect(); 
                
                _selectedUnits.Clear();
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                ClearSelectionArea();
            }
        }

        private void ClearSelectionArea()
        {
            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layer)) return;
            if (!hit.collider.TryGetComponent(out Unit unit)) return;
            if (!unit.hasAuthority) return;
            
            _selectedUnits.Add(unit);

            foreach (var selectedUnit in _selectedUnits)
                selectedUnit.Select();
        }
    }
}