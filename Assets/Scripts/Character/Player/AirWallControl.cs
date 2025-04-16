using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirWallControl : MonoBehaviour
{
    private Material material;
    private GameObject player;

    // ��������
    [SerializeField] private float maxDistance = 10f;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().materials[0];
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if (player != null)
        {
            // ������ҺͿ���ǽ֮��ľ���
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer > maxDistance)
            {
                return;
            }
            else
            {
                //Debug.Log(Vector2.Distance(gameObject.transform.position, player.transform.position));
                material.SetVector("_PlayerPos", player.transform.position);
            }
        }
    }
}
