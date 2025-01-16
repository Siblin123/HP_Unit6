using Unity.Netcode;
using UnityEngine;

public class captain : Enemy
{
    //4. 날아다니면서 플레이어 방향으로 이동
  
    [Header("새로운 플레이어를 찾는 시간")]
    public float random_Player_FindTime;
    [SerializeField] float curFindTime;
    // Update is called once per frame
    void Update()
    {
        Setting_RandomPlayer();
        Fly();
    }

    public void Fly()
    {
        if (tart_player == null)
            return;
        //애니메이션 변경
        //이동속도 변경
        //벽이나 플레이어가 있을때까지 직진을 한다
        view_dir = tart_player.transform.position - transform.position;
        view_dir.Normalize();
        transform.Translate(curSpeed * view_dir * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerControl>().Get_damage(damege,transform);
        }
    }

    //특정 시간 마다 타겟변경
    private void Setting_RandomPlayer()
    {

        if(random_Player_FindTime>= curFindTime)
        {
            var connectedClients = NetworkManager.Singleton.ConnectedClientsList;

            if (connectedClients.Count == 0)
            {
                Debug.LogWarning("No players are currently connected.");
                tart_player = null;
                return;
            }

            // 랜덤한 클라이언트 선택
            int randomIndex = Random.Range(0, connectedClients.Count);

            for (int i = 0; i < connectedClients.Count; i++)
            {
                if (tart_player == null)
                {
                    tart_player = connectedClients[i].PlayerObject.GetComponent<PlayerControl>();
                }
                else
                {
                    if (Vector2.Distance(tart_player.transform.position, transform.position) > Vector2.Distance(connectedClients[i].PlayerObject.transform.position, transform.position))
                    {
                        tart_player = connectedClients[i].PlayerObject.GetComponent<PlayerControl>();
                    }
                }


            }
        }
        else
        {
            curFindTime += Time.deltaTime;
        }
       



    }

}
