using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValuePanel : MonoBehaviour
{
    public Text dia;
    public Text coin;

    private void Update()
    {
        dia.text = DataManager.Instance.getDia().ToString();
        coin.text = DataManager.Instance.getCoin().ToString();
    }
}
