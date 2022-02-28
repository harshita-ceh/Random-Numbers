using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tile : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayData;
    Image myImage;
    private void Awake() {
        displayData.text = Random.Range(0, 100).ToString();
        myImage = GetComponent<Image>();
    }

    public void PopulateCurrentTile(){
        displayData.gameObject.SetActive(true);
        myImage.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
    
    public void GenerateData(){
        PopulateCurrentTile();
        Vector2Int hitPosition = GridManager.instance.lookUpTileIndex(this.gameObject);
        GridManager.instance.PopulateTiles(hitPosition);
    }
    
}
