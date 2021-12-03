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

        public List<Unit> SelectedUnits { get; } = new List<Unit>();
        
        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                foreach (var selectedUnit in SelectedUnits)
                    selectedUnit.Deselect(); 
                
                SelectedUnits.Clear();
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
            
            SelectedUnits.Add(unit);

            foreach (var selectedUnit in SelectedUnits)
                selectedUnit.Select();
        }
    }
}