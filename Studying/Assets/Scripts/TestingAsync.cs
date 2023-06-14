using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TestingAsync : MonoBehaviour
{
    CancellationTokenSource cts;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            Async();
        if (Input.GetKeyDown(KeyCode.J))
            StopAllCoroutines();
        if (Input.GetKeyDown(KeyCode.K))
            cts.Cancel();
        if (Input.GetKeyDown(KeyCode.N))
            StartCoroutine(Coroutine());
    }
    async void Async()
    {
        cts = new CancellationTokenSource();
        print("Async started");
        try
        {
            await Task.Delay(10000, cts.Token); // keeps waiting, even if your out play mode
        }
        catch
        {
            print("Async cancelled");
            return;
        }
        finally
        {
            cts.Dispose();
            cts = null;
        }
        print("Async ended");
    }
    IEnumerator Coroutine()
    {
        print("Coroutine started");
        int counter = 0;
        while (counter < 100)
        {
            print(counter);
            yield return new WaitForSeconds(1);
            counter++;
        }
    }
}
