using UnityEngine;
using UnityEngine.UIElements;

public class ViewController : MonoBehaviour
{
    private Button m_CreateTowerButton;
    private VisualElement m_Root;
    private TowerManager towerManager;
    private VisualElement m_TowerSelection;

    void Awake()
    {
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument component not found.");
            return;
        }

        m_Root = uiDocument.rootVisualElement;
        if (m_Root == null)
        {
            Debug.LogError("Root VisualElement not found.");
            return;
        }

        m_CreateTowerButton = m_Root.Q<Button>("Tower1");
        if (m_CreateTowerButton == null)
        {
            Debug.LogError("Create Tower button not found.");
            return;
        }

        m_TowerSelection = m_Root.Q<VisualElement>("TowerSelection");
        if (m_TowerSelection == null)
        {
            Debug.LogError("Tower Selection not found.");
            return;
        }
        else
        {
            Debug.Log("Tower Selection found.");
        }

        m_CreateTowerButton.clicked += CreateTower;
    }

    void Start()
    {
        // Initialize towerManager here, for example:
        towerManager = FindObjectOfType<TowerManager>();
        if (towerManager == null)
        {
            Debug.LogError("TowerManager not found.");
        }
    }

    void OnDestroy()
    {
        if (m_CreateTowerButton != null)
        {
            m_CreateTowerButton.clicked -= CreateTower;
        }
    }

    void CreateTower()
    {
        if (towerManager != null)
        {
            towerManager.CreateTower(0);
        }
        else
        {
            Debug.LogError("towerManager is not initialized.");
        }
    }
}