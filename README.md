GeodesicLibrary
===============

A C# Net Standard library to implement the JavaScript geodesic scripts written by Chris Veness. The original JavaScript scripts can be obtained from http://www.movable-type.co.uk/scripts/latlong.html, along with further explanation of the maths behind them. The main changes are that I am using UnitsNet (https://github.com/InitialForce/UnitsNet) to simplify unit conversions such as degrees to radians, as well as to cater for differing length units (The original scripts are based around kilometres).

Usage
-----

The easiest way to install GeodesicLibrary is via NuGet: https://www.nuget.org/packages/GeodesicLibrary/

You need to add the following using statements:

```csharp
using Marknotgeorge.GeodesicLibrary;
using UnitsNet;
using UnitsNet.Units;
```

The library is based around a Position class, which is a simple latitude-longitude pair:

```csharp
// Arnos Grove Underground station. 
Position ArnosGrove = new Position(51.6163, -0.1335);
```

The methods I have implemented are:

* Distance, both great circle (the Haversine formula) and rhumb line (constant bearing) to a specified Position.
* Great circle initial and final bearing to a specified Position.
* Rhumb line bearing to a specified Position.
* Midpoint along a path between two positions, both great cirle and rhumb line
* Intersection of two great circle paths, given Positions and bearings. 
* Destination point given a distance and a bearing, both great circle and rhumb line. 

The source code gives summaries of the relevant methods.


Units
-----

Latitude and longitude are in degrees. Bearings are also in degrees, clockwise from North. Distances default to kilometres, but can optionally be any LengthUnit specified in UnitsNet. UnitsNet also allows custom units, but these haven't been tested.

Examples
--------

```csharp
// Given two Positions (ArnosGrove and Hammersmith) get the great circle distance in km, initial and final bearings,
// and the midpoint.
double distance = ArnosGrove.DistanceTo(Hammersmith);
double initialBearing = ArnosGrove.InitialBearing(Hammersmith);
double finalBearing = ArnosGrove.FinalBearing(Hammersmith);
Position midPoint = ArnosGrove.MidpointTo(Hammersmith);

// Show the position 5km due east of ArnosGrove, using a great circle
Position dueEast = ArnosGrove.Destination(90, 5); // East is 90 degrees.

// The distance between ArnosGrove and Hammersmith along a rhumb line (a line of constant bearing) in feet.
double distance = ArnosGrove.RhumbDistanceTo(Hammersmith, LengthUnit.Foot);

// Find the intersection between two great circle paths - one due West from ArnosGrove and one due North from 
// Hammersmith. 
Position intersection = ArnosGrove.Intersection(270, Hammersmith, 0); // West is 270 degrees, North is 0 degrees.
```

Licence
-------

GeodesicLibrary is licenced under the MIT Licence

