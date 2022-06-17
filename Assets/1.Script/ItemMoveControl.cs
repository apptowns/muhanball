using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMoveControl : MonoBehaviour
{
    public int id; 
    GameObject obj;
    public  GameObject destination;
 

    public void OnSetup(int _id)
    {
        switch (_id) 
        {
            case 0:
                destination = GameObject.Find("ButtonC").gameObject;
                break;
            case 1:
                destination = GameObject.Find("ButtonD").gameObject;
                break;
            case 2:
                destination = GameObject.Find("ButtonM").gameObject;
                break;
            case 3:
                destination = GameObject.Find("ButtonB").gameObject;
                break;
            case 4:
                destination = GameObject.Find("ButtonL").gameObject;
                break;
            case 5:
                destination = GameObject.Find("ButtonH").gameObject;
                break;
        }

        if (destination)
        {
            StartCoroutine(CRRotating());
            StartCoroutine(CRMoving());
        }
        else
        {
            Debug.Log("프레펩이 없어");
        }
    }
 
    IEnumerator CRMoving()
    {
        float currenttime = 0.5f;
        float speed = 0;
        float a = 0;
        while(gameObject.activeInHierarchy) 
        {
            yield return null;

            if(currenttime > 0) 
            {
                currenttime -= 0.05f;
                transform.Translate(new Vector3(5f, 0, 0) * Time.deltaTime);
            }

            a = Vector2.Distance(transform.position, destination.transform.position);
            if (a < 1f)
            {
                destination.GetComponent<ItemScale>().scaleStart();
                this.gameObject.SetActive(false);
            }
            speed += 2f;
            transform.position = Vector3.MoveTowards(transform.position, destination.transform.position, speed * Time.deltaTime);                      
        }
    }

    private IEnumerator CRRotating()
    {
        while (gameObject.activeInHierarchy)
        {
            transform.localEulerAngles += Vector3.forward * 500f * Time.deltaTime;
            yield return null;
        }
    }
}
