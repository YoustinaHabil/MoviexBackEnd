
using Movie_Web_Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Movie_Web_Api.Models
{
    public class Movie:IEntityBase
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImageURL { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public MovieCategory MovieCategory { get; set; }


        //Relationships
      //  [DataMember]
      //   [JsonIgnore]
        public List<Actor_Movie> Actors_Movies { get; set; }

        //Cinema
        public int CinemaId { get; set; }
        [ForeignKey("CinemaId")]
     //[JsonIgnore]
      //  [DataMember]
        public Cinema Cinema { get; set; }

        //Producer
     //   [JsonIgnore]
       // [DataMember]
        public int ProducerId { get; set; }
        [ForeignKey("ProducerId")]
     //      [JsonIgnore]
       // [DataMember]
        public Producer Producer { get; set; }
    }
}
