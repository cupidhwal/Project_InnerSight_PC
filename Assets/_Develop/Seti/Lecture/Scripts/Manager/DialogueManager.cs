using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 게임에서 사용하는 데이터들을 관리하는 클래스
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        private static DialogData dialogData = null;

        private void Start()
        {
            //이펙트 데이터 가져오기
            if (dialogData == null)
            {
                dialogData = ScriptableObject.CreateInstance<DialogData>();
                dialogData.LoadData();

                Debug.Log($"{dialogData} 로드 완료");
            }
        }

        //이펙트 데이터 가져오기
        public static DialogData GetDialogData()
        {
            if (dialogData == null)
            {
                dialogData = ScriptableObject.CreateInstance<DialogData>();
                dialogData.LoadData();
            }
            return dialogData;
        }
    }
}