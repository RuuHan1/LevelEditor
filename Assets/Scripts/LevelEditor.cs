using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    [SerializeField] private CommandInvoker _commandInvoker;
    [SerializeField] GameObject _blockPrefab;
    [SerializeField] LayerMask _groundLayer;
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
        _commandInvoker.OnCommandUndone += HandleUndoFeedback;
        _commandInvoker.OnCommandRedone += HandleRedoFeedback;
    }
    private void OnDisable()
    {
        _commandInvoker.OnCommandUndone -= HandleUndoFeedback;
        _commandInvoker.OnCommandRedone -= HandleRedoFeedback;
    }
    private void Update()
    {
            if (Input.GetMouseButtonDown(0) && _remainingBlocks > 0 )
            {
                HandleInput();
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                _commandInvoker.UndoCommand();
                
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                _commandInvoker.RedoCommand();
            }
    }
    private void HandleInput()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _groundLayer))
        {
            Vector3Int cellPos = _grid.WorldToCell(hitInfo.point);
            Vector3 worldCenterWorld = _grid.GetCellCenterWorld(cellPos);
            ICommand placeBlockCommand = new PlaceBlockCommand( worldCenterWorld,_blockPrefab);
            _commandInvoker.ExecuteCommand(placeBlockCommand);
            _remainingBlocks--;
            OnBlockCountChanged?.Invoke(_remainingBlocks);
        }
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
