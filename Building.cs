using UnityEngine;

public class Building : MonoBehaviour
{
    public static int One;

    public GameObject[] Planks;
    public GameObject Enemy;

    public float Plus_x = 1.7f; // ��ĭ �̵��� ���ؾ� �� x���� ũ��
    public float Plus_y = 2.12f; // ���� �̵��� ���ؾ� �� y���� ũ��

    AngryGameMananger angry;

    public int Max_Length = 5; // �ִ� ������ ����
    int floor = 0; 

    Vector2 StartPos = new Vector2(0, -1.89f);

    private void Start()
    {
        angry = GetComponent<AngryGameMananger>();

        Building_Porcess();
    }

    public void Building_Porcess() // �� ������ ���� ����
    {
        int w = Random.Range(5, Max_Length); // ���� ĭ�� ����
        int Enemy_loc = Random.Range(0 , w); // ���� ���� ��ġ 

        int ori_w = w; // ���� �ٲ�� ���� ����
        int floor_dif = 0; // ���� ���� �Ʒ����� ���� ����

        for (int i = 0 ; i < 3 ; i++)
        {
            Floor_Set(w, i , Enemy_loc , floor_dif);

            w = Random.Range(1, w);
            Enemy_loc = Random.Range(0 , w);

            if(i != 2)
            {
                floor_dif += ori_w - w; // floor_dif = �� ĭ�� ���� ����
                ori_w = w;
            }

            
          
        }
            
        

    }

    void Floor_Set(int Length , int floor , int Enemy_loc , int floor_dif) // �ִ� ���� ���� , ���� ���� ���� �޾� �´� , ���� ��ȯ�� ĭ�� �� , ĭ�� ���̸� �޾� �´�
    {
        // pos = (���� ����) + (�ö� ���� * ���� ��) + ((���� �̵� �Ÿ� / 2) * ���� ���� �Ʒ����� ����)
        Vector2 pos = StartPos + new Vector2(0, Plus_y * floor) + new Vector2((Plus_x / 2) * floor_dif , 0); 
        GameObject loc; // ���� ��ȯ�� ��ġ

        for (int i = 0; i < Length ; i++)
        {
            if (i == 0) loc = Instantiate(Planks[0], pos, Quaternion.identity);
            else loc = Instantiate(Planks[1], pos, Quaternion.identity);

            if(i == Enemy_loc) Instantiate(Enemy , loc.transform.position , Quaternion.identity);

            pos += new Vector2(Plus_x, 0);
        }

    }





}
