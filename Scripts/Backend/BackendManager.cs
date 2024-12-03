using UnityEngine;
using BackEnd;		// �ڳ� SDK

public class BackendManager : MonoBehaviour
{
	private void Awake()
	{
		

		// �ڳ� ���� �ʱ�ȭ
		BackendSetup();
	}


	private void BackendSetup()
	{
		// �ڳ� �ʱ�ȭ (�ݹ� �Լ� Ǯ���� ����Ϸ��� �Ű������� true�� ����)
		var bro = Backend.Initialize();

		// �ڳ� �ʱ�ȭ�� ���� ���䰪
		if ( bro.IsSuccess() )
		{
			// �ʱ�ȭ ���� �� statusCode 204 Success
			Debug.Log($"�ʱ�ȭ ���� : {bro}");
		}
		else
		{
			// �ʱ�ȭ ���� �� statusCode 400�� ���� �߻�
			Debug.LogError($"�ʱ�ȭ ���� : {bro}");
		}
	}
}

