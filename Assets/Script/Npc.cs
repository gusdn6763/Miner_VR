using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Npc : MonoBehaviour
{
    [SerializeField] float ConversationChangeTime = 2f;
    [SerializeField] List<string> conversation;
    [SerializeField] Text npcConversation;

    public void Start()
    {
        StartCoroutine(ConversationChangeCoroution());
    }

    public void Update()
    {
        Vector3 targetPosition = new Vector3(Player.instance.transform.position.x, transform.position.y, Player.instance.transform.position.z);
        transform.LookAt(targetPosition);
    }

    IEnumerator ConversationChangeCoroution()
    {
        int i = 0;
        while (true)
        {
            npcConversation.text = conversation[i].ToString();
            yield return new WaitForSeconds(ConversationChangeTime);
            i++;
            if (i >= conversation.Count)
            {
                i = 0;
            }
        }
    }
}
