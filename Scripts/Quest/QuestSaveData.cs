public struct QuestSaveData
{
    public string codeName;
    public QuestState state;
    public int taskgroupIndex;  //현재 테스크 그룹을 저장합니다.
    public int[] taskSuccessCounts;
}
