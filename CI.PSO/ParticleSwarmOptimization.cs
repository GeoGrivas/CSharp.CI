using System;
using System.Collections.Generic;

namespace CI.PSO
{
    public class ParticleSwarmOptimization
    {
        public List<Particle> Particles { get; set; } = new List<Particle>();

        private readonly double velocityFactor = 0.3;
        public double[] BestSwarmPosition { get; set; } = null;
        public double BestSwarmFitness { get; set; }

        private readonly double omega;
        private readonly double fp;
        private readonly double fg;
        private readonly Random random = new Random();
        private readonly Func<double[], double> CalculateFitness;
        private readonly Func<double, bool> TargetReached;
        private readonly double[] upperBounds;
        private readonly double[] lowerBounds;
        public FitnessMethod FitnessMethod { get; set; }
        public ParticleSwarmOptimization(int dimensions, int numberOfParticles, double upperBound, double lowerBound, Func<double[], double> CalculateFitness, Func<double, bool> TargetReached, FitnessMethod FitnessMethod, double velocityFactor = 0.5, double omega = 0.3, double fp = 1, double fg = 0.3)
        {
            if (dimensions < 1)
            {
                throw new ArgumentException("Dimensions should be > 1");
            }
            if (numberOfParticles < 1)
            {
                throw new ArgumentException("Number of particles should be > 1");
            }
            for (int i = 0; i < numberOfParticles; i++)
            {
                Particles.Add(new Particle(dimensions));
            }
            this.velocityFactor = velocityFactor;
            this.omega = omega;
            this.fp = fp;
            this.fg = fg;
            this.CalculateFitness = CalculateFitness;
            this.FitnessMethod = FitnessMethod;
            this.TargetReached = TargetReached;
            lowerBounds = new double[dimensions];
            upperBounds = new double[dimensions];
            for(int i=0;i<dimensions;i++)
            {
                lowerBounds[i] = lowerBound;
                upperBounds[i] = upperBound;
            }
            Initialize(upperBounds, lowerBounds);
        }
        public ParticleSwarmOptimization(int dimensions, int numberOfParticles, double[] upperBounds, double[] lowerBounds, Func<double[], double> CalculateFitness, Func<double, bool> TargetReached, FitnessMethod FitnessMethod, double velocityFactor = 0.5, double omega = 0.3, double fp = 1, double fg = 0.3)
        {
            if (dimensions < 1)
            {
                throw new ArgumentException("Dimensions should be > 1");
            }
            if (numberOfParticles < 1)
            {
                throw new ArgumentException("Number of particles should be > 1");
            }
            for (int i = 0; i < numberOfParticles; i++)
            {
                Particles.Add(new Particle(dimensions));
            }
            
            this.velocityFactor = velocityFactor;
            this.omega = omega;
            this.fp = fp;
            this.fg = fg;
            this.CalculateFitness = CalculateFitness;
            this.FitnessMethod = FitnessMethod;
            this.TargetReached = TargetReached;
            this.lowerBounds = lowerBounds;
            this.upperBounds = upperBounds;
            Initialize(upperBounds, lowerBounds);
        }
        public void RunUntilTargetReached()
        {
            while (!TargetReached(BestSwarmFitness))
            {
                RunOnce();
            }
        }

        public void RunOnceWithFitnessCaching()
        {
            foreach (var particle in Particles)
            {
                CalculateParticlesNewPosition(particle);
                UpdateParticlesFitnessWithCaching(particle);
            }
        }
        private void CalculateParticlesNewPosition(Particle particle)
        {
            var currentPosition = particle.CurrentPosition;
            var bestParticlePosition = particle.BestPosition;
            for (int i = 0; i < currentPosition.Length; i++)
            {
                var particleDimensionVelocity = particle.Velocities[i];
                double selfPart = fp * random.NextDouble() * (bestParticlePosition[i] - currentPosition[i]);
                double bestInSwarmPart = fg * random.NextDouble() * (BestSwarmPosition[i] - bestParticlePosition[i]);
                particleDimensionVelocity = omega * particleDimensionVelocity + selfPart + bestInSwarmPart;
                currentPosition[i] = currentPosition[i] + (velocityFactor * particleDimensionVelocity);
                particle.Velocities[i] = particleDimensionVelocity;
            }
        }

        public void RunOnce()
        {
            foreach (var particle in Particles)
            {

                CalculateParticlesNewPosition(particle);
                UpdateParticlesFitnessWithoutCaching(particle);
            }
        }

        private void UpdateParticlesFitnessWithoutCaching(Particle particle)
        {
            particle.Fitness = CalculateFitness(particle.CurrentPosition);
            if (FitnessMethod.Maximize == FitnessMethod)
            {
                if (particle.Fitness > CalculateFitness(particle.BestPosition))
                {
                    particle.BestPosition = (double[])particle.CurrentPosition.Clone();
                    if (particle.Fitness > CalculateFitness(BestSwarmPosition))
                    {
                        BestSwarmPosition = (double[])particle.CurrentPosition.Clone();
                        BestSwarmFitness = particle.Fitness;
                    }
                }
            }
            else
            {
                if (particle.Fitness < CalculateFitness(particle.BestPosition))
                {
                    particle.BestPosition = (double[])particle.CurrentPosition.Clone();
                    if (particle.Fitness < CalculateFitness(BestSwarmPosition))
                    {
                        BestSwarmPosition = (double[])particle.CurrentPosition.Clone();
                        BestSwarmFitness = particle.Fitness;
                    }
                }
            }
        }
        private void UpdateParticlesFitnessWithCaching(Particle particle)
        {
            particle.Fitness = CalculateFitness(particle.CurrentPosition);
            if (particle.ShouldUpdateParticleBestFitness(FitnessMethod))
            {
                particle.BestPosition = (double[])particle.CurrentPosition.Clone();
                particle.BestFitness = particle.Fitness;
                UpdateBestFitWithCache(particle);
            }
        }

        private void Initialize(double[] upperBounds, double[] lowerBounds)
        {
            foreach (var particle in Particles)
            {

                particle.InitializeParticlePosition(upperBounds, lowerBounds, random);
                particle.Fitness = CalculateFitness(particle.CurrentPosition);
                particle.BestPosition = (double[])particle.CurrentPosition.Clone();
                particle.BestFitness = particle.Fitness;
                if (BestSwarmPosition == null)
                {
                    BestSwarmPosition = particle.CurrentPosition;
                    BestSwarmFitness = particle.Fitness;
                }
                UpdateParticlesFitnessWithCaching(particle);
            }
        }
        private void UpdateBestFitWithCache(Particle candidateParticle)
        {

            if (FitnessMethod == FitnessMethod.Maximize)
            {
                if (candidateParticle.Fitness > BestSwarmFitness)
                {
                    BestSwarmPosition = (double[])candidateParticle.CurrentPosition.Clone();
                    BestSwarmFitness = candidateParticle.Fitness;
                }
            }
            else
            {
                if (candidateParticle.Fitness < BestSwarmFitness)
                {
                    BestSwarmPosition = (double[])candidateParticle.CurrentPosition.Clone();
                    BestSwarmFitness = candidateParticle.Fitness;
                }
            }


        }

    }
}
