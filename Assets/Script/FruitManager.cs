using UnityEngine;


public class FruitManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Fruit[] _fruitsPrefabs;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Fruit curentFruit;
    [SerializeField] private Transform fruitsParent;
    


    [Header("Settings")]
    [SerializeField] private float fruitsSpawnPointY;
    [SerializeField] private bool isMoveAvailable;
    [SerializeField] private bool isControlFruits;
    [SerializeField] private MergeManager _mergeManager;


    [Header("Debug")]
    [SerializeField] private bool enableGizmo;

    void Awake()
    {
       _mergeManager.OnFruitMergeProcessed += FruitMergeProcessedCollback;
    }

    void Start()
    {
        isMoveAvailable = true;
        HidenLineFruits();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoveAvailable)
            ManagerPlayerInput();
    }

    void ManagerPlayerInput()
    {

        if (Input.GetMouseButtonDown(0))
            MouseDownColback();

        else if (Input.GetMouseButton(0))
        {
            if (isControlFruits)
                MouseDragColback();
            else
                MouseDownColback();

        }

        else if (Input.GetMouseButtonUp(0) && isControlFruits)
            MouseUpColback();
    }

    private void MouseDownColback()
    {
        ShowLineFruits();
        PlaceLineAtClickPosition();
        SpawnFruits();
        isControlFruits = true;
    }

    private void MouseDragColback()
    {
        // Сделай для этого метод
        PlaceLineAtClickPosition();
        curentFruit.MoveTo(new Vector2(GetClickWoroldPosition().x, fruitsSpawnPointY));

    }

    private void MouseUpColback()
    {
        HidenLineFruits();
        DropFruits();
        curentFruit.EnableFruit();
        isMoveAvailable = false;
        isControlFruits = false;
        StartTimer();
    }

    private void SpawnFruits()
    {
        //включаем линию
        Vector2 spawnPosition = GetSpawnPositionWorld();
        spawnPosition.y = fruitsSpawnPointY;


        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        curentFruit = Instantiate(_fruitsPrefabs[Random.Range(0, _fruitsPrefabs.Length)], spawnPosition, Quaternion.identity, fruitsParent);

        curentFruit.name = "Fruit" + Random.Range(0, 1000);
    }

    void PlaceLineAtClickPosition()
    {
        _lineRenderer.SetPosition(0, GetSpawnPositionWorld());
        _lineRenderer.SetPosition(1, GetSpawnPositionWorld() + Vector2.down * 15);
    }


    private void DropFruits()
    {
        curentFruit.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }


    private Vector2 GetClickWoroldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private Vector2 GetSpawnPositionWorld()
    {
        Vector2 woroldCekedPosition = GetClickWoroldPosition();
        woroldCekedPosition.y = fruitsSpawnPointY;
        return woroldCekedPosition;
    }


    private void HidenLineFruits()
    {
        _lineRenderer.enabled = false;
    }


    private void ShowLineFruits()
    {
        _lineRenderer.enabled = true;
    }


    private void StartTimer()
    {
        Invoke("EnableMove", .2f);
    }


    private void EnableMove()
    {
        isMoveAvailable = true;
    }


    private void FruitMergeProcessedCollback(FruitType fruitType, Vector2 spawnPosition)
    {
        for (int i = 0; i < _fruitsPrefabs.Length; i++)
        {
            if (_fruitsPrefabs[i].GetFruitType() == fruitType)
            {
                SpawnMergedFruits(spawnPosition, _fruitsPrefabs[i]);
                break;
            }
        }

    }
    

    private void SpawnMergedFruits(Vector2 spawnPosition, Fruit fruit)
    {
        Fruit fruitInstance = Instantiate(fruit, spawnPosition, Quaternion.identity, fruitsParent);  
        fruitInstance.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    //Дебаг полоса появления фруктов. видна только в редакторе Unity
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!enableGizmo)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(-50, fruitsSpawnPointY, 0), new Vector3(50, fruitsSpawnPointY, 0));
    }
#endif

}
