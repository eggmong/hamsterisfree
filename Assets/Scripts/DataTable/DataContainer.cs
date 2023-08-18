using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;
using DataTable;


public class DataContainer : MonoSingleton<DataContainer>
{
    [SerializeField]
    private Table_Stage stageTable;
    public Table_Stage StageTable => this.stageTable;

    [SerializeField]
    private Table_Seed seedTable;
    public Table_Seed SeedTable => this.seedTable;


    private List<Sprite> stageSprites;
    public List<Sprite> StageSprites => this.stageSprites;

    private int tileSpritesCount = 0;




    public void Initialize()
    {
        this.tileSpritesCount = Enum.GetValues(typeof(Define.TileSpriteName)).Length;
        this.stageSprites = new List<Sprite>(this.tileSpritesCount);
    }


    

    // TODO
    // Json 으로 진행상황 저장하는 함수 만들기
    // 진행해야하는 스테이지 넘버 (첫 시작이면 0이란 소리)
    // 탈출문 좌표
    // 스테이지 선택해서 게임 시작할 때 여기 저장된 스테이지 넘버로 스테이지 데이터테이블 참조하여 불러옴



    public async UniTask LoadStageDatas()
    {
        Debug.Log("LoadStageDatas 시작");

        var curIndex = CommonManager.Instance.CurStageIndex;

        try
        {
            var item = stageTable.list.Where(x => x.Index == curIndex).FirstOrDefault();

            if (item != null)
                await LoadStageSprites(item.MapName);
            else
                Debug.Log($"### Error ---> {curIndex} is Not ContainsKey ###");
        }
        catch (Exception ex) when (!(ex is OperationCanceledException))
        {
            Debug.Log("### LoadTileSprites Failed: " + ex.Message + " ###");
        }

        Debug.Log("LoadStageDatas 끝!");
    }

    private async UniTask LoadStageSprites(string _MapName)
    {
        var rootPath = "Images/Map";

        var path = $"{rootPath}/{_MapName}/{_MapName}_";

        Define.TileSpriteName spriteName = Define.TileSpriteName.Center;

        try
        {
            for (spriteName = 0; (int)spriteName < this.tileSpritesCount; spriteName++)
            {
                var spritePath = $"{path}{spriteName}";

                var resource = await Resources.LoadAsync<Sprite>(spritePath);
                var sprite = resource as Sprite;

                if (sprite != null)
                    this.stageSprites.Add(sprite);
                else
                    Debug.Log("### Fail <Sprite> Type Casting ###");
            }
        }
        catch (Exception ex) when (!(ex is OperationCanceledException))
        {
            Debug.Log("### LoadTileSprites Failed: " + ex.Message + " ###");
        }
    }


}
