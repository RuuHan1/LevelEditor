using UnityEngine;

public class PlaceBlockCommand : ICommand
{
    private Vector3 _position;
    private GameObject _blockPrefab;
    private GameObject _placedBlock;
    public PlaceBlockCommand(Vector3 position, GameObject blockPrefab)
    {
        _position = position;
        _blockPrefab = blockPrefab;
    }
    public void Execute()
    {
        _placedBlock = GameObject.Instantiate(_blockPrefab, _position, Quaternion.identity);
    }

    public void Undo()
    {
        if (_placedBlock != null)
        {
            GameObject.Destroy(_placedBlock);
        }
    }
}
