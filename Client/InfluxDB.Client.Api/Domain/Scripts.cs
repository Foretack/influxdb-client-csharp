/* 
 * InfluxDB OSS API Service
 *
 * The InfluxDB v2 API provides a programmatic interface for all interactions with InfluxDB. Access the InfluxDB API using the `/api/v2/` endpoint. 
 *
 * OpenAPI spec version: 2.0.0
 * 
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenAPIDateConverter = InfluxDB.Client.Api.Client.OpenAPIDateConverter;

namespace InfluxDB.Client.Api.Domain
{
    /// <summary>
    /// Scripts
    /// </summary>
    [DataContract]
    public partial class Scripts : IEquatable<Scripts>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Scripts" /> class.
        /// </summary>
        /// <param name="scripts">scripts.</param>
        public Scripts(List<Script> scripts = default)
        {
            _Scripts = scripts;
        }

        /// <summary>
        /// Gets or Sets _Scripts
        /// </summary>
        [DataMember(Name = "scripts", EmitDefaultValue = false)]
        public List<Script> _Scripts { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Scripts {\n");
            sb.Append("  _Scripts: ").Append(_Scripts).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return Equals(input as Scripts);
        }

        /// <summary>
        /// Returns true if Scripts instances are equal
        /// </summary>
        /// <param name="input">Instance of Scripts to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Scripts input)
        {
            if (input == null)
            {
                return false;
            }

            return
                _Scripts == input._Scripts ||
                _Scripts != null &&
                _Scripts.SequenceEqual(input._Scripts);
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;

                if (_Scripts != null)
                {
                    hashCode = hashCode * 59 + _Scripts.GetHashCode();
                }

                return hashCode;
            }
        }
    }
}