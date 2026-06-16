using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCharger : MonoBehaviour,IHittable
{
    [Header("关联的发光控制器")]
    public GroupGlowController machineGlowManager;
    public GroupGlowController IndicatorLightGlowManager;

    public GameObject iceBluePrint;
    public CraftingMachine machine;
    private bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        iceBluePrint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLaserHit(Vector3 hitPoint)
    {
        Debug.Log("OnLaserHit!");
        if (isActive) return;
        machineGlowManager.SetGroupGlow(true);
        IndicatorLightGlowManager.SetGroupGlow(true);
        if(iceBluePrint!=null) iceBluePrint.SetActive(true);
        IndicatorLightGlowManager.SetGlowColor(Color.red);
        machine.ActiveMachine();
        isActive = true;
    }
}
