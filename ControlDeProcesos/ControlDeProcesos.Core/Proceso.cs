using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeProcesos.Core
{
    public class Proceso
    {
        public int Id { get; set; }

        public int Lote { get; set; }

        public int Tiempo { get; set; }

        public string Nombre { get { return ToString(); } }

        public int Prioridad { get; set; }

        public TimeSpan Time
        {
            get
            {
                return TimeSpan.FromSeconds(Tiempo);
            }
        }

        public override string ToString()
        {
            return "Proceso: "+Id+" Lote: "+Lote ;
        }
    }
}
