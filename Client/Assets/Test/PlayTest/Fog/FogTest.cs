using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class FogTest : SceneTest {
    protected override HashSet<Type> TestSystem => new HashSet<Type> { typeof(Navmesh.INavmesh), typeof(Combat.Fog.IFogSystem) };
    protected override string TestSceneName => "FogTest";
}
