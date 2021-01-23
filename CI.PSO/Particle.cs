using CI.Shared;
using System;

namespace CI.PSO
{
    public class Particle
    {
        public double[] BestPosition { get; set; }
        public double BestFitness;
        public double[] CurrentPosition { get; set; }
        public double Fitness { get; set; }
        public double[] Velocities = null;

        public Particle(int dimensions)
        {
            CurrentPosition = new double[dimensions];
            Velocities = new double[dimensions];
        }

        public void InitializeParticlePosition(double[] upperBounds, double[] lowerBounds, Random random)
        {
            for (int i = 0; i < CurrentPosition.Length; i++)
            {
                CurrentPosition[i] = random.NextDouble(lowerBounds[i], upperBounds[i]);
                InitilizeParticleVelocity(i, upperBounds[i], lowerBounds[i], random);
            }
        }
        private void InitilizeParticleVelocity(int dimension, double upperBound, double lowerBound, Random random)
        {
            Velocities[dimension] = random.NextDouble(-Math.Abs(upperBound - lowerBound), Math.Abs(upperBound - lowerBound));
        }
        public void ResetParticleVelocities(double[] upperBounds, double[] lowerBounds, Random random)
        {
            for (int i = 0; i < Velocities.Length; i++)
            {
                InitilizeParticleVelocity(i, upperBounds[i], lowerBounds[i], random);
            }
        }
        public bool ShouldUpdateParticleBestFitness(FitnessMethod fitnessMethod)
        {
            if (fitnessMethod == FitnessMethod.Maximize)
                return Fitness > BestFitness;
            else
                return Fitness < BestFitness;
        }
    }
}
