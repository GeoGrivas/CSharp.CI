# CSharp.CI
A collection of Computational Intelligence(CI) algorithms written in C#.


Here you will find basic implementations of CI algorithms that are useful in function optimizing.

Algorithms that are currently implemented:
<ul>
<li>Particle swarm optimization</li>
<li>Differential evolution</li>
</ul>


How to use
```
using CI.PSO;
...
var pso= new ParticleSwarmOptimization(numberOfDimensions, numberOfParticles, upperBound, lowerBound, FitnessCalculatorFunc, TargetReachedFunc);
pso.RunUntilTargetReached();
```
numberOfDimensions is an int that specified the number of dimensions the problem has.
numberOfParticles is an int that specifies the number of particles that will exist throught the run.
upperBound and lowerBound are the doubles or double arrays (a value for each dimension) that specify the maximum and minimum values of our search space.
FitnessCalculatorFunc is a function that takes an array of doubles(a possible solution) and returns a double (which is the fitness).
TargetReachedFunc is a function that takes in a double (the fitness) and returns a boolean.

The same story applies to Differential evolution also,
```
using CI.DE;
...
var de= new DifferentialEvolution(numberOfDimensions, numberOfAgents, upperBound, lowerBound, FitnessCalculatorFunc, TargetReachedFunc);
de.RunUntilTargetReached();
```


You can also check a playground [here](https://blazor.adventurouscoding.com/ci/mousefollowing)
