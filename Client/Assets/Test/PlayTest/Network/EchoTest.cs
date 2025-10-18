using System.Collections;
using Network;
using NUnit.Framework;
using UnityEngine.TestTools;

public class EchoTest
{
    [SetUp]
    public void Setup() {
        GameMgr.Instance.Start();
    }
    
    [UnityTest]
    public IEnumerator EchoTestWithEnumeratorPasses() {
        int x = 1;
        while (true) {
            Framework.Network.Network.Instance.Send(MessageDef.echo_test_c2s, new echo_test_c2s() {
                Id = x,
                Name = "Test" + x,
            });
            x++;
            
            GameMgr.Instance.Update();
            
            yield return null;
        }
    }
}
