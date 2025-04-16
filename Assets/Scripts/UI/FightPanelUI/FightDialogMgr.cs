using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FightDialogMgr : Singleton<FightDialogMgr>
{
    private TextMeshProUGUI m_TextMeshPro;
    private void Start()
    {
        m_TextMeshPro = GetComponent<TextMeshProUGUI>();
    }
}
