using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    [SerializeField]
    private float durationTime; // 불의 지속시간 
    private float currentDurationTime;

    [SerializeField]
    private float time;         // 익히는데 걸리는 시간
    private float currentTime;
    [SerializeField]
    private GameObject go_CookedItemPrefab; // 완성된 아이템

    // 상태변수
    private bool isFire = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Item")
        {
            currentTime += Time.deltaTime;

            if (currentTime >= time)
            {
                Destroy(other.gameObject);

                Instantiate(go_CookedItemPrefab, other.transform.position, Quaternion.Euler(transform.eulerAngles));
            }
        }
    }

}
