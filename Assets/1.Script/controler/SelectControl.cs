using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectControl : MonoBehaviour
{
    public int id;
    int[] sel = {1,12,24};

    public  Transform pos;

    public GameObject[] sellect;
    int total;

    public void init()
    {
        StartCoroutine(call());
    }

    IEnumerator call()
    {
        sellect = new GameObject[sel[id]];
        yield return null;

        for (int i = 0; i < sellect.Length; i++)
        {
            sellect[i] = pos.transform.GetChild(i).gameObject;
            sellect[i].GetComponent<BullElementSel>().id = i;

            yield return null; 
            //yield return new WaitForSeconds(0.1f);

            sellect[i].GetComponent<BullElementSel>().OnSet(CreateBullet());
        }
    }

    public int CreateBullet() // 총알 뽑기
    {
        Debug.Log("bullet");

        //토탈에 가중치를 더한다..
        total = 0;
        for (int i = 0; i < DataManager.Instance.opBulList.Count; i++)
        {
            total += DataManager.Instance.opBulList[i].weight; //하위템일수록 가중치가 높다.
        }

        Debug.Log("total :" + total);
        int weight = 0;
        int selecnum = 0;

        selecnum = Mathf.RoundToInt(total *Random.Range(0.0f, 1.0f));

        for (int i = 0; i < DataManager.Instance.opBulList.Count; i++)
        {
            weight += DataManager.Instance.opBulList[i].weight;
            if (selecnum <= weight)
            {
                Debug.Log(selecnum + " <= " + weight +    "당첨 :" + i);
                OptionManger.i.selectValueList.Add(i);
                return i;
            }
        }
        
        //OptionManger.i.selectValueList.Add(0);
        return 0;
    }
}
