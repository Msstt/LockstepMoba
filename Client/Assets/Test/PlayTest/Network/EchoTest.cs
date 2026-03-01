using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Network;
using Network;
using NUnit.Framework;
using UnityEngine.TestTools;

public class EchoTest {
    
    [SetUp]
    public void Setup() {
        GameMgr.Instance.RegisterSystem(new HashSet<Type> { typeof(INetwork) });
        GameMgr.Instance.Init();
    }
    
    [UnityTest]
    public IEnumerator Test1() {
        int x = 1;
        while (true) {
            NetworkUtils.Send(MessageDef.echo_test_c2s, new echo_test_c2s() {
                Id = x,
                Name = "Test" + x,
            });
            x++;
            
            GameMgr.Instance.Update();
            yield return null;
        }
    }
}
