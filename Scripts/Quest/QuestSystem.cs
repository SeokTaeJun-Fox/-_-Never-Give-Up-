using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{
    #region Save Path
    private const string kSaveRootPath = "questSystem";
    private const string kActiveQuestsSavePath = "activeQuests";
    private const string kCompletedQuestsSavePath = "completedQuests";
    private const string kActiveAchievementsSavePath = "activeAchievements";
    private const string kCompletedAchievementsSavePath = "completedAchievements";
    #endregion

    #region Events
    public delegate void QuestRegisteredHandler(Quest newQuest);
    public delegate void QuestCompletedHandler(Quest quest);
    public delegate void QuestCompletedReadyHandler(Quest quest);
    public delegate void QuestCanceledHandler(Quest quest);

    public delegate void QuestInitialHandler();
    #endregion

    private static QuestSystem instance;
    private static bool isApplicationQuitting;

    public static QuestSystem Instance
    {
        get
        {
            if (!isApplicationQuitting && instance == null)
            {
                instance = FindObjectOfType<QuestSystem>();
                if (instance == null)
                {
                    instance = new GameObject("Quest System").AddComponent<QuestSystem>();
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
            return instance;
        }
    }

    [SerializeField] private List<Quest> activeQuests = new List<Quest>();
    [SerializeField] private List<Quest> completedQuests = new List<Quest>();

    private List<Quest> activeAchievements = new List<Quest>();
    private List<Quest> completedAchievements = new List<Quest>();

    private QuestDatabase questDatabase;
    private QuestDatabase achievementDatabase;

    public event QuestRegisteredHandler onQuestRegistered;
    public event QuestCompletedHandler onQuestCompleted;
    public event QuestCompletedReadyHandler onQuestCompletedReady;
    public event QuestCanceledHandler onQuestCanceled;

    public event QuestRegisteredHandler onAchievementRegistered;
    public event QuestCompletedHandler onAchievementCompleted;

    public event QuestInitialHandler onQuestInitial;

    public IReadOnlyList<Quest> ActiveQuests => activeQuests;
    public IReadOnlyList<Quest> CompletedQuests => completedQuests;
    public IReadOnlyList<Quest> ActiveAchievements => activeAchievements;
    public IReadOnlyList<Quest> CompletedAchievements => completedAchievements;

    public QuestDatabase QuestDatabase => questDatabase;

    private void Awake()
    {
        questDatabase = Resources.Load<QuestDatabase>("QuestDatbase");
        achievementDatabase = Resources.Load<QuestDatabase>("AchievementDatabase");

        //if (!Load())
        //{
        //    foreach (var achievement in achievementDatabase.Quests)
        //        Register(achievement);
        //}
    }

    private void OnApplicationQuit()
    {
        isApplicationQuitting = true;
        //Save();
    }

    //퀘스트 등록(등록된 퀘스트 오브젝트는 복제된 상태로 반환됩니다)
    public Quest Register(Quest quest)
    {
        var newQuest = quest.Clone();

        if (newQuest is Achievement)
        {
            newQuest.onCompleted += OnAchievementCompleted;
            
            activeAchievements.Add(newQuest);

            newQuest.OnRegister();
            onAchievementRegistered?.Invoke(newQuest);
            newQuest.LateOnRegister();
        }
        else
        {
            newQuest.onCompleted += OnQuestCompleted;
            newQuest.onCompleteReady += OnQuestCompletedReady;
            newQuest.onCanceled += OnQuestCanceled;

            activeQuests.Add(newQuest);

            newQuest.OnRegister();
            onQuestRegistered?.Invoke(newQuest);
            newQuest.LateOnRegister();
        }

        return newQuest;
    }

    //보고 받는다
    public void ReceiveReport(string category, object target, int successCount)
    {
        ReceiveReport(activeQuests, category, target, successCount);
        ReceiveReport(activeAchievements, category, target, successCount);
    }

    public void ReceiveReport(Category category, TaskTarget target, int successCount)
        => ReceiveReport(category.CodeName, target.Value, successCount);

    private void ReceiveReport(List<Quest> quests, string category, object target, int successCount)
    { 
        //ToArray로 List의 사본을 만들어서 for문을 돌리는 이유는 
        //for문이 돌아가는 와중에 Quest가 Complete되어
        //목록에서 빠질 수도 있기 때문입니다.
        //그러므로 원본을 이용해서 돌릴경우 오류가 뜰 수 있습니다.
        foreach (var quest in quests.ToArray())
            quest.ReceiveReport(category, target, successCount);
    }

    public void CompleteWaitingQuests()
    {
        foreach (var quest in activeQuests.ToList())
        {
            if (quest.isComplatable)
                quest.Complete();
        }
    }

    //이 퀘스트가 활성화되어있는지 확인한다.
    public bool ContainsInActiveQuests(Quest quest) => activeQuests.Any(x => x.CodeName == quest.CodeName);

    //이 퀘스트가 완료되어있는지 확인한다.
    public bool ContainsInCompleteQuests(Quest quest) => completedQuests.Any(x => x.CodeName == quest.CodeName);

    //이 업적이 활성화되어있는지 확인한다.
    public bool ContainsInActiveAchievements(Quest quest) => activeAchievements.Any(x => x.CodeName == quest.CodeName);

    //이 업적이 완료되어있는지 확인한다.
    public bool ContainsInCompletedAchievements(Quest quest) => completedAchievements.Any(x => x.CodeName == quest.CodeName);

    //이 퀘스트가 완료대기상태인지 확인합니다.
    public bool ContainsInCompleteReadyQuests(Quest quest) => ActiveQuests.Any(x => (x.CodeName == quest.CodeName) && x.State == QuestState.WaitingForCompletion);


    public IList<Quest> GetReadyQuests()
    {
        List<Quest> list = questDatabase.Quests.Where(x => !ContainsInActiveQuests(x) && !ContainsInCompleteQuests(x)).ToList();
        return list;
    }

    public string CreateSaveData()
    {
        var root = new JObject();
        root.Add(kActiveQuestsSavePath, CreateSaveDatas(activeQuests));
        root.Add(kCompletedQuestsSavePath, CreateSaveDatas(completedQuests));
        root.Add(kActiveAchievementsSavePath, CreateSaveDatas(activeAchievements));
        root.Add(kCompletedAchievementsSavePath, CreateSaveDatas(completedAchievements));

        PlayerPrefs.SetString(kSaveRootPath, root.ToString());
        PlayerPrefs.Save();

        return root.ToString();
    }

    //public bool Load()
    //{
    //    if (PlayerPrefs.HasKey(kSaveRootPath))
    //    {
    //        var root = JObject.Parse(PlayerPrefs.GetString(kSaveRootPath));

    //        LoadSaveDatas(root[kActiveQuestsSavePath], questDatabase, LoadActiveQuest);
    //        LoadSaveDatas(root[kCompletedQuestsSavePath], questDatabase, LoadCompletedQuest);

    //        LoadSaveDatas(root[kActiveAchievementsSavePath], achievementDatabase, LoadActiveQuest);
    //        LoadSaveDatas(root[kCompletedAchievementsSavePath], achievementDatabase, LoadCompletedQuest);

    //        return true;
    //    }
    //    else
    //        return false;
    //}

    public void LoadData(string _data)
    {
        var root = JObject.Parse(_data);

        LoadSaveDatas(root[kActiveQuestsSavePath], questDatabase, LoadActiveQuest);
        LoadSaveDatas(root[kCompletedQuestsSavePath], questDatabase, LoadCompletedQuest);

        LoadSaveDatas(root[kActiveAchievementsSavePath], achievementDatabase, LoadActiveQuest);
        LoadSaveDatas(root[kCompletedAchievementsSavePath], achievementDatabase, LoadCompletedQuest);
    }

    //세이브 데이터 생성
    private JArray CreateSaveDatas(IReadOnlyList<Quest> quests)
    { 
        var saveDatas = new JArray();
        foreach (var quest in quests)
        {
            if (quest.IsSavable)
                saveDatas.Add(JObject.FromObject(quest.ToSaveData()));
        }
        return saveDatas;
    }

    //세이브 데이터 로드
    private void LoadSaveDatas(JToken dataToken, QuestDatabase database, System.Action<QuestSaveData, Quest> onSuccess)
    {
        var datas = dataToken as JArray;
        foreach (var data in datas)
        { 
            var saveData = data.ToObject<QuestSaveData>();
            var quest = database.FindQuestBy(saveData.codeName);
            onSuccess.Invoke(saveData, quest);
        }
    }

    private void LoadActiveQuest(QuestSaveData saveData, Quest quest)
    {
        var newQuest = Register(quest); //퀘스트 등록
        newQuest.LoadFrom(saveData);    //등록된 퀘스트 데이터 불러오기
    }

    private void LoadCompletedQuest(QuestSaveData saveData, Quest quest)
    {
        var newQuest = quest.Clone();
        newQuest.LoadFrom(saveData);

        if (newQuest is Achievement)
            completedAchievements.Add(newQuest);
        else
            completedQuests.Add(newQuest);
    }

    public void Initial()
    {
        activeQuests.Clear();
        activeAchievements.Clear();
        completedQuests.Clear();
        completedAchievements.Clear();

        onQuestInitial?.Invoke();
    }

    #region Callback
    private void OnQuestCompleted(Quest quest)
    {
        Debug.Log($"{quest.CodeName} : QC");
        activeQuests.Remove(quest);
        completedQuests.Add(quest);

        onQuestCompleted?.Invoke(quest);
    }

    private void OnQuestCompletedReady(Quest quest)
    {
        Debug.Log($"{quest.CodeName} : QCR");
        onQuestCompletedReady?.Invoke(quest);
    }

    private void OnQuestCanceled(Quest quest)
    {
        activeQuests.Remove(quest);
        onQuestCanceled?.Invoke(quest);

        Destroy(quest, Time.deltaTime);
    }

    private void OnAchievementCompleted(Quest achievement)
    { 
        activeAchievements.Remove(achievement);
        completedAchievements.Add(achievement);

        onAchievementCompleted?.Invoke(achievement);
    }
    #endregion
}
