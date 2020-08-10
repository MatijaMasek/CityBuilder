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

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip buildAudioClip;

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

    private List<Collider> AllBuildingColliders = new List<Collider>();

    int LastPosX, LastPosZ;

    Vector3 mousePos;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask EverythingLayerMask;

    buildModes buildMode = buildModes.nothing;

    public enum buildModes{ nothing, smallHouse, regularHouse, bigHouse, smallFarm, regularFarm, bigFarm, smallMarket, regularMarket, bigMarket}

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

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

    void BuildSetting(buildModes buildSetting)
    {
        switch (buildSetting)
        {
            case buildModes.nothing:
                VisibleBuildings(false, false, false, false, false, false, false, false, false);
                break;
            case buildModes.smallHouse:
                VisibleBuildings(true, false, false, false, false, false, false, false, false);
                break;
            case buildModes.regularHouse:
                VisibleBuildings(false, true, false, false, false, false, false, false, false);
                break;
            case buildModes.bigHouse:
                VisibleBuildings(false, false, true, false, false, false, false, false, false);
                break;
            case buildModes.smallFarm:
                VisibleBuildings(false, false, false, true, false, false, false, false, false);
                break;
            case buildModes.regularFarm:
                VisibleBuildings(false, false, false, false, true, false, false, false, false);
                break;
            case buildModes.bigFarm:
                VisibleBuildings(false, false, false, false, false, true, false, false, false);
                break;
            case buildModes.smallMarket:
                VisibleBuildings(false, false, false, false, false, false, true, false, false);
                break;
            case buildModes.regularMarket:
                VisibleBuildings(false, false, false, false, false, false, false, true, false);
                break;
            case buildModes.bigMarket:
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

        switch (buildMode)
        {

            case buildModes.smallFarm:
                inventory.food += 1;
                inventory.coins -= 1;
                break;

            case buildModes.regularFarm:
                inventory.food += 2;
                inventory.coins -= 2;
                break;

            case buildModes.bigFarm:
                inventory.food += 4;
                inventory.coins -= 4;
                break;


            case buildModes.smallHouse:
                inventory.food -= 1;
                inventory.coins -= 1;
                inventory.workers += 1;
                break;

            case buildModes.regularHouse:
                inventory.food -= 2;
                inventory.coins -= 2;
                inventory.workers += 2;
                break;

            case buildModes.bigHouse:
                inventory.food -= 4;
                inventory.coins -= 4;
                inventory.workers += 4;
                break;


            case buildModes.smallMarket:
                inventory.food -= 1;
                inventory.coins += 1;
                inventory.workers -= 1;
                break;

            case buildModes.regularMarket:
                inventory.food -= 2;
                inventory.coins += 2;
                inventory.workers -= 2;
                break;

            case buildModes.bigMarket:
                inventory.food -= 4;
                inventory.coins += 4;
                inventory.workers -= 4;
                break;



        }

        return Building.GetComponent<Collider>();

    }

    private bool NoBuildingInSpace(Collider CursorCollider)
    {

        int ColliderCount = AllBuildingColliders.Count;

        if (ColliderCount > 0)
        {

            for (int i = 0; i < ColliderCount; i++)
            {

                if (CursorCollider.bounds.Intersects(AllBuildingColliders[i].bounds))
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

        BuildSetting(buildMode);

        switch (buildMode)
        {            
            case buildModes.smallFarm:
                smallFarm(ray);
                break;

            case buildModes.regularFarm:
                regularFarm(ray);
                break;

            case buildModes.bigFarm:
                bigFarm(ray);
                break;

            
            case buildModes.smallHouse:
                smallHouse(ray);
                break;

            case buildModes.regularHouse:
                regularHouse(ray);
                break;

            case buildModes.bigHouse:
                bigHouse(ray);
                break;


            case buildModes.smallMarket:
                smallMarket(ray);
                break;

            case buildModes.regularMarket:
                regularMarket(ray);
                break;

            case buildModes.bigMarket:
                bigMarket(ray);
                break;

            default:
                break;
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

                        if (NoBuildingInSpace(SmallFarmCursorCollider)) 
                        {

                            AllBuildingColliders.Add(BuildBuilding(SmallFarm, cursorSmallFarm.transform));
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

                        if (NoBuildingInSpace(RegularFarmCursorCollider))
                        {

                            AllBuildingColliders.Add(BuildBuilding(RegularFarm, cursorRegularFarm.transform));
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

                        if (NoBuildingInSpace(BigFarmCursorCollider))
                        {

                            AllBuildingColliders.Add(BuildBuilding(BigFarm, cursorBigFarm.transform));
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

                        if (NoBuildingInSpace(SmallHouseCursorCollider))
                        {

                            AllBuildingColliders.Add(BuildBuilding(SmallHouse, cursorSmallHouse.transform));
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

                        if (NoBuildingInSpace(RegularHouseCursorCollider))
                        {

                            AllBuildingColliders.Add(BuildBuilding(RegularHouse, cursorRegularHouse.transform));
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

                        if (NoBuildingInSpace(BigHouseCursorCollider))
                        {

                            AllBuildingColliders.Add(BuildBuilding(BigHouse, cursorBigHouse.transform));
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

                        if (NoBuildingInSpace(SmallMarketCursorCollider))
                        {

                            AllBuildingColliders.Add(BuildBuilding(SmallMarket, cursorSmallMarket.transform));
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

                        if (NoBuildingInSpace(RegularMarketCursorCollider))
                        {

                            AllBuildingColliders.Add(BuildBuilding(RegularMarket, cursorRegularMarket.transform));
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

                        if (NoBuildingInSpace(BigMarketCursorCollider))
                        {

                            AllBuildingColliders.Add(BuildBuilding(BigMarket, cursorBigMarket.transform));
                            audioSource.PlayOneShot(buildAudioClip, VolumeSFX);
                        }
                    }
                    break;
                }
            }
        }
    }

    
    
    //UI Buttons

    //Farms
    public void BuildSmallFarm()
    {
        buildMode = buildModes.smallFarm;      
    }

    public void BuildRegularFarm()
    {
        buildMode = buildModes.regularFarm;    
    }

    public void BuildBigFarm()
    {
        buildMode = buildModes.bigFarm;    
    }

    //Houses
    public void BuildSmallHouse()
    {
        buildMode = buildModes.smallHouse;      
    }

    public void BuildRegularHouse()
    {
        buildMode = buildModes.regularHouse;    
    }

    public void BuildBigHouse()
    {
        buildMode = buildModes.bigHouse;    
    }

    //Markets

    public void BuildSmallMarket()
    {
        buildMode = buildModes.smallMarket;
    }

    public void BuildRegularMarket()
    {
        buildMode = buildModes.regularMarket;
    }

    public void BuildBigMarket()
    {
        buildMode = buildModes.bigMarket;
    }

}
