using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour
{
    public GameObject keyContainerPrefab;
    private GameObject currentKeyRow;
    public GameObject playerKeyPrefab;
    public GameObject ghostKeyPrefab;
    
    public List<Sprite> attackSprites;
    public List<Sprite> attackLearnedSprites;
    
    public static KeyUI instance;

    private bool ghostMode = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ClearKeys(bool clearAll = false, float delay = 0)
    {
        IEnumerator IE_ClearKeys()
        {
            yield return new WaitForSeconds(delay);
            foreach (Transform keyContainer in this.transform)
            {
                //For each child of the KeyContainer object
                foreach (Transform child in keyContainer)
                {
                    if (!child.CompareTag("AttackIcon") || clearAll)
                    {
                        Destroy(child.gameObject);
                    }
                }

                if (clearAll)
                {
                    Destroy(keyContainer.gameObject);
                }
            }
        }

        if (delay == 0)
        {
            foreach (Transform keyContainer in this.transform)
            {
                //For each child of the KeyContainer object
                foreach (Transform child in keyContainer)
                {
                    if (!child.CompareTag("AttackIcon") || clearAll)
                    {
                        Destroy(child.gameObject);
                    }
                }

                if (clearAll)
                {
                    Destroy(keyContainer.gameObject);
                }
            }
        }
        else StartCoroutine(IE_ClearKeys());
    }

    public void SetGhostMode(bool ghostMode)
    {
        this.ghostMode = ghostMode;
    }
    
    public void InstantiateNewRow(AttackType attack, bool isNewAttack = false)
    {
        currentKeyRow = Instantiate(keyContainerPrefab, this.transform);
        
        //Setting attack type icons
        if (isNewAttack)
        {
            currentKeyRow.transform.GetChild(0).GetComponent<Image>().sprite = attackSprites[(int)attack];
            currentKeyRow.transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(100, 100);
            foreach (char key in KeyGameplay.instance.GetKeysForAttack(attack))
            {
                SpawnKeyObject(key.ToString());
            }
        }
        else
        {
            currentKeyRow.transform.GetChild(0).GetComponent<Image>().sprite = attackLearnedSprites[(int)attack];
            currentKeyRow.transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(35, 35);
        }
    }
    
    public void SpawnKeyObject(string key)
    {
        //Create clone of key "template" aka prefab
        GameObject keyObject = Instantiate(ghostMode? ghostKeyPrefab : playerKeyPrefab);
        
        //Set the parent of the key object to be the KeyContainer
        keyObject.transform.SetParent(currentKeyRow.transform);
        
        //Set the text of the keyobject to our specific text
        keyObject.GetComponentInChildren<TMP_Text>().text = key;
    }

    public void RecallRow(int rowIndex, Attack attackSequence)
    {
        currentKeyRow = this.transform.GetChild(rowIndex).gameObject;
        currentKeyRow.transform.GetChild(0).GetComponent<Image>().sprite = attackLearnedSprites[(int)attackSequence.attacks[rowIndex]];
        currentKeyRow.transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(35, 35);
    }

    public void SetRowGreen(int rowIndex)
    {
        foreach (Transform key in transform.GetChild(rowIndex).transform)
        {
            key.GetComponent<Image>().color = Color.green;
        }
    }
}
