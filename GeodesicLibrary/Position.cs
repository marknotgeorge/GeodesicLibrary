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
        /// <param name="toPosition">The destination point.</param>
        /// <param name="unit">The unit of the distance. Defaults to kilometres</param>
        /// <returns></returns>
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


    }
}