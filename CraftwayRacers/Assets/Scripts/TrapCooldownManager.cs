using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrapCooldownManager : MonoBehaviour
{
    [SerializeField] private int trapCldwnSeconds;
    private int tChoice;

    [SerializeField] private Button thumbtack;
    [SerializeField] private Button trap2;
    [SerializeField] private Button trap3;

    [SerializeField] private GameObject iPadUI;

    public void StartCooldown(int trapNum)
    {
        switch(trapNum)
        {
            case 0:
                StartCoroutine(ThumbtackCooldown());
                gameObject.GetComponent<RandomTrapPlacement>().TrapCaller(tChoice, trapNum);
                iPadUI.SetActive(false);
                break;
            case 1:
                StartCoroutine(Trap2Cooldown());
                gameObject.GetComponent<RandomTrapPlacement>().TrapCaller(tChoice, trapNum);
                iPadUI.SetActive(false);
                break;
            case 2:
                StartCoroutine(Trap3Cooldown());
                gameObject.GetComponent<RandomTrapPlacement>().TrapCaller(tChoice, trapNum);
                iPadUI.SetActive(false);
                break;
        }
    }

    private IEnumerator ThumbtackCooldown()
    {
        thumbtack.interactable = false;
        yield return new WaitForSeconds(trapCldwnSeconds);
        thumbtack.interactable = true;
    }    
    
    private IEnumerator Trap2Cooldown()
    {
        trap2.interactable = false;
        yield return new WaitForSeconds(trapCldwnSeconds);
        trap2.interactable = true;
    }    
    
    private IEnumerator Trap3Cooldown()
    {
        trap3.interactable = false;
        yield return new WaitForSeconds(trapCldwnSeconds);
        trap3.interactable = true;
    }

    public void OpenTrapChoice(int posChoice)
    {
        tChoice = posChoice;
        iPadUI.SetActive(true);
    }

    public void Back()
    {
        iPadUI.SetActive(false);
        tChoice = -1;
    }
}
