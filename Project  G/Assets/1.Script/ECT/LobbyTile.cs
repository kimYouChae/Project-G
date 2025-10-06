using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyTile : MonoBehaviour
{
    [SerializeField] private GameObject[] tileList;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float resetY;
    [SerializeField] private float startY;

    private void Update()
    {
        foreach (GameObject tile in tileList)
        {
            // 아래로 이동
            tile.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

            // 특정 위치보다 내려갔으면 다시 위로 올리기
            if (tile.transform.position.y <= resetY)
            {
                Vector3 pos = tile.transform.position;
                pos.y = startY;
                tile.transform.position = pos;
            }
        }
    }
}
