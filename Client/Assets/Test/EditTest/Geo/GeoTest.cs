using System;
using NUnit.Framework;

public class GeoTest {
    [Test]
    public void LineIntersectTest1() {
        Vector3F a = new Vector3F(1, 0, 0);
        Vector3F b = new Vector3F(-1, 0, 0);
        Vector3F c = new Vector3F(0, 0, 1);
        Vector3F d = new Vector3F(0, 0, -1);
        Assert.IsTrue(GeoUtils.LineIntersect(a, b, c, d, out Vector3F intersection));
        Assert.AreEqual(intersection.x, 0);
        Assert.AreEqual(intersection.z, 0);
    }
}