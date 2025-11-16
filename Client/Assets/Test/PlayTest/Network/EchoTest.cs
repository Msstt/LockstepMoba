using System.Collections;
using Network;
using NUnit.Framework;
using UnityEngine.TestTools;

public class EchoTest {
    [SetUp]
    public void Setup() {
        NetworkUtils.Start();
    }
    
    [UnityTest]
    public IEnumerator EchoTest1() {
        int x = 1;
        while (true) {
            NetworkUtils.Send(MessageDef.echo_test_c2s, new echo_test_c2s() {
                Id = x,
                Name = "Test" + x,
            });
            x++;
            
            // GameMgr.Instance.Update();
            
            yield return null;
        }
    }
}
