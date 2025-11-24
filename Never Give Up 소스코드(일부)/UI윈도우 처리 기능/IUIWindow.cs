using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI 윈도우가 공통적으로 구현해야 하는 인터페이스입니다.
//창의 초기화, 열기/닫기, 의존성 주입 기능을 정의합니다.
public interface IUIWindow
{
    //외부에서 필요한 의존성을 주입합니다.
    void InjectDependencies(object[] _dependencies);

    //초기 데이터를 받아 윈도우를 초기화합니다.
    void Initial(object[] _datas);

    //윈도우를 엽니다.
    void Open();

    //윈도우를 닫습니다.
    void Close();
}
