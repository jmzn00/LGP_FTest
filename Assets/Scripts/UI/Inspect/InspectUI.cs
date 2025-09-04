using TMPro;
using UnityEngine;

public class InspectUI : MonoBehaviour
{
    [SerializeField] private GameObject _inspectPanel;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Transform _inspectTransform;
    [SerializeField] private Camera _inspectCamera;

    private GameObject _currentInspectable;

    public void Inspect(InventoryItem item) 
    {
        if(_currentInspectable != null) 
        {
            Destroy(_currentInspectable);
        }
        _currentInspectable = Instantiate(item.worldPrefab, _inspectTransform, true);
        _currentInspectable.transform.localPosition = Vector3.zero;
        Rigidbody rb = _currentInspectable.GetComponent<Rigidbody>();
        if (rb) 
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Vector3 directionToCamera = _inspectCamera.transform.position - _currentInspectable.transform.position;
        _currentInspectable.transform.rotation = Quaternion.LookRotation(directionToCamera, Vector3.up);

        _descriptionText.text = item.description;
        _inspectPanel.SetActive(true);        
    }
    public void Close() 
    {
        _inspectPanel.SetActive(false);
    }
}
