using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instructionManager : MonoBehaviour
{
    public bool BirdMode;
    public GameObject[] instructions;
    
    public bool PlantPlaced;
    public bool BirdCalled;
    public void showInstruction(int instructionNo)
    {
        for (int i = 0; i < instructions.Length; i++)
        {
            if (instructionNo == i)
            {
                instructions[i].SetActive(true);
            }
            else
            {
                instructions[i].SetActive(false);
            }
        }
    }

    public IEnumerator showFinalInstructions()
    {
        if (BirdMode)
        {
            showInstruction(4);
            while (!PlantPlaced)
                yield return null;
            showInstruction(-1);
            yield return new WaitForSeconds(2f);
            showInstruction(3);
            yield return new WaitForSeconds(3f);
            //while (!BirdCalled)
            //    yield return null;
            showInstruction(-1);
        }
        else
        {
            showInstruction(3);
            yield return new WaitForSeconds(5);
            showInstruction(-1);
            yield return new WaitForSeconds(2f);
            showInstruction(4);
            yield return new WaitForSeconds(5);
            showInstruction(-1);
            
        }
    }

}
