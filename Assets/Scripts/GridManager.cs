using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private int areaOfInterest = 1;
    public static Tile[,] tileMatrix;
    public static GridManager instance;
    private Tile currentTile;
    GridLayoutGroup gridLayoutGroup;

    private void OnEnable() {
        if(instance == null){
            instance = this;
        }
    }
    private void Start() {
        GenerateGrid();
    }
    
    private void Awake() {
        tileMatrix = new Tile[rows, columns];
        SetGridLayout();
    }
    void GenerateGrid(){
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                tileMatrix[x, y] = Instantiate(tilePrefab, Vector3.one , Quaternion.identity, canvas.transform);
                tileMatrix[x, y].name = $"Tile {x}{y}";
            }
        }
    }
    public Vector2Int lookUpTileIndex(GameObject hitinfo){
        for (int x = 0; x < rows; x++)
            for (int y = 0; y < columns; y++)
                if(tileMatrix[x, y].gameObject == hitinfo)
                    return new Vector2Int(x, y);
        return -Vector2Int.one; //Invalid
    }
    public void PopulatethisTile(int x, int y){
        currentTile = tileMatrix[x, y];
        currentTile.PopulateCurrentTile();
    }
    public void PopulateTiles(Vector2Int hitPosition){
        
        for (int x = hitPosition.x - areaOfInterest + 1; x < hitPosition.x + areaOfInterest; x++){
            for (int y = hitPosition.y - areaOfInterest + 1; y < hitPosition.y + areaOfInterest; y++){
                if(x >= 0 && x < rows && y >= 0 && y < columns){
                    //South
                    if(x + 1 < rows){
                        PopulatethisTile(x + 1, y);
                        //South East
                        if(y + 1 < columns)
                            PopulatethisTile(x + 1, y + 1);
                        //South West
                        if(y - 1 >= 0)
                            PopulatethisTile(x + 1, y - 1);

                    }
                    //North
                    if(x - 1 >= 0){
                        PopulatethisTile(x - 1, y);
                        //North East
                        if(y + 1 < columns)
                            PopulatethisTile(x - 1, y + 1);
                        //North West
                        if(y - 1 >= 0)
                            PopulatethisTile(x - 1, y - 1);

                    }

                    //East
                    if(y + 1 < columns)
                        PopulatethisTile(x, y + 1);
                    //West
                    if(y - 1 >= 0)
                        PopulatethisTile(x, y - 1);
                }
            }
        }
    }
    public void SetGridLayout(){
        gridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = columns;
    }
}
