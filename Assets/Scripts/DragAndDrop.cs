using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] Rigidbody2D mouse;

    PlayerInput input;
    Camera camera;
    Vector2 mousePos;

    UIObject heldObject;
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();
        camera = Camera.main;
        heldObject = null;
    }

    private void Update()
    {
        //Debug.Log(camera.WorldToScreenPoint(mousePos));
        mouse.MovePosition(camera.ScreenToWorldPoint(mousePos));
    }

    public void OnMousePosition(InputAction.CallbackContext ctx) => mousePos = ctx.ReadValue<Vector2>();

    public void OnFirePrime(InputAction.CallbackContext context)
    {
        
        if (context.phase == InputActionPhase.Started)
        {
            List<Collider2D> results = new List<Collider2D>();

            ContactFilter2D contactFilter = new ContactFilter2D();
            mouse.OverlapCollider(contactFilter, results);

           

            foreach (Collider2D c in results)
            {
                Debug.Log(c.tag);
                if (c.CompareTag("UIElement"))
                {
                    heldObject = c.GetComponent<UIObject>();
                    
                }
               
            }

            if (heldObject != null)
            {
                HingeJoint2D j = mouse.GetComponent<HingeJoint2D>();
                heldObject.Detach();
                j.connectedBody = heldObject.GetComponent<Rigidbody2D>();
            }
            

            
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            if (heldObject == null) return;
            HingeJoint2D j = mouse.GetComponent<HingeJoint2D>();

            heldObject.Attach();

            j.connectedBody = null;
            heldObject = null;
            

            

            Debug.Log("Let go!");
        }
    }

    
}
