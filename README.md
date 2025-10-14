# Optimizing the Firefly Algorithm for Controlling a Seven Degree of Freedom Robotic Arm

The purpose of this project was to explore potential optimizations to the Firefly Algorithm (FA) described in the paper, Calculation of the inverse kinematics solution of the 7-DOF redundant robot manipulator by the firefly algorithm and statistical analysis of the results in terms of speed and accuracy, by Serkan Dereli and Raşit Köker.

The algorithm described in the paper was reproduced as closely as practicable using a custom-built FA Inverse Kinematics (IK) solver. The same 7-DOF SUNGUR 370 robot Denavit Hartenberg (DH) parameters used in the paper (found in Table 1 of the paper) were used to build a Forward Kinematics (FK) model, which was used in the IK solver. The baseline algorithm was reproduced, using the1000 iteration, 30-firefly swarm approach described in the paper.

The tool BenchmarkDotNet, was used to establish benchmarks for the baseline algorithm. 100 randomly chosen end-effector positions within the robot’s reachable space were calculated, and the result was divided by 100 to reach an average solution time. Position Error was determined by finding the lowest error value for which unit tests could be shown to reliably pass. Based on these measurement approaches, the solver built for this project produced results similar to those found by Dereli and Köker. The improvement in average solution time over that found by Dereli and Köker is suspected to be due to differences in computer hardware and/or programming language used for simulation.

                                Average time (s)	      Position Error (mm)
    Dereli and Köker’s results  0.1825	                  0.0877
    This project’s results	    0.14125	                  < 0.05

Potential optimization techniques were explored:
- A.	Limiting iterations to those needed to satisfy a position error of less than 1 mm.
- B.	Using caching to narrow the range of joint angle values used for the initial swarm in the algorithm.
- C.	Using a combination of optimizations A and B.
- D.	Using a combination of optimizations A and B, but additionally limiting the swarm size to 1/3 the default size whenever a cached value was available for limiting joint angle values.

For optimization A, the baseline algorithm was modified to stop iterating as soon as any solution that produced a position error of less than 1 mm was found. Initially, the modified algorithm was allowed to run until an acceptable solution was found, but it was identified that some randomly chosen target positions were outside the reach of the robot, resulting in an infinite loop. To prevent this problem from occurring in future runs, the algorithm for producing random target values was modified to ensure that all target values were within the arm’s reach and, additionally, code was added to limit the maximum number of iterations the algorithm could use.

For optimization B, the baseline algorithm was modified to utilize caching. The robot’s reachable space was divided into cubical sections, each with a side dimension of 1/8 the diameter of the reachable space sphere. Each time a swarm was processed, the caching algorithm checked each firefly to see if it could be used to populate one of the caching sections with a known firefly position. Then, each time a subsequent target position needed to be solved, the cache was checked to determine if there was a cached firefly in the same caching section that contained the target position. If so, the initial swarm for solving the target position was limited by reducing the range of joint angles used for the fireflies in the swarm. An assumption was made that for each robot joint, the angle ranges used for the initial randomly selected values could be limited to the cached angle, plus or minus 1/8 of the joint’s full range of motion, while still being able to achieve any end-effector position in the caching section.

For optimization C, optimizations A and B were combined.

For optimization D, optimizations A and B were combined with a swarm size reducing algorithm. Since the processing and time needed for the IK solver to find a solution for an end-effector position is primarily due to the number of FK calculations that need to be made, and since an FK calculation was needed for each firefly, once per iteration, limiting the swarm size to 1/3 the baseline size had the potential to reduce the total number of calculations needed.

The same benchmarking approach and 100 target end-effector positions were used for determining the average solution time for each of the optimization techniques. Since most of the optimized algorithms intentionally sacrificed position error for speed, position error was not compared for the optimized solutions.

                                                  Average solution time (s)
      Dereli and Köker’s results	              0.1825
      This project baseline	                      0.14125
      Optimization A (position tolerance 1 mm)    0.05049
      Optimization B (caching)	                  0.14039
      Optimization C (A + B)	                  0.03783
      Optimization D (A + B) reduced swarm size   0.04260

Optimization A showed a significant improvement over the baseline algorithm. Since the extreme precision achievable with the baseline algorithm isn’t needed for many applications, the number of iterations, and therefor the number of FK calculations made, could be drastically reduced by stopping iterations as soon as a solution was found that met the required < 1mm error.

Optimization B showed approximately the same performance as the baseline algorithm with the margin of error calculated by the BenchmarkDotNet tool considered. This makes sense because the total number of FK calculations needed for the optimized algorithm is the same as the number needed for the baseline algorithm.

Optimization C – Limiting iterations to those needed to satisfy a position error of less than 1 mm, combined with caching to narrow the range of joint angle values used for the initial swarm in the algorithm produced the best results, with a 69.7% reduction in average solution time over the project baseline.

Optimization D – Limiting the swarm size when a cached value was available did not improve the performance of the algorithm. Although the smaller swarm size resulted in fewer FK calculations per iteration, more iterations were needed to find a solution that met the required < 1 mm error.

The original intent of the project was to simulate a robotic solution to the 8-queens problem, and the repository contains the code needed to simulate the solution. Due to the long time needed to simulate the thousands of individual robotic arm moves needed to solve the problem, the 8-queens problem solution was not used for comparing algorithms.
