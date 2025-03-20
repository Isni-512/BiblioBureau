using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopitalDB.Models
{
    public class Hopital
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }

        public ObservableCollection<Medecin> Medecins { get; set;}
        public ObservableCollection<Infirmier> infirmiers { get; set; }

    }
}
