using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour {

    public static CameraHandler Instance { get; private set; }

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private Collider2D confiner;

    private float orthographicSize;
    private float targetOrthographicSize;
    private bool edgeScrolling;

    private void Awake() {
        Instance = this;

        edgeScrolling = PlayerPrefs.GetInt("edgeScrolling", 1) == 1;
    }

    private void Start() {
        orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        targetOrthographicSize = orthographicSize;
    }

    private void Update() {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        
        if (edgeScrolling) {
            float edgeScrollingSize = 20;
            if (Input.mousePosition.x > Screen.width - edgeScrollingSize) {
                x = 1f;
            }
            if (Input.mousePosition.x < edgeScrollingSize) {
                x = -1f;
            }
            if (Input.mousePosition.y > Screen.height - edgeScrollingSize) {
                y = 1f;
            }
            if (Input.mousePosition.y < edgeScrollingSize) {
                y = -1f;
            }
        }
        


        Vector3 moveDir = new Vector3(x, y).normalized;
        float moveSpeed = 30f;

        Vector3 newCameraPosition = transform.position + moveDir * moveSpeed * Time.deltaTime;

        if (confiner.bounds.Contains(newCameraPosition)) {
            transform.position = newCameraPosition;
        } 

        
    }

    private void HandleZoom() {
        // Takes the mouse scroll data and adds some speed
        float zoomAmount = 2f;
        targetOrthographicSize += -Input.mouseScrollDelta.y * zoomAmount;

        //Makes sure the size is within bounds
        float minOrtographicSize = 10;
        float maxOrtographicSize = 30;
        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minOrtographicSize, maxOrtographicSize);

        // Changes the size of the camera and makes it smooth
        float zoomSpeed = 5f;
        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);

        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
    }

    public void SetEdgeScrolling(bool edgeScrolling) {
        this.edgeScrolling = edgeScrolling;

        PlayerPrefs.SetInt("edgeScrolling", edgeScrolling ? 1 : 0);
    }

    public bool GetEdgeScrolling() {
        return edgeScrolling;
    }
}
