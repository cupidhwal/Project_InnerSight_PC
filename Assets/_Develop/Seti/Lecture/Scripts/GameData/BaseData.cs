using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Seti
{
    /// <summary>
    /// Data 기본 클래스
    /// 공통적인 데이터 : 이름 목록
    /// 공통적인 기능 : 데이터의 갯수 가져오기, 이름 목록 리스트 얻어오기, 데이터 추가,복사,제거
    /// </summary>
    public class BaseData : ScriptableObject
    {
        #region Variables
        //public string[] names;    
        public List<string> names;
        public const string DataDirectory = "/ResourcesData/Resources/Data/";
        #endregion

        //생성자
        public BaseData() { }

        //데이터의 갯수 가져오기
        public int GetDataCount()
        {
            if (names == null)
            {
                return 0;
            }

            return names.Count;
        }

        //툴에 출력하기 위해 이름 목록 리스트 얻어오기
        public string[] GetNameList(bool showID, string filterWord = "")
        {
            int length = GetDataCount();

            string[] retList = new string[length];

            for (int i = 0; i < length; i++)
            {
                if(filterWord != "")
                {
                    if(names[i].ToLower().Contains(filterWord.ToLower()))
                    {
                        continue;
                    }
                }

                if(showID)
                {
                    retList[i] = i.ToString() + " : " + names[i];
                }
                else
                {
                    retList[i] = names[i];
                }
            }

            return retList;
        }

        //데이터 추가하고 최종 갯수 리턴
        public virtual int AddData(string newName)
        {

            return GetDataCount();
        }

        //데이터 복사
        public virtual void Copy(int index)
        {

        }

        //데이터 제거
        public virtual void RemoveData(int index)
        {

        }
    }
}
