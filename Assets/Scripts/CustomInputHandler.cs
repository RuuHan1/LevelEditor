using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomInputHandler : MonoBehaviour
{
    public event Action OnPlaceInput;
    public event Action OnUndoInput;
    public event Action OnRedoInput;

    public Vector2 MousePosition {  get; private set; }

    [SerializeField] private bool _place;
    public bool Place => _place;


    private InputAction _placeAction;
    private InputAction _undoAction;
    private InputAction _redoAction;
    private InputAction _mouseAction;
    private void Awake()
    {
        _placeAction = new InputAction("Place", binding: "<Mouse>/leftButton", type: InputActionType.Button);
        _undoAction = new InputAction("Undo", binding: "<Keyboard>/z", type: InputActionType.Button);
        _redoAction = new InputAction("Redo", binding: "<Keyboard>/y", type: InputActionType.Button);
        _mouseAction = new InputAction("Mouse Pos", binding: "<Mouse>/position", type: InputActionType.Value);
    }

    private void OnEnable()
    {
        
        _placeAction.Enable();
        _undoAction.Enable();
        _redoAction.Enable();
        _mouseAction.Enable();


        _placeAction.performed += ctx => OnPlaceInput?.Invoke();
        _placeAction.started += ctx => _place = true;
        _placeAction.canceled += ctx => _place = false;
        _undoAction.performed += ctx => OnUndoInput?.Invoke();
        _redoAction.performed += ctx => OnRedoInput?.Invoke();
        _mouseAction.performed += ctx => MousePosition = ctx.ReadValue<Vector2>(); 
    }
    private void OnDisable()
    {
        _placeAction.Disable();
        _undoAction.Disable();
        _redoAction.Disable();
        _mouseAction.Disable();
        _place = false;
    }
}
