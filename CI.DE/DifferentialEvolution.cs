using CI.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CI.DE
{
    public class DifferentialEvolution
    {
        int dimensions;
        public double CrossoverProbablity { get; set; }
        public double DifferentialWeight { get; set; }
        public List<Agent> Agents { get; set; } = new List<Agent>();
        public double? BestFitness { get; set; }
        Random random = new Random();
        Func<double[], double> FitnessCalculator;
        Func<double, bool> TargetReached;

        public DifferentialEvolution(int dimensions, int numberOfAgents, double[] upperBounds, double[] lowerBounds, Func<double[], double> FitnessCalculator, Func<double, bool> TargetReached, double CrossoverProbablity = 0.9,double DifferentialWeight=0.8)
        {
            if(dimensions<1)
            {
                throw new ArgumentException("You should at least have 1 dimension");
            }
            if(numberOfAgents<4)
            {
                throw new ArgumentException("Number of agents should be at least 4.");
                throw new ArgumentException("Number of agents should be at least 4.");
            }
            if(DifferentialWeight>2 || DifferentialWeight<0)
            {
                throw new ArgumentException("Differential weight should be between 0 and 2");
            }

            if (CrossoverProbablity > 1 || CrossoverProbablity < 0)
            {
                throw new ArgumentException("Crossover Probability should be between 0 and 1");
            }
            this.dimensions = dimensions;
            for (int i = 0; i < numberOfAgents; i++)
            {
                Agents.Add(new Agent(dimensions, random, upperBounds, lowerBounds));
            }
            this.FitnessCalculator = FitnessCalculator;
            this.TargetReached = TargetReached;
            this.DifferentialWeight = DifferentialWeight;
            this.CrossoverProbablity = CrossoverProbablity;
        }

        public void RunOnce()
        {
            var currentAgentIndex = 0;
            foreach (var agent in Agents)
            {
                var randoms = GetThreeRandomAgentIndexes(Agents.Count() - 1, currentAgentIndex);
                var randomAgent1 = Agents[randoms[0]];
                var randomAgent2 = Agents[randoms[1]];
                var randomAgent3 = Agents[randoms[2]];

                var randomDimension = random.Next(0, dimensions - 1);

                double[] potentialNewPosition = new double[agent.Position.Length];
                for (int d = 0; d < dimensions; d++)
                {
                    if (CrossoverProbablity > random.NextDouble() || randomDimension == d)
                    {
                        potentialNewPosition[d] = randomAgent1.Position[d] + DifferentialWeight * (randomAgent2.Position[d] - randomAgent3.Position[d]);
                    }
                    else
                    {
                        potentialNewPosition[d] = agent.Position[d];
                    }
                }

                double newPositionFitness = FitnessCalculator(potentialNewPosition);
                if (newPositionFitness > FitnessCalculator(agent.Position))
                {
                    agent.Position = potentialNewPosition;
                    if (!BestFitness.HasValue ||newPositionFitness > BestFitness)
                    {
                        BestFitness = newPositionFitness;
                    }
                }
                currentAgentIndex++;
            }
        }
        private List<int> GetThreeRandomAgentIndexes(int max,int exclusion)
        {
            List<int> randoms = new List<int>();
            AddUniqueRandomInList(randoms, max, exclusion);
            AddUniqueRandomInList(randoms, max, exclusion);
            AddUniqueRandomInList(randoms, max, exclusion);
            return randoms;
        }
        public void RunUntilTargetReached()
        {
            do
            {
                RunOnce();
            } while (!TargetReached(BestFitness.Value));
        }
        private void AddUniqueRandomInList(List<int> randomsList, int upperLimit, int exclusion)
        {
            int randomNumber = random.Next(0, upperLimit);
            while (randomsList.Contains(randomNumber) || exclusion == randomNumber)
            {
                randomNumber = random.Next(0, upperLimit);
            }
            randomsList.Add(randomNumber);
        }

    }

    public class Agent
    {
        public double[] Position { get; set; }

        public Agent(int dimensions, Random random, double[] upperBounds, double[] lowerBounds)
        {
            Position = new double[dimensions];
            InitilizeAgent(random, upperBounds, lowerBounds);
        }

        private void InitilizeAgent(Random random, double[] upperBounds, double[] lowerBounds)
        {
            for (int i = 0; i < Position.Length; i++)
            {
                Position[i] = random.NextDouble(lowerBounds[i], upperBounds[i]);
            }
        }

    }
}
