# PayslipGenerator

This is a C# solution developed using VS2015. The solution consists of several assemblies with various and autonomous functions.

How to run
==========
The solution contains a console application called "PayslipGenerator". The console application reads input from a CSV file called "Data1.txt"
which should be available under {current executing directory}\\TestData folder. One such file is part of the solution already therefore solution
can be tested by pressing F5 which will execute the console app from within VS.

Assumptions
===========
The solution has been developed with assumption that all salary data will lie within one month and pay period will start from the first
of the month and ends with the last day of the month, for instance 01 March - 31 March is a valid pay period.
