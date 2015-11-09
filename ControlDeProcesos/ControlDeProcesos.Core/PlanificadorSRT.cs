using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeProcesos.Core
{
    public class PlanificadorSRT : Planificador
    {
        public override void NextTime(DateTime time)
        {
            
            if (Bloqueado.Count > 0) {
                Bloqueado.First().TiempoBloqueado += time - lastTime;
                if (Bloqueado.First().TiempoBloqueado > Bloqueado.First().TiempoDeBloque)
                    Listo.Add(Bloqueado.Dequeue());
            }
            Listo = Listo.OrderBy(s => s.Tiempo).ToList();
            base.NextTime(time);
        }
    }
}
