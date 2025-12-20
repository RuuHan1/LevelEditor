using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    [SerializeField] private CommandInvoker _commandInvoker;
    [SerializeField] GameObject _blockPrefab;
    [SerializeField] LayerMask _groundLayer;

    [SerializeField] Grid _grid;
    private Camera _mainCamera;
    private void Start()
    {
        _mainCamera = Camera.main;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
        }
    }
}
