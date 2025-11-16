using System;
using NUnit.Framework;

public class FixedTest {
    [Test]
    public void FixedTest1() {
        float a = 1.2f, b = 3.4f;
        Assert.IsTrue(FloatF.Abs(FloatF.FromFloat(a) + FloatF.FromFloat(b) - FloatF.FromFloat(a + b)) < FloatF.eps);
    }
    
    [Test]
    public void FixedTest2() {
        float a = 123456.123456f;
        Assert.IsTrue(FloatF.Abs(FloatF.Sqrt(FloatF.FromFloat(a)) - FloatF.FromFloat(Math.Sqrt(a))) < FloatF.eps);
    }
}