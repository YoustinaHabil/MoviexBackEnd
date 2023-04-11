using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Movie_Web_Api.Models
{
    public class Actor_Movie
    {
        
        public int MovieId { get; set; }

       
       
        public Movie Movie { get; set; }
      
        public int ActorId { get; set; }
      
        public Actor Actor { get; set; }
    }
}
