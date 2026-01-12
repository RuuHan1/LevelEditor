using Unity.Mathematics;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    [SerializeField] private CommandInvoker _commandInvoker;
    [SerializeField] GameObject _blockPrefab;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] LayerMask _blockLayerMask;
    [SerializeField] private CustomInputHandler _inputHandler;
    public static event System.Action <int> OnBlockCountChanged;
    [SerializeField] Grid _grid;
    private Camera _mainCamera;
    private int _remainingBlocks = 150;

    private void Start()
    {
        _mainCamera = Camera.main;
        OnBlockCountChanged?.Invoke(_remainingBlocks);
    }
    private void OnEnable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnPlaceInput += HandlePlacementRequest;
            _inputHandler.OnUndoInput += HandleUndoRequest;
            _inputHandler.OnRedoInput += HandleRedoRequest;
        }
        _commandInvoker.OnCommandUndone += HandleUndoFeedback;
        _commandInvoker.OnCommandRedone += HandleRedoFeedback;
    }
    private void OnDisable()
    {
        _commandInvoker.OnCommandUndone -= HandleUndoFeedback;
        _commandInvoker.OnCommandRedone -= HandleRedoFeedback;
    }
    private void HandlePlacementRequest()
    {
        if (_remainingBlocks <= 0) return;
        Vector2 screenPos = _inputHandler.MousePosition;
        Ray ray = _mainCamera.ScreenPointToRay(screenPos);
        
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _groundLayer))
        {
            Vector3Int cellPos = _grid.WorldToCell(hitInfo.point);
            Vector3 worldCenterWorld = _grid.GetCellCenterWorld(cellPos);
            if(Physics.CheckBox(worldCenterWorld, new Vector3(0.1f, 0.1f, 0.1f), quaternion.identity, _blockLayerMask)) return;
            ICommand placeBlockCommand = new PlaceBlockCommand( worldCenterWorld,_blockPrefab);
            _commandInvoker.ExecuteCommand(placeBlockCommand);
            _remainingBlocks--;
            OnBlockCountChanged?.Invoke(_remainingBlocks);
        }
    }
    private void HandleUndoRequest()
    {
        _commandInvoker.UndoCommand();
    }
    private void HandleRedoRequest()
    {
        _commandInvoker.RedoCommand();
    }
    private void HandleUndoFeedback(ICommand command)
    {
        if (command is PlaceBlockCommand)
        {
            _remainingBlocks++;
            OnBlockCountChanged?.Invoke(_remainingBlocks);
        }
    }
    private void HandleRedoFeedback(ICommand command)
    {
        if (command is PlaceBlockCommand)
        {
            _remainingBlocks--;
            OnBlockCountChanged?.Invoke(_remainingBlocks);
        }
    }
}
