//Credits: Matija Mašek & Zachary Stafford

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class RayCastController : MonoBehaviour
{
    [Header("Buildings")]
    [SerializeField] GameObject SmallFarm;
    [SerializeField] GameObject RegularFarm;
    [SerializeField] GameObject BigFarm;

    [SerializeField] GameObject SmallHouse;
    [SerializeField] GameObject RegularHouse;
    [SerializeField] GameObject BigHouse;

    [SerializeField] GameObject SmallMarket;
    [SerializeField] GameObject RegularMarket;
    [SerializeField] GameObject BigMarket;

    [Header("Cursors")]
    [SerializeField] GameObject cursorSmallFarmPrefab;
    [SerializeField] GameObject cursorRegularFarmPrefab;
    [SerializeField] GameObject cursorBigFarmPrefab;

    [SerializeField] GameObject cursorSmallHousePrefab;
    [SerializeField] GameObject cursorRegularHousePrefab;
    [SerializeField] GameObject cursorBigHousePrefab;

    [SerializeField] GameObject cursorSmallMarketPrefab;
    [SerializeField] GameObject cursorRegularMarketPrefab;
    [SerializeField] GameObject cursorBigMarketPrefab;

    [Header("Trees")]
    [SerializeField] GameObject treePrefab;
    [SerializeField] int treeNumber = 100;

    [Header("Map")]
    [SerializeField] GameObject map;
    [SerializeField] [Range(50, 1000)] int mapSize = 50;
    private float cornerPosition = 0;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip buildAudioClip;
    [SerializeField] AudioClip cutTreeAudioClip;

    [SerializeField] [Range(0f, 1f)] float VolumeSFX;

    [SerializeField] Inventory inventory;

    [SerializeField] private bool ShowRaycasts = false;

    GameObject cursorSmallFarm;
    GameObject cursorRegularFarm;
    GameObject cursorBigFarm;

    GameObject cursorSmallHouse;
    GameObject cursorRegularHouse;
    GameObject cursorBigHouse;

    GameObject cursorSmallMarket;
    GameObject cursorRegularMarket;
    GameObject cursorBigMarket;

    private Collider SmallFarmCursorCollider;
    private Collider RegularFarmCursorCollider;
    private Collider BigFarmCursorCollider;

    private Collider SmallHouseCursorCollider;
    private Collider RegularHouseCursorCollider;
    private Collider BigHouseCursorCollider;

    private Collider SmallMarketCursorCollider;
    private Collider RegularMarketCursorCollider;
    private Collider BigMarketCursorCollider;

    private List<Collider> AllColliders = new List<Collider>();

    int LastPosX, LastPosZ;

    Vector3 mousePos;

    [SerializeField] private LayerMask EverythingLayerMask;

    actionModes actionMode = actionModes.nothing;

    public enum actionModes{ nothing, cutTrees, smallHouse, regularHouse, bigHouse, smallFarm, regularFarm, bigFarm, smallMarket, regularMarket, bigMarket}

    void Start()
    {
        map.transform.localScale = new Vector3(mapSize, 1, mapSize);

        Physics.autoSyncTransforms = true;

        audioSource = GetComponent<AudioSource>();

        cornerPosition = mapSize * 5f;

        GenerateTrees();

        SetUpCursor(ref cursorSmallFarm, cursorSmallFarmPrefab);
        SmallFarmCursorCollider = cursorSmallFarm.GetComponent<Collider>();

        SetUpCursor(ref cursorRegularFarm, cursorRegularFarmPrefab);
        RegularFarmCursorCollider = cursorRegularFarm.GetComponent<Collider>();

        SetUpCursor(ref cursorBigFarm, cursorBigFarmPrefab);
        BigFarmCursorCollider = cursorBigFarm.GetComponent<Collider>();


        SetUpCursor(ref cursorSmallHouse, cursorSmallHousePrefab);
        SmallHouseCursorCollider = cursorSmallHouse.GetComponent<Collider>();

        SetUpCursor(ref cursorRegularHouse, cursorRegularHousePrefab);
        RegularHouseCursorCollider = cursorRegularHouse.GetComponent<Collider>();

        SetUpCursor(ref cursorBigHouse, cursorBigHousePrefab);
        BigHouseCursorCollider = cursorBigHouse.GetComponent<Collider>();


        SetUpCursor(ref cursorSmallMarket, cursorSmallMarketPrefab);
        SmallMarketCursorCollider = cursorSmallMarket.GetComponent<Collider>();

        SetUpCursor(ref cursorRegularMarket, cursorRegularMarketPrefab);
        RegularMarketCursorCollider = cursorRegularMarket.GetComponent<Collider>();

        SetUpCursor(ref cursorBigMarket, cursorBigMarketPrefab);
        BigMarketCursorCollider = cursorBigMarket.GetComponent<Collider>();


    }

    void GenerateTrees()
    {
        for (int i = 0; i < treeNumber; i++)
        {
            GameObject tree = Instantiate(treePrefab);

            tree.transform.position = getRandomPosition();

            Collider collider = tree.GetComponent<Collider>();         

            while (!EmptySpace(collider))
            {
                tree.transform.position = getRandomPosition();               
            }

            AllColliders.Add(collider);

        }
    }

    Vector3 getRandomPosition()
    {
        float x = Random.Range(-cornerPosition, cornerPosition);
        float z = Random.Range(cornerPosition, -cornerPosition);

        return new Vector3(x, 0, z);
    }

    //Setting Up Cursors

    void SetUpCursor(ref GameObject cursor, GameObject prefab)
    {
        cursor = Instantiate(prefab);
        cursor.SetActive(false);
    }

    private void BuildCursorLocation(RaycastHit Hit, Transform CursorTransform)
    {

        int PosX = (int)Mathf.Round(Hit.point.x);
        int PosZ = (int)Mathf.Round(Hit.point.z);

        if (PosX != LastPosX || PosZ != LastPosZ)
        {

            LastPosX = PosX;
            LastPosZ = PosZ;

            CursorTransform.position= new Vector3(PosX, 0, PosZ);

        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            CursorTransform.rotation *= Quaternion.Euler(0f, 90f, 0f);
        }

    }

    void VisibleBuildings(bool smallHouseVisible, bool regularHouseVisible, bool bigHouseVisible, bool smallFarmVisible, bool regularFarmVisible, bool bigFarmVisible, bool smallMarketVisible, bool regularMarketVisible, bool bigMarketVisible)
    {

        cursorSmallHouse.SetActive(smallHouseVisible);
        cursorRegularHouse.SetActive(regularHouseVisible);
        cursorBigHouse.SetActive(bigHouseVisible);

        cursorSmallFarm.SetActive(smallFarmVisible);
        cursorRegularFarm.SetActive(regularFarmVisible);
        cursorBigFarm.SetActive(bigFarmVisible);

        cursorSmallMarket.SetActive(smallMarketVisible);
        cursorRegularMarket.SetActive(regularMarketVisible);
        cursorBigMarket.SetActive(bigMarketVisible);
    }

    void BuildSetting(actionModes buildSetting)
    {
        switch (buildSetting)
        {
            case actionModes.nothing:
                VisibleBuildings(false, false, false, false, false, false, false, false, false);
                break;
            case actionModes.cutTrees:
                VisibleBuildings(false, false, false, false, false, false, false, false, false);
                break;
            case actionModes.smallHouse:
                VisibleBuildings(true, false, false, false, false, false, false, false, false);
                break;
            case actionModes.regularHouse:
                VisibleBuildings(false, true, false, false, false, false, false, false, false);
                break;
            case actionModes.bigHouse:
                VisibleBuildings(false, false, true, false, false, false, false, false, false);
                break;
            case actionModes.smallFarm:
                VisibleBuildings(false, false, false, true, false, false, false, false, false);
                break;
            case actionModes.regularFarm:
                VisibleBuildings(false, false, false, false, true, false, false, false, false);
                break;
            case actionModes.bigFarm:
                VisibleBuildings(false, false, false, false, false, true, false, false, false);
                break;
            case actionModes.smallMarket:
                VisibleBuildings(false, false, false, false, false, false, true, false, false);
                break;
            case actionModes.regularMarket:
                VisibleBuildings(false, false, false, false, false, false, false, true, false);
                break;
            case actionModes.bigMarket:
                VisibleBuildings(false, false, false, false, false, false, false, false, true);
                break;
            default:
                break;
        }
    }

    private Collider BuildBuilding(GameObject Structure, Transform CursorTransform)
    {

        GameObject Building = Instantiate(Structure, transform);
        Building.transform.position = CursorTransform.position;
        Building.transform.rotation = CursorTransform.rotation;

        switch (actionMode)
        {          
            case actionModes.smallFarm:
                inventory.food += 1;
                inventory.tree -= 1;
                break;

            case actionModes.regularFarm:
                inventory.food += 2;
                inventory.tree -= 2;
                break;

            case actionModes.bigFarm:
                inventory.food += 4;
                inventory.tree -= 4;
                break;


            case actionModes.smallHouse:
                inventory.food -= 1;
                inventory.tree -= 1;
                inventory.workers += 1;
                break;

            case actionModes.regularHouse:
                inventory.food -= 2;
                inventory.tree -= 2;
                inventory.workers += 2;
                break;

            case actionModes.bigHouse:
                inventory.food -= 4;
                inventory.tree -= 4;
                inventory.workers += 4;
                break;


            case actionModes.smallMarket:
                inventory.food -= 1;
                inventory.coins += 1;
                inventory.workers -= 1;
                break;

            case actionModes.regularMarket:
                inventory.food -= 2;
                inventory.coins += 2;
                inventory.workers -= 2;
                break;

            case actionModes.bigMarket:
                inventory.food -= 4;
                inventory.coins += 4;
                inventory.workers -= 4;
                break;



        }

        return Building.GetComponent<Collider>();

    }

    private bool EmptySpace(Collider TheCollider)
    {

        int ColliderCount = AllColliders.Count;

        if (ColliderCount > 0)
        {
            
            for (int i = 0; i < ColliderCount; i++)
            {

                if (TheCollider.bounds.Intersects(AllColliders[i].bounds))
                {

                    return false;

                }

            }

        }

        return true;

    }

    void Update()
    {
        
        mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        BuildSetting(actionMode);

        switch (actionMode)
        {
            case actionModes.cutTrees:
                cutTrees(ray);
                break;

            case actionModes.smallFarm:
                smallFarm(ray);
                break;

            case actionModes.regularFarm:
                regularFarm(ray);
                break;

            case actionModes.bigFarm:
                bigFarm(ray);
                break;

            
            case actionModes.smallHouse:
                smallHouse(ray);
                break;

            case actionModes.regularHouse:
                regularHouse(ray);
                break;

            case actionModes.bigHouse:
                bigHouse(ray);
                break;


            case actionModes.smallMarket:
                smallMarket(ray);
                break;

            case actionModes.regularMarket:
                regularMarket(ray);
                break;

            case actionModes.bigMarket:
                bigMarket(ray);
                break;

            default:
                break;
        }            
    }

    //Collection Trees

    void cutTrees(Ray ray)
    {
        RaycastHit[] Hits = PublicStatics.RaycastAll(ray, float.MaxValue, Color.green, 1f, EverythingLayerMask, ShowRaycasts);

        if (Hits.Length > 0)
        {
            for (int i = 0; i < Hits.Length; i++)
            {
                if (Hits[i].transform.CompareTag("Tree"))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        audioSource.PlayOneShot(cutTreeAudioClip, VolumeSFX);
                        inventory.tree += 1;
                        AllColliders.Remove(Hits[i].transform.gameObject.GetComponent<Collider>());
                        Destroy(Hits[i].transform.gameObject);
                    }
                        
                }
            }
        }
    }

    //Farms

    //Building Small farm
    void smallFarm(Ray ray)
    {

        RaycastHit[] Hits = PublicStatics.RaycastAll(ray, float.MaxValue, Color.green, 1f, EverythingLayerMask, ShowRaycasts);

        if (Hits.Length > 0) {
            for (int i = 0; i < Hits.Length; i++) {
                if (Hits[i].transform.CompareTag("Ground")) {

                    BuildCursorLocation(Hits[i], cursorSmallFarm.transform);

                    if (Input.GetMouseButtonDown(0)) {

                        if (EmptySpace(SmallFarmCursorCollider)) 
                        {

                            AllColliders.Add(BuildBuilding(SmallFarm, cursorSmallFarm.transform));
                            audioSource.PlayOneShot(buildAudioClip, VolumeSFX);

                        }
                    }
                    break;
                }
            }
        }
    }

    //Building Regular Farm

    void regularFarm(Ray ray)
    {

        RaycastHit[] Hits = PublicStatics.RaycastAll(ray, float.MaxValue, Color.green, 1f, EverythingLayerMask, ShowRaycasts);

        if (Hits.Length > 0)
        {
            for (int i = 0; i < Hits.Length; i++)
            {
                if (Hits[i].transform.CompareTag("Ground"))
                {

                    BuildCursorLocation(Hits[i], cursorRegularFarm.transform);

                    if (Input.GetMouseButtonDown(0))
                    {

                        if (EmptySpace(RegularFarmCursorCollider))
                        {

                            AllColliders.Add(BuildBuilding(RegularFarm, cursorRegularFarm.transform));
                            audioSource.PlayOneShot(buildAudioClip, VolumeSFX);
                        }
                    }
                    break;
                }
            }
        }               
    }

    //Building Big Farm
    void bigFarm(Ray ray)
    {

        RaycastHit[] Hits = PublicStatics.RaycastAll(ray, float.MaxValue, Color.green, 1f, EverythingLayerMask, ShowRaycasts);

        if (Hits.Length > 0)
        {
            for (int i = 0; i < Hits.Length; i++)
            {
                if (Hits[i].transform.CompareTag("Ground"))
                {

                    BuildCursorLocation(Hits[i], cursorBigFarm.transform);

                    if (Input.GetMouseButtonDown(0))
                    {

                        if (EmptySpace(BigFarmCursorCollider))
                        {

                            AllColliders.Add(BuildBuilding(BigFarm, cursorBigFarm.transform));
                            audioSource.PlayOneShot(buildAudioClip, VolumeSFX);
                        }
                    }
                    break;
                }
            }
        }
    }

    //Houses

    //Building Small House
    void smallHouse(Ray ray)
    {

        RaycastHit[] Hits = PublicStatics.RaycastAll(ray, float.MaxValue, Color.green, 1f, EverythingLayerMask, ShowRaycasts);

        if (Hits.Length > 0)
        {
            for (int i = 0; i < Hits.Length; i++)
            {
                if (Hits[i].transform.CompareTag("Ground"))
                {

                    BuildCursorLocation(Hits[i], cursorSmallHouse.transform);

                    if (Input.GetMouseButtonDown(0))
                    {

                        if (EmptySpace(SmallHouseCursorCollider))
                        {

                            AllColliders.Add(BuildBuilding(SmallHouse, cursorSmallHouse.transform));
                            audioSource.PlayOneShot(buildAudioClip, VolumeSFX);
                        }
                    }
                    break;
                }
            }
        }
    }

    //Building Regular House
    void regularHouse(Ray ray)
    {

        RaycastHit[] Hits = PublicStatics.RaycastAll(ray, float.MaxValue, Color.green, 1f, EverythingLayerMask, ShowRaycasts);

        if (Hits.Length > 0)
        {
            for (int i = 0; i < Hits.Length; i++)
            {
                if (Hits[i].transform.CompareTag("Ground"))
                {

                    BuildCursorLocation(Hits[i], cursorRegularHouse.transform);

                    if (Input.GetMouseButtonDown(0))
                    {

                        if (EmptySpace(RegularHouseCursorCollider))
                        {

                            AllColliders.Add(BuildBuilding(RegularHouse, cursorRegularHouse.transform));
                            audioSource.PlayOneShot(buildAudioClip, VolumeSFX);
                        }
                    }
                    break;
                }
            }
        }
    }

    //Building Big House
    void bigHouse(Ray ray)
    {

        RaycastHit[] Hits = PublicStatics.RaycastAll(ray, float.MaxValue, Color.green, 1f, EverythingLayerMask, ShowRaycasts);

        if (Hits.Length > 0)
        {
            for (int i = 0; i < Hits.Length; i++)
            {
                if (Hits[i].transform.CompareTag("Ground"))
                {

                    BuildCursorLocation(Hits[i], cursorBigHouse.transform);

                    if (Input.GetMouseButtonDown(0))
                    {

                        if (EmptySpace(BigHouseCursorCollider))
                        {

                            AllColliders.Add(BuildBuilding(BigHouse, cursorBigHouse.transform));
                            audioSource.PlayOneShot(buildAudioClip, VolumeSFX);
                        }
                    }
                    break;
                }
            }
        }
    }

    //Markets

    //Building Small Market
    void smallMarket(Ray ray)
    {

        RaycastHit[] Hits = PublicStatics.RaycastAll(ray, float.MaxValue, Color.green, 1f, EverythingLayerMask, ShowRaycasts);

        if (Hits.Length > 0)
        {
            for (int i = 0; i < Hits.Length; i++)
            {
                if (Hits[i].transform.CompareTag("Ground"))
                {

                    BuildCursorLocation(Hits[i], cursorSmallMarket.transform);

                    if (Input.GetMouseButtonDown(0))
                    {

                        if (EmptySpace(SmallMarketCursorCollider))
                        {

                            AllColliders.Add(BuildBuilding(SmallMarket, cursorSmallMarket.transform));
                            audioSource.PlayOneShot(buildAudioClip, VolumeSFX);
                        }
                    }
                    break;
                }
            }
        }
    }

    //Building Regular Market
    void regularMarket(Ray ray)
    {


        RaycastHit[] Hits = PublicStatics.RaycastAll(ray, float.MaxValue, Color.green, 1f, EverythingLayerMask, ShowRaycasts);

        if (Hits.Length > 0)
        {
            for (int i = 0; i < Hits.Length; i++)
            {
                if (Hits[i].transform.CompareTag("Ground"))
                {

                    BuildCursorLocation(Hits[i], cursorRegularMarket.transform);

                    if (Input.GetMouseButtonDown(0))
                    {

                        if (EmptySpace(RegularMarketCursorCollider))
                        {

                            AllColliders.Add(BuildBuilding(RegularMarket, cursorRegularMarket.transform));
                            audioSource.PlayOneShot(buildAudioClip, VolumeSFX);
                        }
                    }
                    break;
                }
            }
        }
    }

    //Building Big Market
    void bigMarket(Ray ray)
    {


        RaycastHit[] Hits = PublicStatics.RaycastAll(ray, float.MaxValue, Color.green, 1f, EverythingLayerMask, ShowRaycasts);

        if (Hits.Length > 0)
        {
            for (int i = 0; i < Hits.Length; i++)
            {
                if (Hits[i].transform.CompareTag("Ground"))
                {

                    BuildCursorLocation(Hits[i], cursorBigMarket.transform);

                    if (Input.GetMouseButtonDown(0))
                    {

                        if (EmptySpace(BigMarketCursorCollider))
                        {

                            AllColliders.Add(BuildBuilding(BigMarket, cursorBigMarket.transform));
                            audioSource.PlayOneShot(buildAudioClip, VolumeSFX);
                        }
                    }
                    break;
                }
            }
        }
    }

    
    
    //UI Buttons

    //Cutting Trees
    public void CutDownTrees()
    {
        actionMode = actionModes.cutTrees;
    }

    //Farms
    public void BuildSmallFarm()
    {
        actionMode = actionModes.smallFarm;      
    }

    public void BuildRegularFarm()
    {
        actionMode = actionModes.regularFarm;    
    }

    public void BuildBigFarm()
    {
        actionMode = actionModes.bigFarm;    
    }

    //Houses
    public void BuildSmallHouse()
    {
        actionMode = actionModes.smallHouse;      
    }

    public void BuildRegularHouse()
    {
        actionMode = actionModes.regularHouse;    
    }

    public void BuildBigHouse()
    {
        actionMode = actionModes.bigHouse;    
    }

    //Markets

    public void BuildSmallMarket()
    {
        actionMode = actionModes.smallMarket;
    }

    public void BuildRegularMarket()
    {
        actionMode = actionModes.regularMarket;
    }

    public void BuildBigMarket()
    {
        actionMode = actionModes.bigMarket;
    }

}
