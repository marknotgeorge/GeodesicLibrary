//-----------------------------------------------------------------------
// <copyright file="Position.cs" company="marknotgeorge">
//     ©2014 Mark Johnson//
// </copyright>
// Based on Javascript code written by and ©2002-2-12 Chris Veness
// http://www.movable-type.co.uk/scripts/latlong.html
//-----------------------------------------------------------------------
using System;
using UnitsNet;
using UnitsNet.Units;

namespace Marknotgeorge.GeodesicLibrary
{
    public class Position
    {
        private Angle _latitude;

        /// <summary>
        /// Latitude in degrees
        /// </summary>
        public double Latitude
        {
            get
            {
                return _latitude.Degrees;
            }
            set
            {
                _latitude = Angle.FromDegrees(value);
            }
        }

        private Angle _longitude;

        /// <summary>
        /// Longitude in degrees
        /// </summary>
        public double Longitude
        {
            get
            {
                return _longitude.Degrees;
            }
            set
            {
                _longitude = Angle.FromDegrees(value);
            }
        }

        private Length _radius = Length.FromKilometers(6371);

        /// <summary>
        /// Initialise a Position, using the default radius of 6371km.
        /// </summary>
        /// <param name="lat">Latitude in degrees</param>
        /// <param name="lon">Longitude in degrees</param>
        public Position(double lat, double lon)
        {
            Latitude = lat;
            Longitude = lon;
        }

        /// <summary>
        /// Initialise a Position with a specific radius.
        /// </summary>
        /// <param name="lat">Latitude in degrees.</param>
        /// <param name="lon">Longitude in degrees.</param>
        /// <param name="radius">Radius of Earth in kilometres.</param>
        public Position(double lat, double lon, double radius)
        {
            Latitude = lat;
            Longitude = lon;
            _radius = Length.FromKilometers(radius);
        }

        /// <summary>
        /// Returns the distance from this Position to the supplied Position, using the Haversine
        /// formula.
        /// </summary>
        /// <param name="toPosition">The destination Position.</param>
        /// <param name="unit">The unit of the distance. Defaults to kilometres</param>
        /// <returns>The distance, in the units specified by the unit parameter</returns>
        public double DistanceTo(Position toPosition, LengthUnit unit = LengthUnit.Kilometer)
        {
            var lat1 = this._latitude.Radians; var lat2 = toPosition._latitude.Radians;
            var lon1 = this._longitude.Radians; var lon2 = toPosition._longitude.Radians;
            var R = _radius.As(unit);

            var dLat = lat2 - lat1;
            var dLon = lon2 - lon1;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + 
                Math.Cos(lat1) * Math.Cos(lat2) * 
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c;

            return d;
        }   
  
        /// <summary>
        /// Returns the (initial) bearing from this position to the supplied position, in degrees
        /// see http://williams.best.vwh.net/avform.htm#Crs
        /// </summary>
        /// <param name="toPosition">The destination Position</param>
        /// <returns>Initial bearing, in degrees clockwise from North</returns>        
        public double InitialBearing(Position toPosition)
        {
            var lat1 = this._latitude.Radians; var lat2 = toPosition._latitude.Radians;
            var dLon = (toPosition._longitude - this._longitude).Radians;

            var y = Math.Sin(dLon) * Math.Cos(lat2);
            var x = Math.Cos(lat1) * Math.Sin(lat2) -
                    Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
            Angle brng = Angle.FromRadians(Math.Atan2(y, x));

            return (brng.Degrees + 360) % 360;
        }

        /// <summary>
        /// Returns final bearing arriving at supplied destination Position from this Position; the final bearing 
        /// will differ from the initial bearing by varying degrees according to distance and latitude.
        /// </summary>
        /// <param name="toPosition">The destination Position</param>
        /// <returns>Final bearing, in degrees clockwise from North</returns>
        public double FinalBearing(Position toPosition)
        {
            // get initial bearing from supplied point back to this point...
            var lat1 = toPosition._latitude.Radians; var lat2 = this._latitude.Radians;
            var dLon = (this._longitude - toPosition._longitude).Radians;

            var y = Math.Sin(dLon) * Math.Cos(lat2);
            var x = Math.Cos(lat1) * Math.Sin(lat2) -
                    Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
            Angle brng = Angle.FromRadians(Math.Atan2(y, x));

            // ... & reverse it by adding 180°
            return (brng.Degrees + 180) % 360;
        }

        /// <summary>
        /// Returns the midpoint between this Position and the supplied Position.
        /// see http://mathforum.org/library/drmath/view/51822.html for derivation
        /// </summary>
        /// <param name="toPosition">The destination Position</param>
        /// <returns>Midpoint between this Position and the supplied Position</returns>
        public Position MidpointTo (Position toPosition)
        {
            var lat1 = this._latitude.Radians; var lon1 = this._longitude.Radians;
            var lat2 = toPosition._latitude.Radians;
            var dLon = (toPosition._longitude - this._longitude).Radians;

            var Bx = Math.Cos(lat2) * Math.Cos(dLon);
            var By = Math.Cos(lat2) * Math.Sin(dLon);

            Angle lat3 = Angle.FromRadians(Math.Atan2(Math.Sin(lat1) + Math.Sin(lat2),
                    Math.Sqrt((Math.Cos(lat1)+Bx) * (Math.Cos(lat1)+Bx) + By*By)));
            var midLon = lon1 + Math.Atan2(By, Math.Cos(lat1) + Bx);
            Angle lon3 = Angle.FromRadians((midLon + 3 * Math.PI) % (2 * Math.PI) - Math.PI);  // normalise to -180..+180º
  
            return new Position(lat3.Degrees, lon3.Degrees);
        }

        /// <summary>
        /// Returns the destination point from this point having travelled the given distance on the 
        /// given initial bearing (bearing may vary before destination is reached)
        /// </summary>
        /// <param name="brng">Initial bearing in degrees</param>
        /// <param name="dist">Distance</param>
        /// <param name="unit">Unit of distance - defaults to km</param>
        /// <returns></returns>
        public Position Destination(double brng, double dist, LengthUnit unit = LengthUnit.Kilometer)
        {
            Angle bearing = Angle.FromDegrees(brng);
            Length distance = Length.From(dist, unit);

            // Convert distance to angular distance in radians
            double angularDistance = distance.As(unit) / this._radius.As(unit);

            var lat1 = this._latitude.Radians; var lon1 = this._longitude.Radians;

            Angle lat2 = Angle.FromRadians(Math.Asin(Math.Sin(lat1) * Math.Cos(angularDistance) + 
                Math.Cos(lat1) * Math.Sin(angularDistance) * Math.Cos(bearing.Radians)));

            var midLon = lon1 + Math.Atan2(Math.Sin(bearing.Radians) * Math.Sin(angularDistance) * Math.Cos(lat1),
                               Math.Cos(angularDistance) - Math.Sin(lat1) * Math.Sin(lat2.Radians));

            Angle lon2 = Angle.FromRadians((midLon + 3 * Math.PI) % (2 * Math.PI) - Math.PI);

            return new Position(lat2.Degrees, lon2.Degrees);
        }

        /// <summary>
        /// Returns the point of intersection of two paths defined by Position and bearing
        ///
        /// see http://williams.best.vwh.net/avform.htm#Intersection
        /// </summary>        
        /// <param name="firstBearing">Initial bearing from first point in degrees</param>
        /// <param name="secondPoint">Second Position</param>
        /// <param name="secondBearing">Initial bearing from second point in degrees</param>
        /// <returns></returns>
        public Position Intersection(double firstBearing, Position secondPoint, double secondBearing)
        {
            Angle bearing1 = Angle.FromDegrees(firstBearing);
            Angle bearing2 = Angle.FromDegrees(secondBearing);

            double lat1 = this._latitude.Radians; double lon1 = this._longitude.Radians;
            double lat2 = secondPoint._latitude.Radians; double lon2 = secondPoint._longitude.Radians;
            double dLat = lat2 - lat1; double dLon = lon2 - lon1;

            double dist12 = 2 * Math.Asin(Math.Sqrt(Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1) * Math.Cos(lat2) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2)));
            if (dist12 == 0) 
                return null;

            // initial/final bearings between points, in radians
            double brngA = Math.Acos((Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(dist12)) /
              (Math.Sin(dist12) * Math.Cos(lat1)));
            if (double.IsNaN(brngA))
                brngA = 0;  // protect against rounding
            double brngB = Math.Acos((Math.Sin(lat1) - Math.Sin(lat2) * Math.Cos(dist12)) /
              (Math.Sin(dist12) * Math.Cos(lat2)));

            double brng12; double brng21;
            
            if (Math.Sin(lon2 - lon1) > 0)
            {
                brng12 = brngA;
                brng21 = 2 * Math.PI - brngB;
            }
            else
            {
                brng12 = 2 * Math.PI - brngA;
                brng21 = brngB;
            }

            double alpha1 = (bearing1.Radians - brng12 + Math.PI) % (2 * Math.PI) - Math.PI;  // angle 2-1-3
            double alpha2 = (brng21 - bearing2.Radians + Math.PI) % (2 * Math.PI) - Math.PI;  // angle 1-2-3

            if (Math.Sin(alpha1) == 0 && Math.Sin(alpha2) == 0) 
                return null;  // infinite intersections
            if (Math.Sin(alpha1) * Math.Sin(alpha2) < 0) 
                return null;  // ambiguous intersection

            //alpha1 = Math.abs(alpha1);
            //alpha2 = Math.abs(alpha2);
            // ... Ed Williams takes abs of alpha1/alpha2, but seems to break calculation?

            double alpha3 = Math.Acos( -Math.Cos(alpha1) * Math.Cos(alpha2) + 
                Math.Sin(alpha1) * Math.Sin(alpha2) * Math.Cos(dist12) );
            double dist13 = Math.Atan2(Math.Sin(dist12) * Math.Sin(alpha1) * Math.Sin(alpha2), 
                Math.Cos(alpha2) + Math.Cos(alpha1) * Math.Cos(alpha3));

            Angle lat3 = Angle.FromRadians(Math.Asin(Math.Sin(lat1) * Math.Cos(dist13) + 
                Math.Cos(lat1) * Math.Sin(dist13) * Math.Cos(bearing1.Radians)));
            double dLon13 = Math.Atan2(Math.Sin(bearing1.Radians) * Math.Sin(dist13) * Math.Cos(lat1), 
                Math.Cos(dist13) - Math.Sin(lat1) * Math.Sin(lat3.Radians));
            double midLon = lon1 + dLon13;
            Angle lon3 = Angle.FromRadians((midLon + 3 * Math.PI) % (2 * Math.PI) - Math.PI);

            return new Position(lat3.Degrees, lon3.Degrees);
        }


    }
}