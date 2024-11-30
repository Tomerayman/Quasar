using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using ET = UnityEngine.InputSystem.EnhancedTouch;


public class InputController : MonoBehaviour
{
    public static UnityEvent<Vector2> pullStartedEvent = new UnityEvent<Vector2>();
    public static UnityEvent<Vector2> pullEndedEvent = new UnityEvent<Vector2>();
    
    [SerializeField] private InputActionReference pullAction;
    
    public static InputController Instance { get; private set; }
    public static Vector2 CurrMainTouchScreenPos{ get; private set; }

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
    }

    private void OnEnable()
    {
        //pullAction.action.Enable();
        //pullAction.action.started += StartPull;
        //pullAction.action.canceled += EndPull;    
    }

    private void Start()
    {
        //quasarInputActions.Touch1.PullSling.started += StartPull;
        //quasarInputActions.Touch1.PullSling.performed += cc => StartPull(cc);
        //quasarInputActions.Touch1.PullSling.canceled += EndPull;
        updateMainTouchScreenPosCR = StartCoroutine(UpdateCurrMainTouchScreenPos());

    }

    private void OnDisable()
    {

        //pullAction.action.started -= StartPull;
        //pullAction.action.canceled -= EndPull;
    }

    private IEnumerator UpdateCurrMainTouchScreenPos()
    {
        ET.EnhancedTouchSupport.Enable();

        while (true)
        {
            if (ET.Touch.activeTouches.Count > 0)
            {
                var touch = ET.Touch.activeTouches[0];
                if (touch.screenPosition.x >= 0 && touch.screenPosition.y >= 0)
                {
                    CurrMainTouchScreenPos = touch.screenPosition;
                }
            }
            yield return null;
        }
    }
}
