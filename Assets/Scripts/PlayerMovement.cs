using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float playerSpeed;
    [SerializeField] float lookSpeed;

    [SerializeField] float minPlayerLook;
    [SerializeField] float maxPlayerLook;

    Vector2 movement;
    Vector2 mouseDelta;
    Rigidbody rb;
    Camera playerCam;

    float xRotation = 0f;
    float yRotation = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerCam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = mouseDelta.x * lookSpeed * Time.deltaTime;
        float mouseY = mouseDelta.y * lookSpeed * Time.deltaTime;

        //Debug.Log($"MouseX : {mouseX}, MouseY : {mouseY}");

        //Debug.Log(Time.deltaTime);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxPlayerLook, minPlayerLook);

        yRotation += mouseX;
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.localRotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void FixedUpdate()
    {
        Vector3 moveVec = transform.right * movement.x * playerSpeed * Time.fixedDeltaTime + transform.forward * movement.y * playerSpeed * Time.fixedDeltaTime;

        rb.velocity = moveVec;

    }

    public void OnMove(InputAction.CallbackContext ctx) => movement = ctx.ReadValue<Vector2>();

    public void OnMouseMove(InputAction.CallbackContext ctx) => mouseDelta = ctx.ReadValue<Vector2>();

    public void OnFirePrime()
    {
        int layerMask = 1 << 8 | 1 << 10;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        //Physics.Raycast(mainCamera.transform.position, mainCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask);

        Physics.Raycast(new Vector3(playerCam.transform.position.x, playerCam.transform.position.y, playerCam.transform.position.z), playerCam.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask);


        //Debug.DrawRay(new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z), mainCamera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

        
        GetComponentInChildren<IGun>().Fire(hit);
        
    }
}
