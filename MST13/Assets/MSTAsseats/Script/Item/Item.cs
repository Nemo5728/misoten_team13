//稲垣
//2017/10/17
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class Item :  TrueSyncBehaviour
{

    [AddTracking]
    bool bUse;
    private GameObject particle;
    private float time;
    private FP UseTime;
    void Start()
    {
        bUse = true;
        UseTime = 0f;
    }

    void Update()
    {
        
    }

    public override void OnSyncedStart()
    {
        particle = GameObject.Find("ParticleItemAppear");
        time = 0f;

        if(this.gameObject.tag == "ItemLoveUp")
        {
            particle.GetComponent<ParticleManager>().Play("FX_ItemAppear_core", new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z));
        }
        else if (this.gameObject.tag == "ItemPower")
        {
            particle.GetComponent<ParticleManager>().Play("FX_ItemAppear_core", new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z));
        }
        else if (this.gameObject.tag == "ItemMiniUp")
        {
            particle.GetComponent<ParticleManager>().Play("FX_ItemAppear_core", new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z));
        }
        else if (this.gameObject.tag == "ItemSpeed")
        {
            particle.GetComponent<ParticleManager>().Play("FX_ItemAppear_core", new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z));
        }
        /*
        if(tag == "ItemLoveUp")
        {
            GetComponent<ParticleManager>().Play("FX_ItemAppear_Gauge", new Vector3(transform.position.x,
                                                                                                   transform.position.y + 2f,
                                                                                                   transform.position.z));
        }
        else if (tag == "ItemPower")
        {
            GetComponent<ParticleManager>().Play("FX_ItemAppear_Power", new Vector3(transform.position.x,
                                                                                                   transform.position.y + 2f,
                                                                                                   transform.position.z));
        }
        else if (tag == "ItemMiniUp")
        {
            GetComponent<ParticleManager>().Play("FX_ItemAppear_Respawn", new Vector3(transform.position.x,
                                                                                                   transform.position.y + 2f,
                                                                                                   transform.position.z));
        }
        else if (name == "Item_Speed(Clone)")
        {
            GetComponent<ParticleManager>().Play("FX_ItemAppear_Speed", new Vector3(transform.position.x,
                                                                                                   transform.position.y + 2f,
                                                                                                   transform.position.z));
        }
        */

        GameObject target = GameObject.FindWithTag("SetAR");
        transform.SetParent(target.transform);

        bUse = true;
    }
    public override void OnSyncedUpdate()
    {
        
        if(!bUse)
        {
          
          /*
            if (tag == "ItemLoveUp")
            {
                GetComponent<ParticleManager>().Play("FX_itemGot_Gauge", new Vector3(transform.position.x,
                                                                                                   transform.position.y + 2f,
                                                                                                   transform.position.z));
            }
            else if (tag == "ItemPower")
            {
                GetComponent<ParticleManager>().Play("FX_itemGot_Power", new Vector3(transform.position.x,
                                                                                                   transform.position.y + 2f,
                                                                                                   transform.position.z));
            }
            else if (tag == "ItemMiniUp")
            {
                GetComponent<ParticleManager>().Play("FX_itemGot_Respawn", new Vector3(transform.position.x,
                                                                                                   transform.position.y + 2f,
                                                                                                   transform.position.z));
            }
            else if (tag== "ItemSpeed")
            {
                GetComponent<ParticleManager>().Play("FX_itemGot_Speed", new Vector3(transform.position.x,
                                                                                                   transform.position.y + 2f,
                                                                                                   transform.position.z));
            }
*/
            TrueSyncManager.SyncedDestroy(gameObject);
        }
        else{
            UseTime += TrueSyncManager.DeltaTime;

            if (UseTime >= 10f)
            {
                UseTime = 0f;
                TrueSyncManager.SyncedDestroy(gameObject);
            }
        }
    }

    private void OnSyncedCollisionEnter(TSCollision col)
    {
        //playerに触れたら
        if(col.gameObject.tag == "Player")
        {
            bUse = false;
           
        }
    }
    /*

    public Vector3[] posItemTex;        //計算用座標（位置座標）
    public GameObject[] ItemTex;        //オブジェクト本体（プレハブ）
    const int itemNam_Max = 5;          //最大数を設定
    private GameObject[] ItemCreator;

    //スケール
    [SerializeField]
    private Vector3 scale;


    // Use this for initialization
    void Start () {

        posItemTex = new Vector3[itemNam_Max];//生成
    
        //乱数を生成
        float low = -Screen.width/200;
        float high = Screen.width/200;
        for (int i = 0; i < itemNam_Max; i++)
        {
            //座標を乱数で設定
            posItemTex[i] = new Vector3(Random.Range(low, high), 0.0f, Random.Range(low, high));//位置
        }

        ItemCreator = new GameObject[itemNam_Max];

        for (int i = 0; i < itemNam_Max; i++)
        {
            GameObject itemObj = Instantiate(ItemTex[i]);
            // itemを子要素にする
            itemObj.transform.parent = this.transform;
            this.transform.GetChild(i).GetComponent<GameObject>();
            itemObj.name = "item" + i.ToString();               //名前セット
            ItemCreator[i] = itemObj;                           //オブジェクト割り振り
            ItemCreator[i].transform.position = posItemTex[i];  //初期座標設定
            //ItemCreator[i].transform.localScale = new Vector3(scale.x, scale.y, scale.z);//スケール



            //menuCreator[i].GetComponent<menuContror>().SetTargetPos(posMenuTex[i]);//初期ターゲット座標設定
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    //------------------------------------------------------------------------------------------
    // トリガーとの接触時に呼ばれるコールバック
    //------------------------------------------------------------------------------------------
    void OnTriggerEnter(Collider hit)
    {
        // 接触対象はPlayerタグですか？
        if (hit.CompareTag("Player"))
        {
            // このコンポーネントを持つGameObjectを破棄する
            Destroy(gameObject);

        }
    }
    */
}
