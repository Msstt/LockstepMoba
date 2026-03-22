using System;
using NUnit.Framework;

public class FixedTest {
    [Test]
    public void Test1() {
        float a = 1.2f, b = 3.4f;
        Assert.IsTrue(FloatF.Abs(a.ToFloatF() + b.ToFloatF() - (a + b).ToFloatF()) < FloatF.eps);
    }
    
    [Test]
    public void Test2() {
        float a = 123456.123456f;
        Assert.IsTrue(FloatF.Abs(FloatF.Sqrt(a.ToFloatF()) - Math.Sqrt(a).ToFloatF()) < FloatF.eps);
    }
    
    [Test]
    public void Test3() {
        float a = 123456.123456f;
        Assert.IsTrue(FloatF.Abs(FloatF.Sin(a.ToFloatF()) - Math.Sin(a).ToFloatF()) < 0.03f.ToFloatF());
        a = 123456.123456f + (float)Math.PI / 2;
        Assert.IsTrue(FloatF.Abs(FloatF.Sin(a.ToFloatF()) - Math.Sin(a).ToFloatF()) < 0.03f.ToFloatF());
        a = 123456.123456f + (float)Math.PI;
        Assert.IsTrue(FloatF.Abs(FloatF.Sin(a.ToFloatF()) - Math.Sin(a).ToFloatF()) < 0.03f.ToFloatF());
        a = 123456.123456f + (float)Math.PI / 2 * 3;
        Assert.IsTrue(FloatF.Abs(FloatF.Sin(a.ToFloatF()) - Math.Sin(a).ToFloatF()) < 0.03f.ToFloatF());
    }
}