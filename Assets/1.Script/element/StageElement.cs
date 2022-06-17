using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageElement : MonoBehaviour
{
    public int id;
    public GameObject rock;
    public int stage;
    public Text stageNo;
    public Image currentStage;
    public Button click;
    float y;

    public void onSet(int _id, float _x)
    {
        id = _id;
        stage = ((id * 10) + 1);
        stageNo.text = stage.ToString();

        Vector3 v = click.transform.localPosition;
        v.x = _x;
        click.transform.localPosition = v;

        //Debug.Log("스테이지 셋 : " + stage + " : " + DataManager.Instance.stage);
        currentStage.enabled = false;

        if (stage <= DataManager.Instance.saveData.stageMax)
        {
            click.GetComponent<Button>().interactable = true;
            rock.SetActive(true);

            if (stage > DataManager.Instance.saveData.stagePlay)
            {
                currentStage.enabled = true;
            }
            if (DataManager.Instance.saveData.stagePlay ==1)
            {
                currentStage.enabled = true;
            }

        }
        else
        {
            click.GetComponent<Button>().interactable = false;
            rock.SetActive(false);
        }
    } 

    public void getLaststage() 
    {


    }
}
