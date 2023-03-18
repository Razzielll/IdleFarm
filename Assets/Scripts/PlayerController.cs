using StarterAssets;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

public class PlayerController : MonoBehaviour
{
   // StarterAssetsInputs starterAssetsInputs;
    private Vector2 rawInput;
    
    private NavMeshAgent navMeshAgent;
    [SerializeField] float maxSpeed =5f;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] float stoppingDistance = 0.3f;
    [SerializeField] float cuttingRange = 0.3f;
    [SerializeField] bool idleControl = false;
    [SerializeField] FloatingJoystick joystick;
    [SerializeField] float moveRange = 10f;


    Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
       // starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimator();
        if (idleControl)
        {

            return;
        }
        Move();

    }

    private void Move()
    {
        
        rawInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        
        Vector3 moveInput = new Vector3(transform.position.x + moveRange* rawInput.x, 0, transform.position.z + moveRange* rawInput.y);
        Vector3 moveDirection = new Vector3(moveRange * rawInput.x, 0, moveRange * rawInput.y);
        if (moveDirection.magnitude >0)
        {
            
            transform.forward = moveDirection;
        }

        MoveTo(moveInput);
        UpdateAnimator();
        
    }

    public float GetCuttingRange()
    {
        return cuttingRange;
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.magnitude;
        GetComponent<Animator>().SetFloat("speed", speed);
    }
    public void MoveTo(Vector3 destination)
    {
        if(Vector3.Distance(destination,transform.position) > 0.5f)
        {
            transform.LookAt(destination);
        }
        
        navMeshAgent.destination = destination;
        navMeshAgent.speed = maxSpeed;
        navMeshAgent.isStopped = false;
    }

    public void CancelMoveAction()
    {
        navMeshAgent.isStopped = true;
    }

    void OnClick()
    {
        RaycastHit hit;
        bool hasHit = Physics.Raycast(GetPointerPosition(), out hit);
        if (!hasHit)
        {
            return;
        }
        if (hit.transform.GetComponent<IClickable>() == null)
        {
            return;
        }
        IClickable hitAction = hit.transform.GetComponent<IClickable>();

        ProcessInteraction(hitAction);
        

    }

    private void ProcessInteraction(IClickable hitAction)
    {
        if(cuttingRange < Vector3.Distance(transform.position, hitAction.GetPosition()))
        {
            return;
        }
        transform.LookAt(hitAction.GetPosition());
        
        bool interactable = hitAction.Interact();
        if (interactable)
        {
            animator.SetTrigger("Crop");
        }
    }
    //Animator Event
    void SwingStart()
    {
        StopCoroutine(DelayedWeaponStoved());
        StopAllCoroutines();
        GetComponentInChildren<Instrument>(true).gameObject.SetActive(true);
    }

    //Animator Event
    void CropStart()
    {
        Collider[] colliders = GetComponentInChildren<Instrument>(true).GetComponents<Collider>();

        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }
    }

    //Animator Event
    void CropEnd()
    {
        Collider[] colliders = GetComponentInChildren<Instrument>(true).GetComponents<Collider>();

        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
    }
    //Animator Event
    void SwingFinished()
    {
        StopCoroutine(DelayedWeaponStoved());
        StartCoroutine(DelayedWeaponStoved());
        
    }

    IEnumerator DelayedWeaponStoved()
    {
        yield return new WaitForSeconds(4f);
        GetComponentInChildren<Instrument>(true).gameObject.SetActive(false);
    }

    public Vector2 GetPointer()
    {
         return Camera.main.ScreenToWorldPoint(playerInput.actions["Look"].ReadValue<Vector2>());
    }
    public Ray GetPointerPosition()
    {
         return Camera.main.ScreenPointToRay(playerInput.actions["Look"].ReadValue<Vector2>());
    }



    public Ray GetMouseRay()
    {
        return Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
    }

    

}
