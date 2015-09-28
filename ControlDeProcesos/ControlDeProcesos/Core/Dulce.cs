using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeProcesos.Core
{

    public class Dulce: Proceso
    {
        public Dulce():base() {
            
        }

        public static string[] Tipos = {
            "Chocolate",
            "Vainilla",
            "Cacahuate",
            "Limon",
            "Fresa",
            "Tamarindo",
            "Mango",
            "Durazno",
            "Manzana",
            "Café",
        };
        public string Tipo { get; set; }


        public override string ToString()
        {
            //return "Id: "+Id+", Tu dulce es de " + Tipo+", "+Tiempo+"s";
            return "Id: "+Id+", Tu dulce es de " + Tipo + ", t: "+Tiempo;
        }
    }
}
