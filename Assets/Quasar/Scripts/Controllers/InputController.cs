using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

public class InputController : MonoBehaviour
{
    public static UnityEvent<Vector2> pullStartedEvent = new UnityEvent<Vector2>();
    public static UnityEvent<Vector2> pullEndedEvent = new UnityEvent<Vector2>();
    
    [SerializeField] private InputActionReference pullAction;
    
    public static InputController Instance { get; private set; }
    public static Vector2 CurrMainTouchScreenPos{ get; private set; }

    private QuasarInputActions quasarInputActions;
    private Coroutine updateMainTouchScreenPosCR;


   


    public void StartPull(InputAction.CallbackContext cc)
    {
        
        updateMainTouchScreenPosCR = StartCoroutine(UpdateCurrMainTouchScreenPos());
    }
    
    public void EndPull(InputAction.CallbackContext cc)
    {
        if (updateMainTouchScreenPosCR != null)
            StopCoroutine(updateMainTouchScreenPosCR);

    }

    // ===== Private API
    private void Awake()
    {
        Instance = this;
        CurrMainTouchScreenPos = Vector2.zero;
        quasarInputActions = new QuasarInputActions();
    }

    private void OnEnable()
    {
        quasarInputActions.Enable();
        //pullAction.action.Enable();
        //pullAction.action.started += StartPull;
        //pullAction.action.canceled += EndPull;    
    }

    private void Start()
    {
        quasarInputActions.Touch1.PullSling.started += cc => StartPull(cc);
        //quasarInputActions.Touch1.PullSling.performed += cc => StartPull(cc);
        quasarInputActions.Touch1.PullSling.canceled += cc => EndPull(cc);
    }

    private void OnDisable()
    {
        quasarInputActions.Disable();

        //pullAction.action.started -= StartPull;
        //pullAction.action.canceled -= EndPull;
    }

    private IEnumerator UpdateCurrMainTouchScreenPos()
    {
        TouchControl t = Touchscreen.current.primaryTouch;
        while (true)
        {
            CurrMainTouchScreenPos = t.position.ReadValue();
            //Debug.Log(CurrMainTouchScreenPos);
            yield return null;
        }
    }
}
