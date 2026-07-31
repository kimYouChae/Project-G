using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : Singleton<AddressableManager>
{
    protected override void Singleton_Awake()
    {

    }

    // 리스트로 반환되는 에셋을 가져옴 
    public async Task<IList<T>> GetAddressableAssets<T>(AddressableLabelType label) 
    {
        AsyncOperationHandle<IList<T>> handler
            = Addressables.LoadAssetsAsync<T>(label.ToString(), null);

        await handler.Task;

        if (handler.Status == AsyncOperationStatus.Succeeded) 
        {
            Debug.Log($"[AddressableManager] {label}타입에 해당하는 에셋들 가져오기 성공");
            return handler.Result;
        }
        else
        {
            Debug.Log($"[AddressableManager] {label}타입에 해당하는 에셋들 가져오기 실패");
            return null;
        }
    }

    // 단일객체로 반환되는 에셋을 가져옴
    public async Task<T> GetAddressableAsset<T>(AddressableLabelType label) 
    {
        AsyncOperationHandle<T> handler
                = Addressables.LoadAssetAsync<T>(label.ToString());

        await handler.Task;

        if (handler.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"[AddressableManager] {label}타입에 해당하는 에셋 가져오기 성공");
            return handler.Result;
        }
        else
        {
            Debug.Log($"[AddressableManager] {label}타입에 해당하는 에셋 가져오기 실패");
            return default;
        }
    }

}
