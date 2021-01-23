using System;
using Xunit;
using CI.DE;
namespace CI.DifferentialEvolution.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void ShouldRun()
        {
            var engine = new DifferentialEvolution(2,10,new double[] { 10,10}, new double[] { -10, -10 }, (double[] d) => { return -Math.Abs((d[0] + d[1]) - 5); }, (double f) => { return f == 0; });
            engine.RunUntilTargetReached();
            
            Assert.True(engine.BestFitness == 0);
            
        }
    }
}
