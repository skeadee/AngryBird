using UnityEngine;

public class Building : MonoBehaviour
{
    public static int One;

    public GameObject[] Planks;
    public GameObject Enemy;

    public float Plus_x = 1.7f; // 옆칸 이동시 더해야 할 x축의 크기
    public float Plus_y = 2.12f; // 위로 이동시 더해야 할 y축의 크기

    AngryGameMananger angry;

    public int Max_Length = 5; // 최대 가로의 길이
    int floor = 0; 

    Vector2 StartPos = new Vector2(0, -1.89f);

    private void Start()
    {
        angry = GetComponent<AngryGameMananger>();
    }

    public void Building_Porcess() // 적 구조물 생성 로직
    {
        int w = Random.Range(5, Max_Length); // 가로 칸의 갯수
        int Enemy_loc = Random.Range(0 , w); // 적을 놓을 위치 

        int ori_w = w; // 층이 바뀌기 전의 길이
        int floor_dif = 0; // 현재 층과 아래층의 길이 차이

        for (int i = 0 ; i < 3 ; i++)
        {
            Floor_Set(w, i , Enemy_loc , floor_dif);

            w = Random.Range(1, w);
            Enemy_loc = Random.Range(0 , w);

            if(i != 2)
            {
                floor_dif += ori_w - w; // floor_dif = 각 칸의 차이 갯수
                ori_w = w;
            }

            
          
        }
            
        

    }

    void Floor_Set(int Length , int floor , int Enemy_loc , int floor_dif) // 최대 가로 길이 , 현재 층의 값을 받아 온다 , 적을 소환할 칸의 값 , 칸의 차이를 받아 온다
    {
        // pos = (시작 지점) + (올라갈 높이 * 현재 층) + ((가로 이동 거리 / 2) * 현재 층과 아래층이 차이)
        Vector2 pos = StartPos + new Vector2(0, Plus_y * floor) + new Vector2((Plus_x / 2) * floor_dif , 0); 
        GameObject loc; // 적을 소환할 위치

        for (int i = 0; i < Length ; i++)
        {
            if (i == 0) loc = Instantiate(Planks[0], pos, Quaternion.identity);
            else loc = Instantiate(Planks[1], pos, Quaternion.identity);

            if(i == Enemy_loc) Instantiate(Enemy , loc.transform.position , Quaternion.identity);

            pos += new Vector2(Plus_x, 0);
        }

    }





}
