using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Business
    {
        public string Address { get; }
        public string[] Hours { get; set; }
        public string[] Reviews { get; }
        public Boolean IsOpen { get; set; }
        public List<string> Tags { get; }
        public double Position { get; }
        private double Stars;
        
    }