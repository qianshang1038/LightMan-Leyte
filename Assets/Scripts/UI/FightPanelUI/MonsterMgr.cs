using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterMgr : Singleton<MonsterMgr>
{
    private List<BaseMonster2D> Monsters = new List<BaseMonster2D>();
    public TextMeshProUGUI CurrentMonsterInfo;

    private int currentIndex = 0;

    void Start()
    {
        for(int i = 0; i < transform.childCount; i++) 
        {
            Monsters.Add(transform.GetChild(i).GetComponent<BaseMonster2D>());
            transform.GetChild(i).GetComponent<BaseMonster2D>().index = i;
            
        }

        setSpriteActive(currentIndex);
        UpdateMonsterInfo(currentIndex);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveToNextMonster();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveToLastMonster();
        }
        
        
    }

    /// <summary>
    /// ����ĸ���
    /// </summary>
    public void UpdateMonsterCount()
    {
       
        Monsters.Clear();
        
        
        for (int i = 0; i < transform.childCount; i++)
        {
            Monsters.Add(transform.GetChild(i).GetComponent<BaseMonster2D>());
            transform.GetChild(i).GetComponent<BaseMonster2D>().index = i;
        }
        if (Monsters.Count >0)
        {
            setSpriteActive(currentIndex);
            UpdateMonsterInfo(currentIndex);
        }
        
    }

    #region �Ӿ�Ч��
    public void UpdateMonsterInfo(int index)
    {
        CurrentMonsterInfo.text=Monsters[index].MonsterInfo();
    }

    private void MoveToNextMonster()
    {
        setSpriteNotActive(currentIndex);
        currentIndex = (currentIndex + 1) % Monsters.Count;
        setSpriteActive(currentIndex);
        UpdateMonsterInfo(currentIndex);
    }

    private void MoveToLastMonster()
    {
        setSpriteNotActive(currentIndex);
        currentIndex = (currentIndex - 1 + Monsters.Count) % Monsters.Count;
        setSpriteActive(currentIndex);
        UpdateMonsterInfo(currentIndex);
    }
    public void MoveToChooseMonster(int index)
    {
        setSpriteNotActive(currentIndex);
        currentIndex = index;
        setSpriteActive(currentIndex);
        UpdateMonsterInfo(currentIndex);
    }

    private void setSpriteActive(int index)
    {
        if (Monsters[index] == null)
        {
            return;
        }
        //TODO ָ�붯Ч
        Monsters[index].SetSpriteActiveOrNot(true);
    }

    private void setSpriteNotActive(int index)
    {
        Monsters[index].SetSpriteActiveOrNot(false);
    }
    #endregion 
    
    public GameObject GetCurrentMonsterObject()
    {
        return Monsters[currentIndex].gameObject;
    }
    public BaseMonster2D GetCurrentMonster()
    {
        return Monsters[currentIndex];
    }
    public List<BaseMonster2D> GetMonsters()
    {
        return Monsters;
    }
    /// <summary>
    /// �ݶ�ÿ���ֶ�ʹ��һ�鼼��
    /// </summary>
    public IEnumerator MakeMonsterDoSkill()
    {
        //TODO ����aiǿ��ִ��(�����Ѿ�����)
        for(int i=0;i< Monsters.Count; i++)
        {
            UpdateMonsterCount();
            yield return new WaitForSeconds(1f);
            if(!IsMonsterFail()&&Player2D.Instance.IsAlive())
            {
                if (Monsters[i].state == 0)
                    Monsters[i].MonsterSkill();
                else
                {
                    Monsters[i].state = 0;
                }
                
            }
        }
    }
    /// <summary>
    /// �������Ƿ�ʧ��
    /// </summary>
    public bool IsMonsterFail()
    {
        UpdateMonsterCount();
        return Monsters.Count == 0;
    }
}
