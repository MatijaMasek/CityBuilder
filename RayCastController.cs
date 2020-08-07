//Credits: Matija Mašek & Zachary Stafford

using UnityEngine;
using UnityEngine.EventSystems;

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

    [SerializeField] Inventory inventory;


    GameObject cursorSmallFarm;
    GameObject cursorRegularFarm;
    GameObject cursorBigFarm;

    GameObject cursorSmallHouse;
    GameObject cursorRegularHouse;
    GameObject cursorBigHouse;

    GameObject cursorSmallMarket;
    GameObject cursorRegularMarket;
    GameObject cursorBigMarket;

    int LastPosX, LastPosZ;

    Vector3 mousePos;

    public LayerMask layerMask;

    buildModes buildMode = buildModes.nothing;

    public enum buildModes{ nothing, smallHouse, regularHouse, bigHouse, smallFarm, regularFarm, bigFarm, smallMarket, regularMarket, bigMarket, smallBuilding, regularBuilding, bigBuilding, }

    void Start()
    {

        SetUpCursor(ref cursorSmallFarm, cursorSmallFarmPrefab);

        SetUpCursor(ref cursorRegularFarm, cursorRegularFarmPrefab);

        SetUpCursor(ref cursorBigFarm, cursorBigFarmPrefab);

        SetUpCursor(ref cursorSmallHouse, cursorSmallHousePrefab);

        SetUpCursor(ref cursorRegularHouse, cursorRegularHousePrefab);

        SetUpCursor(ref cursorBigHouse, cursorBigHousePrefab);

        SetUpCursor(ref cursorSmallMarket, cursorSmallMarketPrefab);

        SetUpCursor(ref cursorRegularMarket, cursorRegularMarketPrefab);

        SetUpCursor(ref cursorBigMarket, cursorBigMarketPrefab);
    }

    void SetUpCursor(ref GameObject cursor, GameObject prefab)
    {
        cursor = Instantiate(prefab);
        cursor.SetActive(false);
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
            //Farms
            case buildModes.smallFarm:
                smallFarm(ray);
                break;

            case buildModes.regularFarm:
                regularFarm(ray);
                break;

            case buildModes.bigFarm:
                bigFarm(ray);
                break;

            //Houses
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

        

        

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            int PosX = (int)Mathf.Round(hit.point.x);
            int PosZ = (int)Mathf.Round(hit.point.z);

            if (PosX != LastPosX || PosZ != LastPosZ)
            {
                LastPosX = PosX;
                LastPosZ = PosZ;

                cursorSmallFarm.transform.position = new Vector3(PosX, 0, PosZ);
            }
            /*Structure structure = hit.transform.GetComponent<Structure>();

            if(structure != null)
            {
                if (Input.GetMouseButtonDown(0) && structure.canBuild)
                {

                    GameObject smallFarm = Instantiate(SmallFarm, transform);
                    smallFarm.transform.position = cursorSmallFarm.transform.position;

                    inventory.food += 1;
                    inventory.coins -= 1;
                }
            }*/
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit[] hits = Physics.RaycastAll(ray, float.MaxValue);

                for (int i = 0; i < hits.Length; i++)
                {
                    Debug.Log(hits[i].transform.name);
                    //if(hit.transform.tag != "Ground")
                    //{
                    //    Debug.Log(hits[i].transform.name);
                    //}
                    //break;

                    //if (!hit.transform.CompareTag("Ground"))
                    //{
                    //    Bounds bounds = hit.transform.GetComponent<Collider>().bounds;
                    //    Debug.Log(hits[i].transform.name);

                    //    if (bounds != null)
                    //    {
                    //        if (cursorSmallFarm.GetComponent<Collider>().bounds.Intersects(bounds))
                    //        {
                    //            Debug.Log("false");

                    //        }
                    //        else
                    //        {
                    //            Debug.Log("True");
                    //            GameObject smallFarm = Instantiate(SmallFarm, transform);
                    //            smallFarm.transform.position = cursorSmallFarm.transform.position;

                    //            inventory.food += 1;
                    //            inventory.coins -= 1;
                    //        }
                    //    }
                    //}
                }

            }

            
                
         
            
            

            
        }
    }

    //Building Regular Farm
    void regularFarm(Ray ray)
    {

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            int PosX = (int)Mathf.Round(hit.point.x);
            int PosZ = (int)Mathf.Round(hit.point.z);

            if (PosX != LastPosX || PosZ != LastPosZ)
            {
                LastPosX = PosX;
                LastPosZ = PosZ;

                cursorRegularFarm.transform.position = new Vector3(PosX, 0, PosZ);
            }

            if (Input.GetMouseButtonDown(0))
            {
                GameObject regularFarm = Instantiate(RegularFarm, transform);
                regularFarm.transform.position = cursorRegularFarm.transform.position;

                inventory.food += 2;
                inventory.coins -= 2;
            }
        }
    }

    //Building Big Farm
    void bigFarm(Ray ray)
    {

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            int PosX = (int)Mathf.Round(hit.point.x);
            int PosZ = (int)Mathf.Round(hit.point.z);

            if (PosX != LastPosX || PosZ != LastPosZ)
            {
                LastPosX = PosX;
                LastPosZ = PosZ;

                cursorBigFarm.transform.position = new Vector3(PosX, 0, PosZ);
            }

            if (Input.GetMouseButtonDown(0))
            {
                GameObject bigFarm = Instantiate(BigFarm, transform);
                bigFarm.transform.position = cursorBigFarm.transform.position;

                inventory.food += 4;
                inventory.coins -= 4;
            }
        }
    }

    //Houses

    //Building Small House
    void smallHouse(Ray ray)
    {
        
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            int PosX = (int)Mathf.Round(hit.point.x);
            int PosZ = (int)Mathf.Round(hit.point.z);

            if (PosX != LastPosX || PosZ != LastPosZ)
            {
                LastPosX = PosX;
                LastPosZ = PosZ;

                cursorSmallHouse.transform.position = new Vector3(PosX, 0, PosZ);
            }

            if(Input.GetMouseButtonDown(0))
            {
                GameObject smallHouse = Instantiate(SmallHouse, transform);
                smallHouse.transform.position = cursorSmallHouse.transform.position;

                inventory.food -= 1;
                inventory.coins -= 1;
                inventory.workers += 1;
            }                         
        }             
    }

    //Building Regular House
    void regularHouse(Ray ray)
    {

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            int PosX = (int)Mathf.Round(hit.point.x);
            int PosZ = (int)Mathf.Round(hit.point.z);

            if (PosX != LastPosX || PosZ != LastPosZ)
            {
                LastPosX = PosX;
                LastPosZ = PosZ;

                cursorRegularHouse.transform.position = new Vector3(PosX, 0, PosZ);
            }

            if (Input.GetMouseButtonDown(0))
            {
                GameObject regularHouse = Instantiate(RegularHouse, transform);
                regularHouse.transform.position = cursorRegularHouse.transform.position;

                inventory.food -= 2;
                inventory.coins -= 2;
                inventory.workers += 2;
            }
        }
    }

    //Building Big House
    void bigHouse(Ray ray)
    {

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            int PosX = (int)Mathf.Round(hit.point.x);
            int PosZ = (int)Mathf.Round(hit.point.z);

            if (PosX != LastPosX || PosZ != LastPosZ)
            {
                LastPosX = PosX;
                LastPosZ = PosZ;

                cursorBigHouse.transform.position = new Vector3(PosX, 0, PosZ);
            }

            if (Input.GetMouseButtonDown(0))
            {
                GameObject bigHouse = Instantiate(BigHouse, transform);
                bigHouse.transform.position = cursorBigHouse.transform.position;

                inventory.food -= 4;
                inventory.coins -= 4;
                inventory.workers += 4;
            }
        }
    }

    //Markets

    //Building Small Market
    void smallMarket(Ray ray)
    {

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            int PosX = (int)Mathf.Round(hit.point.x);
            int PosZ = (int)Mathf.Round(hit.point.z);

            if (PosX != LastPosX || PosZ != LastPosZ)
            {
                LastPosX = PosX;
                LastPosZ = PosZ;

                cursorSmallMarket.transform.position = new Vector3(PosX, 0, PosZ);
            }

            if (Input.GetMouseButtonDown(0))
            {
                GameObject smallMarket = Instantiate(SmallMarket, transform);
                smallMarket.transform.position = cursorSmallMarket.transform.position;

                inventory.food -= 1;
                inventory.coins += 1;
                inventory.workers -= 1;
            }
        }
    }

    //Building Regular Market
    void regularMarket(Ray ray)
    {

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            int PosX = (int)Mathf.Round(hit.point.x);
            int PosZ = (int)Mathf.Round(hit.point.z);

            if (PosX != LastPosX || PosZ != LastPosZ)
            {
                LastPosX = PosX;
                LastPosZ = PosZ;

                cursorRegularMarket.transform.position = new Vector3(PosX, 0, PosZ);
            }

            if (Input.GetMouseButtonDown(0))
            {
                GameObject regularMarket = Instantiate(RegularMarket, transform);
                regularMarket.transform.position = cursorRegularMarket.transform.position;

                inventory.food -= 2;
                inventory.coins += 2;
                inventory.workers -= 2;
            }
        }
    }

    //Building Big Market
    void bigMarket(Ray ray)
    {

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            int PosX = (int)Mathf.Round(hit.point.x);
            int PosZ = (int)Mathf.Round(hit.point.z);

            if (PosX != LastPosX || PosZ != LastPosZ)
            {
                LastPosX = PosX;
                LastPosZ = PosZ;

                cursorBigMarket.transform.position = new Vector3(PosX, 0, PosZ);
            }

            if (Input.GetMouseButtonDown(0))
            {
                GameObject bigMarket = Instantiate(BigMarket, transform);
                bigMarket.transform.position = cursorBigMarket.transform.position;

                inventory.food -= 4;
                inventory.coins += 4;
                inventory.workers -= 4;
            }
        }
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
            case buildModes.smallBuilding:
                break;
            case buildModes.regularBuilding:
                break;
            case buildModes.bigBuilding:
                break;
            default:
                break;
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
