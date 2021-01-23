using System;
using Xunit;

namespace CI.DE.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void ShouldRun()
        {
            var engine = new DifferentialEvolution(2, 10, new double[] { 10, 10 }, new double[] { -10, -10 }, (double[] d) => { return -Math.Abs((d[0] + d[1]) - 5); }, (double f) => { return f == 0; });
            engine.RunUntilTargetReached();

            Assert.True(engine.BestFitness == 0);

        }

        [Fact]
        public void ShouldThrowArgumentExceptionWhenDimensionsLowerThan1()
        {
           
            Assert.Throws<ArgumentException>(() =>
            {
                new DifferentialEvolution(0, 10, new double[] { 10, 10 }, new double[] { -10, -10 }, (double[] d) => { return -Math.Abs((d[0] + d[1]) - 5); }, (double f) => { return f == 0; });
            });
        }
        [Fact]
        public void ShouldThrowArgumentExceptionWhenParticlesLowerThan1()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new DifferentialEvolution(2, 3, new double[] { 10, 10 }, new double[] { -10, -10 }, (double[] d) => { return -Math.Abs((d[0] + d[1]) - 5); }, (double f) => { return f == 0; });
            });
        }

        [Fact]
        public void ShouldThrowArgumentExceptionWhenDifferentialWeightIsNotBetweenZeroAndTwo()
        {

            Assert.Throws<ArgumentException>(() =>
            {
                new DifferentialEvolution(0, 10, new double[] { 10, 10 }, new double[] { -10, -10 }, (double[] d) => { return -Math.Abs((d[0] + d[1]) - 5); }, (double f) => { return f == 0; },0.8,-1);
            });
            Assert.Throws<ArgumentException>(() =>
            {
                new DifferentialEvolution(0, 10, new double[] { 10, 10 }, new double[] { -10, -10 }, (double[] d) => { return -Math.Abs((d[0] + d[1]) - 5); }, (double f) => { return f == 0; }, 0.8, 2.1);
            });
        }
        [Fact]
        public void ShouldThrowArgumentExceptionWhenCrossoverProbabilityIsNotBetweenZeroAndOne() { 
            Assert.Throws<ArgumentException>(() =>
            {
                new DifferentialEvolution(2, 3, new double[] { 10, 10 }, new double[] { -10, -10 }, (double[] d) => { return -Math.Abs((d[0] + d[1]) - 5); }, (double f) => { return f == 0; },-1);
            });
            Assert.Throws<ArgumentException>(() =>
            {
                new DifferentialEvolution(2, 3, new double[] { 10, 10 }, new double[] { -10, -10 }, (double[] d) => { return -Math.Abs((d[0] + d[1]) - 5); }, (double f) => { return f == 0; }, 1.1);
            });
        }
    }
}
