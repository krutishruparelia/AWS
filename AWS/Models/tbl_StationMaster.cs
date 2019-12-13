namespace AWS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_StationMaster
    {
        public int ID { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Latitude { get; set; }

        [StringLength(1000)]
        public string Longitude { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string District { get; set; }

        [StringLength(100)]
        public string State { get; set; }

        [StringLength(255)]
        public string TehsilTaluk { get; set; }

        [StringLength(255)]
        public string Block { get; set; }

        [StringLength(150)]
        public string Village { get; set; }

        [Column(TypeName = "xml")]
        public string Image { get; set; }

        [StringLength(4000)]
        public string Address { get; set; }

        [StringLength(500)]
        public string Bank { get; set; }

        [StringLength(500)]
        public string BusStand { get; set; }

        [StringLength(500)]
        public string RailwayStation { get; set; }

        [StringLength(500)]
        public string Airport { get; set; }

        [StringLength(4000)]
        public string OtherInformation { get; set; }

        public int? StationCategoryID { get; set; }

        [StringLength(50)]
        public string Profile { get; set; }

        public string Gain { get; set; }

        public string Offset { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public bool? IsDeleted { get; set; }

        [StringLength(500)]
        public string StationID { get; set; }
        public string SerialNumber { get; set; }

    }
}
