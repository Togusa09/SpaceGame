using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class StatusUiPanel : MonoBehaviour
{
    private Text _shipName;
    private Text _shipHealth;

    private Button _moveButton;
    private Button _attackButton;
    private Button _stopButton;

    public CustomProgressBar healthBar;
    public CustomProgressBar shieldBar;

    public Control Control;

    void Start()
    {
        _shipName = transform.Find("ShipName").GetComponent<Text>();
        _shipHealth = transform.Find("ShipHealth").GetComponent<Text>();

        _moveButton = transform.Find("MoveButton").GetComponent<Button>();
        _attackButton = transform.Find("AttackButton").GetComponent<Button>();
        _stopButton = transform.Find("StopButton").GetComponent<Button>();

        // Temporarily disabling these until the functionality is added
        _moveButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        var selectedShip = SelectionManager.Instance.GetSelectedShip();
        if (selectedShip != null)
        {
            _shipName.text = selectedShip.name;
            _shipHealth.text = $"{selectedShip.CurrentHealth}/{selectedShip.MaxHealth}";
            healthBar.CurrentValue = ((float)selectedShip.CurrentHealth / (float)selectedShip.MaxHealth);
            shieldBar.CurrentValue = ((float)selectedShip.CurrentShield / (float)selectedShip.MaxShield);
            _attackButton.interactable = true;
            //_moveButton.interactable = true;
            _stopButton.interactable = true;
        }
        else
        {
            _shipName.text = string.Empty;
            _shipHealth.text = string.Empty;
            _attackButton.interactable = false;
            _moveButton.interactable = false;
            _stopButton.interactable = false;
        }
    }

    public void StopSelectedShip()
    {
        var selectedShip = SelectionManager.Instance.GetSelectedShip();
        selectedShip.StopAll();
    }

    public void StartAttack()
    {
        Control.StartAttackSelection();
    }
}
