using System;
using UnityEngine;
using UnityEngine.Serialization;

public class UIInterface : MonoBehaviour {
    private GameObject focusObject;
    [SerializeField] private GameObject rocketTurretPrefab;
    private Camera mainCamera;

    private void Start() {
        mainCamera = Camera.main;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) { // if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition); // Input.GetTouch(0).position
            if (!Physics.Raycast(ray, out var hit)) {
                return;
            }

            focusObject = Instantiate(rocketTurretPrefab, hit.point, rocketTurretPrefab.transform.rotation);
            var focusCollider = focusObject.GetComponentInChildren<Collider>();
            if (focusCollider) {
                focusCollider.enabled = false;
            }
        } else if (focusObject && Input.GetMouseButton(0)) { // if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit)) {
                return;
            }

            focusObject.transform.position = hit.point;
        } else if (focusObject && Input.GetMouseButtonUp(0)) { // if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit))
                return;
            var collidedObject = hit.collider.gameObject;
            if (collidedObject.CompareTag("EmptyPlatform") && hit.normal == Vector3.up) {
                var collidedPosition = collidedObject.transform.position;
                focusObject.transform.position = new Vector3(collidedPosition.x, focusObject.transform.position.y, collidedPosition.z);
                collidedObject.tag = "OccupiedPlatform";
            } else {
                Destroy(focusObject);
            }

            focusObject = null;
        }
    }
}
