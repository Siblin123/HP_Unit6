using Unity.Netcode;
using UnityEngine;

public class captain : Enemy
{
    //4. ���ƴٴϸ鼭 �÷��̾� �������� �̵�
  
    [Header("���ο� �÷��̾ ã�� �ð�")]
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
        //�ִϸ��̼� ����
        //�̵��ӵ� ����
        //���̳� �÷��̾ ���������� ������ �Ѵ�
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

    //Ư�� �ð� ���� Ÿ�ٺ���
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

            // ������ Ŭ���̾�Ʈ ����
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
