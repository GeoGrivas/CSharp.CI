using System;
using Xunit;

namespace CI.PSO.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void ShouldRun()
        {
            var pso = new ParticleSwarmOptimization(2, 10, 10, -10, (double[] d) => { return Math.Abs((d[0] + d[1]) - 5); }, (double f) => { return f == 0; }, FitnessMethod.Minimize);
            pso.RunUntilTargetReached();
            Assert.True(pso.BestSwarmFitness == 0);
        }


        [Fact]
        public void ShouldRun10Times()
        {
            int i = 0;
            var pso = new ParticleSwarmOptimization(2, 10, 50, -50, (double[] d) => { return Math.Abs((d[0] + d[1]) - 5); }, (double f) =>
            {
                i++;
                return i == 10;
            }, FitnessMethod.Minimize);
            pso.RunUntilTargetReached();
            Assert.True(i == 10);
        }
        [Fact]
        public void ShouldMaximizeFunction()
        {
            int i = 0;
            var pso = new ParticleSwarmOptimization(2, 10, 50, -50, (double[] d) => { return Math.Abs((d[0] + d[1]) - 5); }, (double f) =>
            {
                i++;
                return i == 10;
            }, FitnessMethod.Maximize);
            double startBestFitness = pso.BestSwarmFitness;
            pso.RunUntilTargetReached();
            Assert.True(pso.BestSwarmFitness > startBestFitness);
        }
        [Fact]
        public void ShouldThrowArgumentExceptionWhenDimensionsLowerThan1()
        {
            int i = 0;
            Assert.Throws<ArgumentException>(() =>
            {
                new ParticleSwarmOptimization(0, 10, 50, -50, (double[] d) => { return Math.Abs((d[0] + d[1]) - 5); }, (double f) => { i++; return i == 10; }, FitnessMethod.Maximize);
            });
        }
        [Fact]
        public void ShouldThrowArgumentExceptionWhenParticlesLowerThan1()
        {
            int i = 0;
            Assert.Throws<ArgumentException>(() =>
            {
                new ParticleSwarmOptimization(1, 0, 50, -50, (double[] d) => { return Math.Abs((d[0] + d[1]) - 5); }, (double f) => { i++; return i == 10; }, FitnessMethod.Maximize);
            });
        }
    }
}
