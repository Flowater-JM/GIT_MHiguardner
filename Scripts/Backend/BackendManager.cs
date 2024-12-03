using UnityEngine;
using BackEnd;		// 뒤끝 SDK

public class BackendManager : MonoBehaviour
{
	private void Awake()
	{
		

		// 뒤끝 서버 초기화
		BackendSetup();
	}


	private void BackendSetup()
	{
		// 뒤끝 초기화 (콜백 함수 풀링을 사용하려면 매개변수를 true로 설정)
		var bro = Backend.Initialize();

		// 뒤끝 초기화에 대한 응답값
		if ( bro.IsSuccess() )
		{
			// 초기화 성공 시 statusCode 204 Success
			Debug.Log($"초기화 성공 : {bro}");
		}
		else
		{
			// 초기화 실패 시 statusCode 400대 에러 발생
			Debug.LogError($"초기화 실패 : {bro}");
		}
	}
}

