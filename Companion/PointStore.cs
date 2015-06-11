using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using System.Runtime.Serialization.Json;
using System.Device.Location;


namespace Companion
{
    [DataContract]
    class PointStore 
    {
        [DataMember]
        private GeoCoordinate cord;

        [DataMember]
        public GeoCoordinate point
        {
            get;
            set;
        }

        public PointStore(GeoCoordinate point)
        {
            this.cord = point;
        }
    }


}
