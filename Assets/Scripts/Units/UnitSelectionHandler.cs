using System;
using System.Collections.Generic;
using Mirror;
using Networking;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Units
{
    public class UnitSelectionHandler : MonoBehaviour
    {
        [SerializeField] private RectTransform _unitSelectionArea;
        [SerializeField] private LayerMask _layer;

        private Vector2 _startPosition;
        
        private RTSPlayer _player;
        private Camera _camera;

        public List<Unit> SelectedUnits { get; } = new List<Unit>();
        
        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (ReferenceEquals(_player, null))
            {
                _player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
            }
            
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                StartSelectionArea();
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                ClearSelectionArea();
            }
            else if (Mouse.current.leftButton.isPressed)
            {
                UpdateSelectionArea();
            }
        }

        private void StartSelectionArea()
        {
            if (!Keyboard.current.leftShiftKey.isPressed)
            {
                foreach (var selectedUnit in SelectedUnits)
                    selectedUnit.Deselect(); 
                
                SelectedUnits.Clear();
            }
            
            _unitSelectionArea.gameObject.SetActive(true);
            _startPosition = Mouse.current.position.ReadValue();
            UpdateSelectionArea();
        }

        private void UpdateSelectionArea()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            float areaWidth = mousePosition.x - _startPosition.x;
            float areaHight = mousePosition.y - _startPosition.y;

            _unitSelectionArea.sizeDelta = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areaHight));
            _unitSelectionArea.anchoredPosition = _startPosition + new Vector2(areaWidth / 2, areaHight / 2);
        }

        private void ClearSelectionArea()
        {
            _unitSelectionArea.gameObject.SetActive(false);

            if (_unitSelectionArea.sizeDelta.magnitude == 0)
            {
                Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layer)) return;
                if (!hit.collider.TryGetComponent(out Unit unit)) return;
                if (!unit.hasAuthority) return;
            
                SelectedUnits.Add(unit);

                foreach (var selectedUnit in SelectedUnits)
                    selectedUnit.Select();
                
                return;
            }

            Vector2 min = _unitSelectionArea.anchoredPosition - _unitSelectionArea.sizeDelta / 2;
            Vector2 max = _unitSelectionArea.anchoredPosition + _unitSelectionArea.sizeDelta / 2;

            foreach (var unit in _player.GetUnits)
            {
                if (SelectedUnits.Contains(unit)) continue;
                
                Vector3 screenPosition = _camera.WorldToScreenPoint(unit.transform.position);

                if (screenPosition.x > min.x && screenPosition.x < max.x && screenPosition.y > min.y && screenPosition.y < max.y)
                {
                    SelectedUnits.Add(unit);
                    unit.Select();
                }
            }
        }
    }
}