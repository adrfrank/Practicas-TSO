using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeProcesos.Core
{
    public interface IFabricaProceso
    {
        Proceso CrearProceso(int id, int lote, int prioridad=0, int tiempo=0);
    }

    public class FabricaDulce : IFabricaProceso
    {
        Random rdm = new Random();

        public Proceso CrearProceso(int id, int lote, int prioridad=0, int tiempo=1)
        {
            
            return new Dulce() { Id = id, Lote = lote, Prioridad = prioridad, Tiempo = tiempo, Tipo = Dulce.Tipos[rdm.Next(10)] };
        }

       
    }
}
