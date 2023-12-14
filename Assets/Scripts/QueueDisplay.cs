using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QueueDisplay : MonoBehaviour
{
    preTetros[] prefabedTetros = new preTetros[7];
    private int firstIndex, secondIndex, thirdIndex;
    private int prevFirst,prevHolded;
    public Board board;
    public Queue<int> temp = new Queue<int>();
    public GameObject[] prefabs = new GameObject[7];
    private GameObject temp1, temp2, temp3,temp4;

   private struct preTetros
   {
       public Vector3[] positions;
       public GameObject prefab;
   }

    private void UpdatePrefabPositions()
    {
        temp1.transform.position = prefabedTetros[firstIndex].positions[0];
        temp2.transform.position = prefabedTetros[secondIndex].positions[1];
        temp3.transform.position = prefabedTetros[thirdIndex].positions[2];
    }

    private void DestroyPreviousInstances()
    {
        if (temp1 != null)
            Destroy(temp1);
        if (temp2 != null)
            Destroy(temp2);
        if (temp3 != null)
            Destroy(temp3);
    }

    private void Awake()
    {
        prevHolded = board.holdedIndex;
        temp = board.indexHolder;
        for(int i = 0; i < prefabedTetros.Length; i++)
        {
            prefabedTetros[i].prefab = prefabs[i];
            prefabedTetros[i].positions = new Vector3[4];
        }
        ///////////////////////////////////////////////////////////////////
        //positions for I
        prefabedTetros[0].positions[0] = new Vector3(21f, -2.75f, 0f); 
        prefabedTetros[0].positions[1] = new Vector3(21f, -6.75f, 0f);
        prefabedTetros[0].positions[2] = new Vector3(21f, -10.75f, 0f);
        //for hold
        prefabedTetros[0].positions[3] = new Vector3(21f, 9.25f, 0f);
        ///////////////////////////////////////////////////////////////////
        //positions for O
        prefabedTetros[1].positions[0] = new Vector3(20.75f, -2.25f, 0f);
        prefabedTetros[1].positions[1] = new Vector3(20.75f, -6.25f, 0f);
        prefabedTetros[1].positions[2] = new Vector3(20.75f, -10.25f, 0f);
        //for hold
        prefabedTetros[1].positions[3] = new Vector3(21f, 9.75f, 0f);
        ///////////////////////////////////////////////////////////////////
        //positions for T
        prefabedTetros[2].positions[0] = new Vector3(21f, -1.75f, 0f);
        prefabedTetros[2].positions[1] = new Vector3(21f, -5.75f, 0f);
        prefabedTetros[2].positions[2] = new Vector3(21f, -9.75f, 0f);
        //for hold
        prefabedTetros[2].positions[3] = new Vector3(21f, 10.25f, 0f);
        ///////////////////////////////////////////////////////////////////
        //positions for J
        prefabedTetros[3].positions[0] = new Vector3(21.25f, -2.5f, 0f);
        prefabedTetros[3].positions[1] = new Vector3(21.25f, -6.5f, 0f);
        prefabedTetros[3].positions[2] = new Vector3(21.25f, -10.5f, 0f);
        //for hold
        prefabedTetros[3].positions[3] = new Vector3(21.25f, 9.5f, 0f);
        ///////////////////////////////////////////////////////////////////
        //positions for L
        prefabedTetros[4].positions[0] = new Vector3(20.75f, -2.5f, 0f);
        prefabedTetros[4].positions[1] = new Vector3(20.75f, -6.5f, 0f);
        prefabedTetros[4].positions[2] = new Vector3(20.75f, -10.5f, 0f);
        //for hold
        prefabedTetros[4].positions[3] = new Vector3(20.75f, 9.5f, 0f);
        ///////////////////////////////////////////////////////////////////
        //positions for S
        prefabedTetros[5].positions[0] = new Vector3(20.5f, -2.25f, 0f);
        prefabedTetros[5].positions[1] = new Vector3(20.5f, -6.25f, 0f);
        prefabedTetros[5].positions[2] = new Vector3(20.5f, -10.25f, 0f);
        //for hold
        prefabedTetros[5].positions[3] = new Vector3(20.5f, 9.75f, 0f);
        ///////////////////////////////////////////////////////////////////
        //positions for Z
        prefabedTetros[6].positions[0] = new Vector3(21f, -2.25f, 0f);
        prefabedTetros[6].positions[1] = new Vector3(21f, -6.25f, 0f);
        prefabedTetros[6].positions[2] = new Vector3(21f, -10.25f, 0f);
        //for hold
        prefabedTetros[6].positions[3] = new Vector3(21f, 9.75f, 0f);
        ///////////////////////////////////////////////////////////////////

    }
    //displays prefabs in queue and hold blocks
    public void Update()
    {
        temp = new Queue<int>(board.indexHolder);
        if (temp.Count > 0)
        {
            firstIndex = board.indexHolder.ElementAt(0);
            secondIndex = board.indexHolder.ElementAt(1);
            thirdIndex = board.indexHolder.ElementAt(2);

        }
        if (prevFirst != firstIndex)
        {
            DestroyPreviousInstances();
        }
        prevFirst = firstIndex;
        if (temp1 == null)
        {
            temp1 = Instantiate(prefabedTetros[firstIndex].prefab, prefabedTetros[firstIndex].positions[0], Quaternion.identity);
        }
        if (temp2 == null)
        {
            temp2 = Instantiate(prefabedTetros[secondIndex].prefab, prefabedTetros[secondIndex].positions[1], Quaternion.identity);
        }
        if (temp3 == null)
        {
            temp3 = Instantiate(prefabedTetros[thirdIndex].prefab, prefabedTetros[thirdIndex].positions[2], Quaternion.identity);
        }
        if (board.holdedIndex != 10)
        {
            if(temp4 == null)
                temp4 = Instantiate(prefabedTetros[board.holdedIndex].prefab, prefabedTetros[board.holdedIndex].positions[3], Quaternion.identity);
        }

        if (prevHolded != board.holdedIndex)
        {
            if (temp4 != null)
                Destroy(temp4);
        }

        prevHolded = board.holdedIndex;
        UpdatePrefabPositions();
    }
}
