using System;
using System.Collections.Generic;

public class FogTest : SceneTest {
    protected override HashSet<Type> TestSystem => new HashSet<Type> { typeof(Navmesh.INavmesh), typeof(Combat.Fog.IFogSystem) };
    protected override string TestSceneName => "FogTest";
}
