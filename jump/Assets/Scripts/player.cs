using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    public GameObject stand_box;//当前player所在盒子
    public float maxDis;//生成盒子与上一个盒子的最大距离
    public GameObject _box;//获取生成的盒子对应的预制体
    public float Factor;//实现速度基数
    public Transform transform1;//获取player中空物体的位置
    public Transform head;//记录player的头部位置
    public Transform body;//记录player的身体位置
    public Transform Camera;//相机位置，实现相机跟随时用到
    public Text sco;//记录分数
    public GameObject lizi;//例子特效

    private SqliteDataReader dbReader;
    public Button startGame;
    bool isStart;
    public Image startPanel;
    public Button restartGame;
    public Image endPanel;
    public Button registerBtn;//注册按钮
    public Button loginBtn;//登录按钮
    public Button shopBtn;//商店按钮
    public Image shopPanel;//商店面板
    public Button closeShopBtn;//关闭商店面板
    public InputField acountInp;//账号输入
    public InputField passwordInp;//密码输入

    public Button[] buyBtn;

    public Image tipPanel;
    public Button tipReturn;

    private Vector3 _dir = new Vector3(1, 0, 0);//生成第二个盒子的方向变量
    private Rigidbody rig1;//获取player刚体组件
    private float start_time;//记录按下开始space的时间
    private GameObject new_box;//新生成的盒子
    private static Vector3 _cameraRelativeTranfrom;//相机需要移动距离
    //记录player位置和大小及当前盒子大小
    Vector3 _head = new Vector3(0, 0, 0);
    Vector3 _body = new Vector3(0, 0, 0);
    Vector3 _stand_box = new Vector3(0, 0, 0);
    private int _sco;
    SQLiteHelp db;
    string dbstr = "Data Source = "+ Application.streamingAssetsPath+"/player.db";
    public Material bodyMaterial;
    public Text diama;
    // Start is called before the first frame update
    void Start()
    {
        lizi.SetActive(false);//默认粒子特效为关闭状态
        startGame.onClick.AddListener(StartGame);
        restartGame.onClick.AddListener(RestartGame);
        registerBtn.onClick.AddListener(RegisterPlayer);
        shopBtn.onClick.AddListener(OpenShop);
        closeShopBtn.onClick.AddListener(CloseShop);
        tipReturn.onClick.AddListener(CloseTip);
        db = new SQLiteHelp(dbstr);
        db.OpenSQLite(dbstr);
        Debug.Log(db.ToString());
        for (int i = 0; i < buyBtn.Length; i++)
        {
            buyBtn[i].onClick.AddListener(BuySkin);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))//判断是否按下space
            {
                lizi.SetActive(true);//开启粒子特效
                _head = head.transform.localPosition;//记录缩放前头部位置
                _body = body.transform.localScale;//记录缩放前身体大小
                _stand_box = stand_box.transform.localScale;//记录盒子大小
                start_time = Time.time;//记录按下space的时间
            }

            if (Input.GetKeyUp(KeyCode.Space))//判断是否松开按键
            {
                lizi.SetActive(false);//关闭粒子特效
                var time = Time.time - start_time;//计算按键时间
                jump(time);
                body.transform.localScale = _body;//身体还原
                head.transform.localPosition = _head;
                stand_box.transform.localScale = _stand_box;//盒子还原
            }
            if (Input.GetKey(KeyCode.Space))
            {

                head.transform.localPosition += new Vector3(0, -1, 0) * 0.1f * Time.deltaTime;//头动画缩小
                body.transform.localScale += new Vector3(1, -1, 1) * 0.05f * Time.deltaTime;//身体动画缩小

                stand_box.transform.localScale += new Vector3(1, -1, 1) * 0.05f * Time.deltaTime;//盒子动画缩小

            }
        }
    }
    void RegisterPlayer()
    {
        
    }
    int diamond1;
    void BuySkin()
    {
        var buttonSelf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        Debug.Log(buttonSelf.tag);
        if (buttonSelf.tag == "Orange")
        {
            dbReader = db.ReadSpecificData("playermanage", "diamond", acountInp.text, passwordInp.text);
            if (dbReader.Read())
            {
                diamond1 = int.Parse(dbReader.GetValue(2).ToString());
                if (diamond1 >= 100)
                {
                    db.UpdateInto("playermanage", "diamond", diamond - 100);
                    buttonSelf.transform.GetChild(0).GetComponent<Text>().text = "穿戴";
                    buttonSelf.GetComponent<Button>().onClick.RemoveListener(BuySkin);
                    buttonSelf.GetComponent<Button>().onClick.AddListener(WearSkin);
                }
                else
                {
                    tipPanel.gameObject.SetActive(true);
                    Debug.Log(diamond1);
                    diama.text = diamond1.ToString();
                }
            }
        }
        if (buttonSelf.tag == "Red")
        {
            dbReader = db.ReadSpecificData("playermanage", "diamond", acountInp.text, passwordInp.text);
            if (dbReader.Read())
            {
                diamond1 = int.Parse(dbReader.GetValue(2).ToString());
                if (diamond1 >= 150)
                {
                    db.UpdateInto("playermanage", "diamond", diamond - 150);
                    buttonSelf.transform.GetChild(0).GetComponent<Text>().text = "穿戴";
                    buttonSelf.GetComponent<Button>().onClick.RemoveListener(BuySkin);
                    buttonSelf.GetComponent<Button>().onClick.AddListener(WearSkin);
                }
                else
                {
                    tipPanel.gameObject.SetActive(true);
                    diama.text = diamond1.ToString();
                }
            }
        }
        if (buttonSelf.tag == "Purple")
        {
            dbReader = db.ReadSpecificData("playermanage", "diamond", acountInp.text, passwordInp.text);
            if (dbReader.Read())
            {
                diamond1 = int.Parse(dbReader.GetValue(2).ToString());
                if (diamond1 >= 200)
                {
                    db.UpdateInto("playermanage", "diamond", diamond - 200);
                    buttonSelf.transform.GetChild(0).GetComponent<Text>().text = "穿戴";
                    buttonSelf.GetComponent<Button>().onClick.RemoveListener(BuySkin);
                    buttonSelf.GetComponent<Button>().onClick.AddListener(WearSkin);
                }
                else
                {
                    tipPanel.gameObject.SetActive(true);
                    diama.text = diamond1.ToString();
                }
            }
        }
    }
    void WearSkin()
    {
        var buttonSelf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (buttonSelf.tag == "Orange")
        {
            Color color = new Color(0.88f, 0.7f, 0.55f, 1);
            bodyMaterial.color = color;
        }
        if (buttonSelf.tag == "Red")
        {
            Color color = new Color(0.56f, 0.49f, 0.5f, 1);
            bodyMaterial.color = color;
        }
        if (buttonSelf.tag == "Purple")
        {
            Color color = new Color(0.75f, 0.44f, 0.98f, 1);
            bodyMaterial.color = color;
        }
    }
    void CloseTip()
    {
        tipPanel.gameObject.SetActive(false);
    }
    void OpenShop()
    {
        endPanel.gameObject.SetActive(false);
        shopPanel.gameObject.SetActive(true);
    }
    void CloseShop()
    {
        endPanel.gameObject.SetActive(true);
        shopPanel.gameObject.SetActive(false);
    }
    bool isPlayer;
    void StartGame()
    {
        IDataReader sqReader = db.ReadFullTable("playermanage",acountInp.text,passwordInp.text);
        if (sqReader.Read())
        {
            isStart = true;
            startPanel.gameObject.SetActive(false);
            sco.gameObject.SetActive(true);
            _sco = 0;//分数初始化
            sco.text = _sco.ToString();//分数更新
            _cameraRelativeTranfrom = Camera.position - transform.position;//计算相机对于player的相对位置
            rig1 = GetComponent<Rigidbody>();//获取player的Rigidbody组件，因为此脚本挂在player上，所以默认获取player的刚体组件，可以查资料学习一下怎样获取指定物体组件
            rig1.centerOfMass = transform1.localPosition;//将物体重心放在player中空物体的位置
            putBox();//调用生成盒子函数
        }
    }
    void RestartGame()
    {
        SceneManager.LoadScene(0);//重新开始游戏
        endPanel.gameObject.SetActive(false);
    }
    void putBox()
    {
        Random_dir();//调用随机生成盒子方向函数
        new_box = Instantiate(_box);//实例化盒子
        new_box.transform.position = stand_box.transform.position + _dir * Random.Range(3f, maxDis);//设置盒子位置
        var a = Random.Range(0.5f, 1.5f);//生成一个随机数
        new_box.transform.localScale = new Vector3(a, -0.63f, a);//改变盒子scale（尺寸）中的x（长）与z（宽）全部为生成的随机数
    }

    void Random_dir()//随机盒子生成方向
    {
        var a = Random.Range(0, 2);//随机生成0，1两个数
        if (a == 0)
        {
            _dir = new Vector3(1, 0, 0);
        }
        else
        {
            _dir = new Vector3(0, 0, 1);
        }
    }

    void jump(float time)
    {
        //给player添加瞬间力
        //Factor为控制player跳跃速度的控制基数
        rig1.AddForce((new Vector3(0, 2, 0) + _dir) * time * Factor, ForceMode.Impulse);
    }
    int diamond;
    void OnCollisionEnter(Collision collision)//碰撞事件函数
    {
        Debug.Log(collision.gameObject.name);//打印与player碰撞的物体的名字
        Collider collider_box = stand_box.GetComponent<Collider>();//获取player原来所在盒子的碰撞器
        if (collision.gameObject.name.Contains("Box") && collider_box != collision.collider)//判断是否与盒子碰撞且碰撞的盒子是否不同于上一个盒子
        {
            _sco++;//分数加一
            sco.text = _sco.ToString();//将分数更新
            dbReader = db.ReadSpecificData("playermanage", "diamond",acountInp.text,passwordInp.text);
            if (dbReader.Read()) {
                diamond = int.Parse(dbReader.GetValue(2).ToString());
                Debug.Log(diamond);
                db.UpdateInto("playermanage", "diamond", diamond + 1);
            }
            MoveCamera();
            GameObject destory_box = stand_box;//定义将要销毁的盒子
            stand_box = new_box;//将player所在盒子进行刷新
            putBox();//生成新的盒子
            Destroy(destory_box);

        }
        if (collision.gameObject.name == "ground")
        {
            endPanel.gameObject.SetActive(true);
            isStart = false;
        }
    }

    void MoveCamera()//相机跟随函数
    {
        Camera.position = transform.position + _cameraRelativeTranfrom;//移动相机：相机位置等于小人目前位置加相对位置
    }
}
