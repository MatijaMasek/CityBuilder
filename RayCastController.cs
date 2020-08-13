//Credits: Matija Mašek & Zachary Stafford

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Threading;

public class RayCastController : MonoBehaviour
{

    #region Declared variables

    #region Serializables

    //Notes: 0 small farm, 1 regular farm, 2 big farm, 3 small house, 4 regular house, 5 big house, 6 small market, 7 regular market, 8 big market

    [Header("Buildings")]
    [SerializeField] GameObject[] Buildings;

    [Header("Cursors")]
    [SerializeField] GameObject[] CursorPrefabs;
    [SerializeField] private LayerMask EverythingLayerMask;

    [Header("Trees")]
    [SerializeField] GameObject treePrefab;
    [SerializeField] int treeNumber = 100;

    [Header("Map")]
    [SerializeField] GameObject map;
    [SerializeField] [Range(50, 1000)] int mapSize = 50;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip buildAudioClip;
    [SerializeField] AudioClip cutTreeAudioClip;
    [SerializeField] [Range(0f, 1f)] float VolumeSFX;

    [Header("Inventory")]
    [SerializeField] Inventory inventory;

    [Header("Debugging")]
    [SerializeField] private bool ShowRaycasts = false;

    #endregion

    #region Privates

    //Main
    private GameObject[] cursors;
    private Collider[] cursorColliders;
    private List<Collider> AllColliders = new List<Collider>();

    //Cursor position
    int LastPosX, LastPosZ;
    Vector3 mousePos;
    Quaternion rotate90 = Quaternion.Euler(0f, 90f, 0f);

    //Map
    private float cornerPosition = 0;

    #endregion

    #region Constants

    //Constants
    private string TREE = "Tree";
    private string GROUND = "Ground";

    #endregion

    #region Enumerators

    actionModes actionMode = actionModes.nothing;

    public enum actionModes {
        nothing = -2, cutTrees = -1, smallFarm = 0, regularFarm = 1, bigFarm = 2, smallHouse = 3, regularHouse = 4, bigHouse = 5, smallMarket = 6, regularMarket = 7, bigMarket = 8
    }

    #endregion

    #endregion

    #region Start and initialization

    void Start()
    {
        Physics.autoSyncTransforms = true; //Tells Unity to update colliders real time

        map.transform.localScale = new Vector3(mapSize, 1, mapSize);

        audioSource = GetComponent<AudioSource>();

        cornerPosition = mapSize * 5f;

        GenerateTrees();

        cursors = new GameObject[Buildings.Length];

        cursorColliders = new Collider[Buildings.Length];

        for (int i = 0; i < cursors.Length; i++) {
            SetupTheCursor(ref cursors[i], CursorPrefabs[i], ref cursorColliders[i]);
        }
        
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

    private void SetupTheCursor(ref GameObject cursor, GameObject prefab, ref Collider collider) {
        SetUpCursor(ref cursor, prefab);
        collider = cursor.GetComponent<Collider>();
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

    #endregion

    #region Update

    void Update() {

        mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        BuildSetting(actionMode);

        switch (actionMode) {
            case actionModes.nothing:
                break;
            case actionModes.cutTrees:
                cutTrees(ray);
                break;

            default:
                int ActionMode = (int)actionMode;
                BuildStructure(ray, cursors[ActionMode].transform, cursorColliders[ActionMode], Buildings[ActionMode]);
                break;

        }
    }

    void BuildSetting(actionModes buildSetting) {
        switch (buildSetting) {
            case actionModes.nothing:
            case actionModes.cutTrees:
                VisibleBuildings(false, false, false, false, false, false, false, false, false);
                break;

            #region Farms

            case actionModes.smallFarm:
                VisibleBuildings(true, false, false, false, false, false, false, false, false);
                break;
            case actionModes.regularFarm:
                VisibleBuildings(false, true, false, false, false, false, false, false, false);
                break;
            case actionModes.bigFarm:
                VisibleBuildings(false, false, true, false, false, false, false, false, false);
                break;

            #endregion

            #region Houses

            case actionModes.smallHouse:
                VisibleBuildings(false, false, false, true, false, false, false, false, false);
                break;
            case actionModes.regularHouse:
                VisibleBuildings(false, false, false, false, true, false, false, false, false);
                break;
            case actionModes.bigHouse:
                VisibleBuildings(false, false, false, false, false, true, false, false, false);
                break;

            #endregion

            #region Markets

            case actionModes.smallMarket:
                VisibleBuildings(false, false, false, false, false, false, true, false, false);
                break;
            case actionModes.regularMarket:
                VisibleBuildings(false, false, false, false, false, false, false, true, false);
                break;
            case actionModes.bigMarket:
                VisibleBuildings(false, false, false, false, false, false, false, false, true);
                break;

            #endregion

            default:
                break;

        }
    }

    void VisibleBuildings(bool smallFarmVisible, bool regularFarmVisible, bool bigFarmVisible, bool smallHouseVisible, bool regularHouseVisible, bool bigHouseVisible, bool smallMarketVisible, bool regularMarketVisible, bool bigMarketVisible) {

        cursors[0].SetActive(smallFarmVisible);
        cursors[1].SetActive(regularFarmVisible);
        cursors[2].SetActive(bigFarmVisible);
        cursors[3].SetActive(smallHouseVisible);
        cursors[4].SetActive(regularHouseVisible);
        cursors[5].SetActive(bigHouseVisible);
        cursors[6].SetActive(smallMarketVisible);
        cursors[7].SetActive(regularMarketVisible);
        cursors[8].SetActive(bigMarketVisible);

    }

    #region Collection trees

    //Collection Trees

    void cutTrees(Ray ray) {
        RaycastHit[] Hits = PublicStatics.RaycastAll(ray, float.MaxValue, Color.green, 1f, EverythingLayerMask, ShowRaycasts);

        if (Hits.Length > 0) {
            for (int i = 0; i < Hits.Length; i++) {
                if (Hits[i].transform.CompareTag(TREE)) {
                    if (MouseDown()) {
                        audioSource.PlayOneShot(cutTreeAudioClip, VolumeSFX);
                        inventory.tree += 1;
                        AllColliders.Remove(Hits[i].transform.gameObject.GetComponent<Collider>());
                        Destroy(Hits[i].transform.gameObject);
                    }

                }
            }
        }
    }

    #endregion

    #region Build a structure

    void BuildStructure(Ray ray, Transform cursorTransform, Collider cursorCollider, GameObject building) {

        RaycastHit[] Hits = PublicStatics.RaycastAll(ray, float.MaxValue, Color.green, 1f, EverythingLayerMask, ShowRaycasts);

        if (Hits.Length > 0) {
            for (int i = 0; i < Hits.Length; i++) {
                if (Hits[i].transform.CompareTag(GROUND)) {

                    BuildCursorLocation(Hits[i], cursorTransform);

                    if (MouseDown()) {

                        if (EmptySpace(cursorCollider)) {

                            AllColliders.Add(BuildBuilding(building, cursorTransform));
                            audioSource.PlayOneShot(buildAudioClip, VolumeSFX);

                        }
                    }
                    break;
                }
            }
        }
    }

    private void BuildCursorLocation(RaycastHit Hit, Transform CursorTransform) {

        int PosX = (int)Mathf.Round(Hit.point.x);
        int PosZ = (int)Mathf.Round(Hit.point.z);

        if (PosX != LastPosX || PosZ != LastPosZ) {

            LastPosX = PosX;
            LastPosZ = PosZ;

            CursorTransform.position = new Vector3(PosX, 0, PosZ);

        }

        if (Input.GetKeyDown(KeyCode.R)) {
            CursorTransform.rotation *= rotate90;
        }

    }

    private bool EmptySpace(Collider TheCollider) {

        int ColliderCount = AllColliders.Count;

        if (ColliderCount > 0) {

            for (int i = 0; i < ColliderCount; i++) {

                if (TheCollider.bounds.Intersects(AllColliders[i].bounds)) {

                    return false;

                }

            }

        }

        return true;

    }

    private Collider BuildBuilding(GameObject Structure, Transform CursorTransform) {

        GameObject Building = Instantiate(Structure, transform);
        Building.transform.position = CursorTransform.position;
        Building.transform.rotation = CursorTransform.rotation;

        //Inventory editing, bonuses, and costs
        switch (actionMode) {

            #region Farms

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

            #endregion

            #region Houses

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

            #endregion

            #region Markets

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

                #endregion

        }

        return Building.GetComponent<Collider>();

    }

    #endregion

    #endregion

    #region Mouse input

    private bool MouseDown() {
        return Input.GetMouseButtonDown(0);
    }

    #endregion

    #region UI buttons

    //UI Buttons

    //Cutting Trees
    public void CutDownTrees()
    {
        actionMode = actionModes.cutTrees;
    }

    #region Farms

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

    #endregion

    #region Houses

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

    #endregion

    #region Markets

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

    #endregion

    #endregion

}
